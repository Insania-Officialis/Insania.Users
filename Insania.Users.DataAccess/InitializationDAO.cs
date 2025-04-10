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
using Insania.Users.Messages;
using Insania.Users.Models.Settings;

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
                    string connectionServer = _configuration.GetConnectionString("UsersSever") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
                    string patternDatabases = @"^databases_users_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("UsersEmpty") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
                    string patternSchemes = @"^schemes_users_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }
                if (_settings.Value.Databases?.LogsApiUsers == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("LogsApiUsersServer") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
                    string patternDatabases = @"^databases_logs_api_users_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("LogsApiUsersEmpty") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
                    string patternSchemes = @"^schemes_logs_api_users_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }

                //Выход
                return;
            }

            //Проверки
            if (string.IsNullOrWhiteSpace(_settings.Value.ScriptsPath)) throw new Exception(ErrorMessages.EmptyScriptsPath);

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
                        new(_transliteration, 1, _username, "Главная"),
                        new(_transliteration, 2, _username, "Удалённая", DateTime.UtcNow)
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.AccessRights.Any(x => x.Id == entity.Id)) await _usersContext.AccessRights.AddAsync(entity);
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
                        ["2", "4", "0", DateTime.UtcNow.ToString()]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.Players.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            User user = await _usersContext.Users.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessages.NotFoundUser);

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
                        ["1", "1", "1", ""],
                        ["2", "2", "1", ""],
                        ["3", "3", "1", ""],
                        ["4", "4", "2", DateTime.UtcNow.ToString()]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.RolesAccessRights.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            Role role = await _usersContext.Roles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessages.NotFoundRole);
                            AccessRight accessRight = await _usersContext.AccessRights.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessages.NotFoundAccessRight);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            RoleAccessRight entity = new(long.Parse(key[0]), _username, accessRight, role, dateDeleted);

                            //Добавление сущности в бд
                            await _usersContext.RolesAccessRights.AddAsync(entity);
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
            if (_settings.Value.Tables?.UsersRoles == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _usersContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "2", "1", DateTime.UtcNow.ToString()],
                        ["2", "4", "2", ""],
                        ["3", "5", "3", ""],
                        ["4", "6", "4", ""]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_usersContext.UsersRoles.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            User user = await _usersContext.Users.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessages.NotFoundUser);
                            Role role = await _usersContext.Roles.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessages.NotFoundRole);

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
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

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
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessages.NotExecutedScript, filePath, ex);
        }
    }
    #endregion
}