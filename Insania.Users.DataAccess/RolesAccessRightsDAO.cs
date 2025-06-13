using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными прав доступа ролей
/// </summary>
/// <param cref="ILogger{RolesAccessRightsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class RolesAccessRightsDAO(ILogger<RolesAccessRightsDAO> logger, UsersContext context) : IRolesAccessRightsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<RolesAccessRightsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка прав доступа ролей
    /// </summary>
    /// <returns cref="List{RoleAccessRight}">Список прав доступа ролей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<RoleAccessRight>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListRolesAccessRightsMethod);

            //Получение данных из бд
            List<RoleAccessRight> data = await _context.RolesAccessRights.Where(x => x.DateDeleted == null).ToListAsync();

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