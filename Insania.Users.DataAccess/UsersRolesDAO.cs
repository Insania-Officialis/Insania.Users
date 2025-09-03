using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

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
    /// <param cref="long?" name="userId">Идентификатор пользователя</param>
    /// <returns cref="List{UserRole}">Список ролей пользователей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<UserRole>> GetList(long? userId = null)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListUsersRolesMethod);

            //Формирование запроса
            IQueryable<UserRole> query = _context.UsersRoles.Where(x => x.DateDeleted == null);

            //Дополнение запроса, в зависимости от наличия идентификатора пользователя
            if (userId != null) query = query.Include(x => x.RoleEntity).Where(x => x.UserId == userId);

            //Получение данных из бд
            List<UserRole> data = await query.ToListAsync();

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