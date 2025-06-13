using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными пользователей
/// </summary>
/// <param cref="ILogger{UsersDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class UsersDAO(ILogger<UsersDAO> logger, UsersContext context) : IUsersDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<UsersDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения пользователя по логину
    /// </summary>
    /// <param cref="string?" name="login">Логин</param>
    /// <returns cref="User?">Пользователь</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<User?> GetByLogin(string? login)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByLoginUserMethod);

            //Проверки
            if (string.IsNullOrWhiteSpace(login)) throw new Exception(ErrorMessagesUsers.EmptyLogin);

            //Получение данных из бд
            User? data = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);

            //Возврат результата
            return data;
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
    /// Метод получения списка пользователей
    /// </summary>
    /// <returns cref="List{User}">Список пользователей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<User>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListUsersMethod);

            //Получение данных из бд
            List<User> data = await _context.Users.Where(x => x.DateDeleted == null).ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}