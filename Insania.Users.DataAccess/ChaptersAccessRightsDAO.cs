using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными прав доступа капитулов
/// </summary>
/// <param cref="ILogger{RolesAccessRightsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class ChaptersAccessRightsDAO(ILogger<ChaptersAccessRightsDAO> logger, UsersContext context) : IChaptersAccessRightsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ChaptersAccessRightsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка прав доступа капитулов
    /// </summary>
    /// <returns cref="List{ChapterAccessRight}">Список прав доступа капитулов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<ChapterAccessRight>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListChaptersAccessRightsMethod);

            //Получение данных из бд
            List<ChapterAccessRight> data = await _context.ChaptersAccessRights.Where(x => x.DateDeleted == null).ToListAsync();

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