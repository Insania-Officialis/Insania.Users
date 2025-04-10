using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными ролей пользователей
/// </summary>
/// <param cref="ILogger{UsersRolesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class UsersRolesDAO(ILogger<UsersRolesDAO> logger, UsersContext context) : IUsersRolesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<UsersRolesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка ролей пользователей
    /// </summary>
    /// <returns cref="List{UserRole}">Список ролей пользователей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<UserRole>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListUsersRolesMethod);

            //Получение данных из бд
            List<UserRole> data = await _context.UsersRoles.Where(x => x.DateDeleted == null).ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}