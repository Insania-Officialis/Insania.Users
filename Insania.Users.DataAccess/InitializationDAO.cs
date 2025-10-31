using System.Text.RegularExpressions;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Npgsql;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;

using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Models.Settings;

using InformationMessages = Insania.Shared.Messages.InformationMessages;
using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис инициализации данных в бд пользователей
/// </summary>
/// <param cref="ILogger{InitializationDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="usersContext">Контекст базы данных пользователей</param>
/// <param cref="LogsApiUsersContext" name="logsApiUsersContext">Контекст базы данных логов сервиса пользователей</param>
/// <param cref="IOptions{InitializationDataSettings}" name="settings">Параметры инициализации данных</param>
/// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
/// <param cref="IConfiguration" name="configuration">Конфигурация приложения</param>
public class InitializationDAO(ILogger<InitializationDAO> logger, UsersContext usersContext, LogsApiUsersContext logsApiUsersContext, IOptions<InitializationDataSettings> settings, ITransliterationSL transliteration, IConfiguration configuration) : IInitializationDAO
{
    #region Поля
    private readonly string _username = "initializer";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<InitializationDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _usersContext = usersContext;

    /// <summary>
    /// Контекст базы данных логов сервиса пользователей
    /// </summary>
    private readonly LogsApiUsersContext _logsApiUsersContext = logsApiUsersContext;

    /// <summary>
    /// Параметры инициализации данных
    /// </summary>
    private readonly IOptions<InitializationDataSettings> _settings = settings;

    /// <summary>
    /// Сервис транслитерации
    /// </summary>
    private readonly ITransliterationSL _transliteration = transliteration;

    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    private readonly IConfiguration _configuration = configuration;
    #endregion

