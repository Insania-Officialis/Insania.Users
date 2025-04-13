using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными прав доступа ролей
/// </summary>
/// <param cref="ILogger{PositionsTitlesAccessRightsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class PositionsTitlesAccessRightsDAO(ILogger<PositionsTitlesAccessRightsDAO> logger, UsersContext context) : IPositionsTitlesAccessRightsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<PositionsTitlesAccessRightsDAO> _logger = logger;

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
    public async Task<List<PositionTitleAccessRight>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListPositionsTitlesAccessRightsMethod);

            //Получение данных из бд
            List<PositionTitleAccessRight> data = await _context.PositionsTitlesAccessRights.Where(x => x.DateDeleted == null).ToListAsync();

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