    #region Методы
    /// <summary>
    /// Метод инициализации данных
    /// </summary>
    /// <exception cref="Exception">Исключение</exception>
    public async Task Initialize()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredInitializeMethod);

            //Инициализация структуры
            if (_settings.Value.InitStructure == true)
            {
                //Логгирование
                _logger.LogInformation("{text}", InformationMessages.InitializationStructure);

                //Инициализация баз данных в зависимости от параметров
                if (_settings.Value.Databases?.Users == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("UsersSever") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_users_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("UsersEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_users_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }
                if (_settings.Value.Databases?.LogsApiUsers == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("LogsApiUsersServer") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_logs_api_users_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("LogsApiUsersEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_logs_api_users_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }

                //Выход
                return;
            }

            //Накат миграций
            if (_usersContext.Database.IsRelational()) await _usersContext.Database.MigrateAsync();
            if (_logsApiUsersContext.Database.IsRelational()) await _logsApiUsersContext.Database.MigrateAsync();

            //Проверки
            if (string.IsNullOrWhiteSpace(_settings.Value.ScriptsPath)) throw new Exception(ErrorMessagesShared.EmptyScriptsPath);

            //Инициализация данных в зависимости от параметров
            if (_settings.Value.Tables?.Roles == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<Role> entities =
                    [
                        new(_transliteration, 1, _username, "Гость"),
                        new(_transliteration, 2, _username, "Администратор"),
                        new(_transliteration, 3, _username, "Игрок"),
                        new(_transliteration, 4, _username, "Удалённая", DateTime.UtcNow),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Roles.Any(x => x.Id == entity.Id)) await _usersContext.Roles.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Users == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<User> entities =
                    [
                        new(1, _username, true, "test", "1"),
                        new(2, _username, true, "deleted", "1", dateDeleted: DateTime.UtcNow),
                        new(3, _username, true, "blocked", "1", isBlocked: true),
                        new(4, _username, true, "player", "1"),
                        new(5, _username, true, "guest", "1"),
                        new(6, _username, true, "administrator", "1"),
                        new(7, _username, true, "for_synchronization", "1"),
                        new(8, _username, true, "deleted_player", "1"),
                        new(9, _username, true, "deleted_administrator", "1")
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Users.Any(x => x.Id == entity.Id)) await _usersContext.Users.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.AccessRights == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<AccessRight> entities =
                    [
                        new(_transliteration, 1, _username, "Удалённая", "",  "",DateTime.UtcNow),
                        new(_transliteration, 2, _username, "Проверка логина", "users", "check_login"),
                        new(_transliteration, 3, _username, "Получение списка наций по идентификатору расы", "races", "list"),
                        new(_transliteration, 4, _username, "Получение списка рас", "nations", "list"),
                        new(_transliteration, 5, _username, "Получение списка типов файлов", "files_types", "list"),
                        new(_transliteration, 6, _username, "Получение списка файлов по идентификатору сущности и идентификатору типа", "files", "list"),
                        new(_transliteration, 7, _username, "Получение файла по идентификатору", "files", "by_id"),
                        new(_transliteration, 8, _username, "Получение списка стран", "countries", "list"),
                        new(_transliteration, 9, _username, "Получение списка фракций", "factions", "list"),
                        new(_transliteration, 10, _username, "Получение списка новостей", "news", "list"),
                        new(_transliteration, 11, _username, "Получение списка географических объектов", "geography_objects", "list"),
                        new(_transliteration, 12, _username, "Получение списка координат географических объектов", "geography_objects_coordinates", "by_geography_object_id"),
                        new(_transliteration, 13, _username, "Получение списка координат страны по идентификатору страны", "countries_coordinates", "by_country_id"),
                        new(_transliteration, 14, _username, "Метод актуализации координаты географического объекта", "geography_objects_coordinates", "upgrade"),
                        new(_transliteration, 15, _username, "Метод актуализации координаты страны", "countries_coordinates", "upgrade"),
                        new(_transliteration, 16, _username, "Получение списка географических объектов с координатами", "geography_objects", "list_with_coordinates"),
                        new(_transliteration, 17, _username, "Получение списка стран с координатами", "countries", "list_with_coordinates"),
                        new(_transliteration, 18, _username, "Получение списка рас с нациями", "races", "list_with_nations"),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.AccessRights.Any(x => x.Id == entity.Id)) await _usersContext.AccessRights.AddAsync(entity);
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_access_rights_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _usersContext);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Players == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "4", "0", ""],
                        ["2", "8", "0", DateTime.UtcNow.ToString()]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Players.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            User user = await _usersContext.Users.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundUser);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            Player entity = new(long.Parse(key[0]), _username, true, user, int.Parse(key[2]), dateDeleted);

                            //Добавление сущности в бд
                            await _usersContext.Players.AddAsync(entity);
                        }
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.RolesAccessRights == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "4", "1", DateTime.UtcNow.ToString()],
                        ["2", "1", "2", ""],
                        ["3", "1", "3", ""],
                        ["4", "1", "4", ""],
                        ["5", "1", "5", ""],
                        ["6", "1", "6", ""],
                        ["7", "1", "7", ""],
                        ["8", "1", "8", ""],
                        ["9", "1", "9", ""],
                        ["10", "1", "10", ""],
                        ["11", "2", "2", ""],
                        ["12", "2", "3", ""],
                        ["13", "2", "4", ""],
                        ["14", "2", "5", ""],
                        ["15", "2", "6", ""],
                        ["16", "2", "7", ""],
                        ["17", "2", "8", ""],
                        ["18", "2", "9", ""],
                        ["19", "2", "10", ""],
                        ["20", "3", "2", ""],
                        ["21", "3", "3", ""],
                        ["22", "3", "4", ""],
                        ["23", "3", "5", ""],
                        ["24", "3", "6", ""],
                        ["25", "3", "7", ""],
                        ["26", "3", "8", ""],
                        ["27", "3", "9", ""],
                        ["28", "3", "10", ""],
                        ["29", "1", "11", ""],
                        ["30", "2", "11", ""],
                        ["31", "3", "11", ""],
                        ["32", "1", "12", ""],
                        ["33", "2", "12", ""],
                        ["34", "3", "12", ""],
                        ["35", "1", "13", ""],
                        ["36", "2", "13", ""],
                        ["37", "3", "13", ""],
                        ["38", "2", "14", ""],
                        ["39", "2", "15", ""],
                        ["40", "1", "16", ""],
                        ["41", "2", "16", ""],
                        ["42", "3", "16", ""],
                        ["43", "1", "17", ""],
                        ["44", "2", "17", ""],
                        ["45", "3", "17", ""],
                        ["46", "1", "18", ""],
                        ["47", "2", "18", ""],
                        ["48", "3", "18", ""],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.RolesAccessRights.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            Role role = await _usersContext.Roles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundRole);
                            AccessRight accessRight = await _usersContext.AccessRights.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesUsers.NotFoundAccessRight);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            RoleAccessRight entity = new(long.Parse(key[0]), _username, accessRight, role, dateDeleted);

                            //Добавление сущности в бд
                            await _usersContext.RolesAccessRights.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_roles_access_rights_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _usersContext);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.UsersRoles == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "4", "3", ""],
                        ["2", "5", "1", ""],
                        ["3", "6", "2", ""],
                        ["4", "2", "4", DateTime.UtcNow.ToString()]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.UsersRoles.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            User user = await _usersContext.Users.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundUser);
                            Role role = await _usersContext.Roles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesUsers.NotFoundRole);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            UserRole entity = new(long.Parse(key[0]), _username, user, role, dateDeleted);

                            //Добавление сущности в бд
                            await _usersContext.UsersRoles.AddAsync(entity);
                        }
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Positions == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<Position> entities =
                    [
                        new(_transliteration, 1, _username, "Демиург", "Автор и создатель проекта"),
                        new(_transliteration, 2, _username, "Магистр", "Управляющий капитулом"),
                        new(_transliteration, 3, _username, "Комтур", "Управляющий администрацией капитула"),
                        new(_transliteration, 4, _username, "Распорядитель", "Ответственный за приём игроков"),
                        new(_transliteration, 5, _username, "Мейстер", "Ответственный за гражданское и политическое судейство"),
                        new(_transliteration, 6, _username, "Маршал", "Ответственный за военное судейство"),
                        new(_transliteration, 7, _username, "Инженер", "Ответственный за технологическое судейство"),
                        new(_transliteration, 8, _username, "Интендант", "Ответственный за экономическое судейство"),
                        new(_transliteration, 9, _username, "Архимаг", "Ответственный за магическое судейство"),
                        new(_transliteration, 10, _username, "Жрец", "Ответственный за религиозное судейство"),
                        new(_transliteration, 11, _username, "Бард", "Ответственный за культурное судейство"),
                        new(_transliteration, 12, _username, "Глашатай", "Ответственный за ведение новостной системы"),
                        new(_transliteration, 13, _username, "Посол", "Ответственный за рекламу и представительство"),
                        new(_transliteration, 14, _username, "Архивариус", "Ответственный за ведение статистики и хронологии"),
                        new(_transliteration, 15, _username, "Картограф", "Ответственный за ведение карты"),
                        new(_transliteration, 16, _username, "Гофмалер", "Ответственный за дизайн"),
                        new(_transliteration, 17, _username, "Механик", "Ответственный за функциональность системы"),
                        new(_transliteration, 18, _username, "Удалённая", null, DateTime.UtcNow)
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Positions.Any(x => x.Id == entity.Id)) await _usersContext.Positions.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Titles == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<Title> entities =
                    [
                        new(_transliteration, 1, _username, "Демиург", 1024),
                        new(_transliteration, 2, _username, "Верховный", 16),
                        new(_transliteration, 3, _username, "Главный", 8),
                        new(_transliteration, 4, _username, "Ведущий", 4),
                        new(_transliteration, 5, _username, "Старший", 2),
                        new(_transliteration, 6, _username, "Младший", 1),
                        new(_transliteration, 7, _username, "Фамилиар", 0.5),
                        new(_transliteration, 8, _username, "Удалённое", 0, DateTime.UtcNow),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Titles.Any(x => x.Id == entity.Id)) await _usersContext.Titles.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.PositionsTitles == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "1", "1", ""],
                        ["2", "2", "2", ""],
                        ["3", "2", "3", ""],
                        ["4", "2", "4", ""],
                        ["5", "2", "5", ""],
                        ["6", "2", "6", ""],
                        ["7", "2", "7", ""],
                        ["8", "3", "2", ""],
                        ["9", "3", "3", ""],
                        ["10", "3", "4", ""],
                        ["11", "3", "5", ""],
                        ["12", "3", "6", ""],
                        ["13", "3", "7", ""],
                        ["14", "4", "2", ""],
                        ["15", "4", "3", ""],
                        ["16", "4", "4", ""],
                        ["17", "4", "5", ""],
                        ["18", "4", "6", ""],
                        ["19", "4", "7", ""],
                        ["20", "5", "2", ""],
                        ["21", "5", "3", ""],
                        ["22", "5", "4", ""],
                        ["23", "5", "5", ""],
                        ["24", "5", "6", ""],
                        ["25", "5", "7", ""],
                        ["26", "6", "2", ""],
                        ["27", "6", "3", ""],
                        ["28", "6", "4", ""],
                        ["29", "6", "5", ""],
                        ["30", "6", "6", ""],
                        ["31", "6", "7", ""],
                        ["32", "7", "2", ""],
                        ["33", "7", "3", ""],
                        ["34", "7", "4", ""],
                        ["35", "7", "5", ""],
                        ["36", "7", "6", ""],
                        ["37", "7", "7", ""],
                        ["38", "8", "2", ""],
                        ["39", "8", "3", ""],
                        ["40", "8", "4", ""],
                        ["41", "8", "5", ""],
                        ["42", "8", "6", ""],
                        ["43", "8", "7", ""],
                        ["44", "9", "2", ""],
                        ["45", "9", "3", ""],
                        ["46", "9", "4", ""],
                        ["47", "9", "5", ""],
                        ["48", "9", "6", ""],
                        ["49", "9", "7", ""],
                        ["50", "10", "2", ""],
                        ["51", "10", "3", ""],
                        ["52", "10", "4", ""],
                        ["53", "10", "5", ""],
                        ["54", "10", "6", ""],
                        ["55", "10", "7", ""],
                        ["56", "11", "2", ""],
                        ["57", "11", "3", ""],
                        ["58", "11", "4", ""],
                        ["59", "11", "5", ""],
                        ["60", "11", "6", ""],
                        ["61", "11", "7", ""],
                        ["62", "12", "2", ""],
                        ["63", "12", "3", ""],
                        ["64", "12", "4", ""],
                        ["65", "12", "5", ""],
                        ["66", "12", "6", ""],
                        ["67", "12", "7", ""],
                        ["68", "13", "2", ""],
                        ["69", "13", "3", ""],
                        ["70", "13", "4", ""],
                        ["71", "13", "5", ""],
                        ["72", "13", "6", ""],
                        ["73", "13", "7", ""],
                        ["74", "14", "2", ""],
                        ["75", "14", "3", ""],
                        ["76", "14", "4", ""],
                        ["77", "14", "5", ""],
                        ["78", "14", "6", ""],
                        ["79", "14", "7", ""],
                        ["80", "15", "2", ""],
                        ["81", "15", "3", ""],
                        ["82", "15", "4", ""],
                        ["83", "15", "5", ""],
                        ["84", "15", "6", ""],
                        ["85", "15", "7", ""],
                        ["86", "16", "2", ""],
                        ["87", "16", "3", ""],
                        ["88", "16", "4", ""],
                        ["89", "16", "5", ""],
                        ["90", "16", "6", ""],
                        ["91", "16", "7", ""],
                        ["92", "17", "2", ""],
                        ["93", "17", "3", ""],
                        ["94", "17", "4", ""],
                        ["95", "17", "5", ""],
                        ["96", "17", "6", ""],
                        ["97", "17", "7", ""],
                        ["98", "18", "8", DateTime.UtcNow.ToString()],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.PositionsTitles.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            Position position = await _usersContext.Positions.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundPosition);
                            Title title = await _usersContext.Titles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesUsers.NotFoundTitle);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            PositionTitle entity = new(long.Parse(key[0]), _username, position, title, dateDeleted);

                            //Добавление сущности в бд
                            await _usersContext.PositionsTitles.AddAsync(entity);
                        }
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Administrators == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "6", "0", "97", ""],
                        ["2", "9", "0", "98", DateTime.UtcNow.ToString()]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Administrators.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            User user = await _usersContext.Users.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundUser);
                            PositionTitle positionTitle = await _usersContext.PositionsTitles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundPositionTitle);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[4])) dateDeleted = DateTime.Parse(key[4]);
                            Administrator entity = new(long.Parse(key[0]), _username, true, user, int.Parse(key[2]), positionTitle, dateDeleted);

                            //Добавление сущности в бд
                            await _usersContext.Administrators.AddAsync(entity);
                        }
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Chapters == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции родительских сущностей
                    List<Chapter> parentEntities =
                    [
                        new(1, _username, true, "Генеральный капитул", null),
                        new(24, _username, true, "Удалённый", null, DateTime.UtcNow)
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in parentEntities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Chapters.Any(x => x.Id == entity.Id)) await _usersContext.Chapters.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Создание коллекции дочерних сущностей
                    List<Chapter> entities =
                    [
                        new(2, _username, true, "Капитул Альвраатской империи", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(3, _username, true, "Капитул княжества Саорса", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(4, _username, true, "Капитул королевства Берген", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(5, _username, true, "Капитул Фесгарского княжества", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(6, _username, true, "Капитул Сверденского каганата", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(7, _username, true, "Капитул ханства Тавалин", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(8, _username, true, "Капитул княжества Саргиб", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(9, _username, true, "Капитул царства Банду", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(10, _username, true, "Капитул королевства Нордер", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(11, _username, true, "Капитул Альтерского княжества", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(12, _username, true, "Капитул Орлиадарской конфедерации", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(13, _username, true, "Капитул королевства Удстир", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(14, _username, true, "Капитул королевства Вервирунг", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(15, _username, true, "Капитул Дестинского ордена", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(16, _username, true, "Капитул вольного города Лийсет", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(17, _username, true, "Капитул Лисцийской империи", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(18, _username, true, "Капитул королевства Вальтир", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(19, _username, true, "Капитул вассального княжества Гратис", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(20, _username, true, "Капитул княжества Ректа", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(21, _username, true, "Капитул Волара", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(22, _username, true, "Капитул союза Иль-Ладро", parentEntities.FirstOrDefault(x => x.Id == 1)),
                        new(23, _username, true, "Капитул Мергерской унии", parentEntities.FirstOrDefault(x => x.Id == 1))
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Chapters.Any(x => x.Id == entity.Id)) await _usersContext.Chapters.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.PositionsTitlesAccessRights == true)
            {
                ////Открытие транзакции
                //IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                //try
                //{
                //    //Создание коллекции ключей
                //    string[][] keys =
                //    [
                //        ["1", "1", "1", ""],
                //        ["2", "2", "1", ""],
                //        ["3", "3", "1", ""],
                //        ["4", "4", "2", DateTime.UtcNow.ToString()]
                //    ];

                //    //Проход по коллекции ключей
                //    foreach (var key in keys)
                //    {
                //        //Добавление сущности в бд при её отсутствии
                //        if (!_usersContext.RolesAccessRights.Any(x => x.Id == long.Parse(key[0])))
                //        {
                //            //Получение сущностей
                //            Role role = await _usersContext.Roles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundRole);
                //            AccessRight accessRight = await _usersContext.AccessRights.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesUsers.NotFoundAccessRight);

                //            //Создание сущности
                //            DateTime? dateDeleted = null;
                //            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                //            RoleAccessRight entity = new(long.Parse(key[0]), _username, accessRight, role, dateDeleted);

                //            //Добавление сущности в бд
                //            await _usersContext.RolesAccessRights.AddAsync(entity);
                //        }
                //    }

                //    //Сохранение изменений в бд
                //    await _usersContext.SaveChangesAsync();

                //    //Фиксация транзакции
                //    transaction.Commit();
                //}
                //catch (Exception)
                //{
                //    //Откат транзакции
                //    transaction.Rollback();

                //    //Проброс исключения
                //    throw;
                //}
            }
            if (_settings.Value.Tables?.ChaptersAccessRights == true)
            {
                ////Открытие транзакции
                //IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                //try
                //{
                //    //Создание коллекции ключей
                //    string[][] keys =
                //    [
                //        ["1", "1", "1", ""],
                //        ["2", "2", "1", ""],
                //        ["3", "3", "1", ""],
                //        ["4", "4", "2", DateTime.UtcNow.ToString()]
                //    ];

                //    //Проход по коллекции ключей
                //    foreach (var key in keys)
                //    {
                //        //Добавление сущности в бд при её отсутствии
                //        if (!_usersContext.RolesAccessRights.Any(x => x.Id == long.Parse(key[0])))
                //        {
                //            //Получение сущностей
                //            Role role = await _usersContext.Roles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundRole);
                //            AccessRight accessRight = await _usersContext.AccessRights.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesUsers.NotFoundAccessRight);

                //            //Создание сущности
                //            DateTime? dateDeleted = null;
                //            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                //            RoleAccessRight entity = new(long.Parse(key[0]), _username, accessRight, role, dateDeleted);

                //            //Добавление сущности в бд
                //            await _usersContext.RolesAccessRights.AddAsync(entity);
                //        }
                //    }

                //    //Сохранение изменений в бд
                //    await _usersContext.SaveChangesAsync();

                //    //Фиксация транзакции
                //    transaction.Commit();
                //}
                //catch (Exception)
                //{
                //    //Откат транзакции
                //    transaction.Rollback();

                //    //Проброс исключения
                //    throw;
                //}
            }
            if (_settings.Value.Tables?.ChaptersAdministrators == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "1", "1", ""],
                        ["2", "24", "2", DateTime.UtcNow.ToString()]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.ChaptersAdministrators.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            Chapter chapter = await _usersContext.Chapters.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesUsers.NotFoundChapter);
                            Administrator administrator = await _usersContext.Administrators.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesUsers.NotFoundAdministrator);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            ChapterAdministrator entity = new(long.Parse(key[0]), _username, chapter, administrator, dateDeleted);

                            //Добавление сущности в бд
                            await _usersContext.ChaptersAdministrators.AddAsync(entity);
                        }
                    }

                    //Сохранение изменений в бд
                    await _usersContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Метод создание базы данных
    /// </summary>
    /// <param name="connectionServer">Строка подключения к серверу</param>
    /// <param name="patternDatabases">Шаблон файлов создания базы данных</param>
    /// <param name="connectionDatabase">Строка подключения к базе данных</param>
    /// <param name="patternSchemes">Шаблон файлов создания схемы</param>
    /// <returns></returns>
    private async Task CreateDatabase(string connectionServer, string patternDatabases, string connectionDatabase, string patternSchemes)
    {
        //Проход по всем скриптам в директории и создание баз данных
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternDatabases)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionServer);
        }

        //Проход по всем скриптам в директории и создание схем
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternSchemes)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionDatabase);
        }
    }

    /// <summary>
    /// Метод выполнения скрипта со строкой подключения
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="string" name="connectionString">Строка подключения</param>
    private async Task ExecuteScript(string filePath, string connectionString)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Создание соединения к бд
            using NpgsqlConnection connection = new(connectionString);

            //Открытие соединения
            connection.Open();

            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Создание sql-запроса
            using NpgsqlCommand command = new(sql, connection);

            //Выполнение команды
            await command.ExecuteNonQueryAsync();

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }

    /// <summary>
    /// Метод выполнения скрипта с контекстом
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="DbContext" name="context">Контекст базы данных</param>
    private async Task ExecuteScript(string filePath, DbContext context)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Выполнение sql-команды
            await context.Database.ExecuteSqlRawAsync(sql);

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }
    #endregion
}