using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными администраторов капитулов
/// </summary>
/// <param cref="ILogger{ChaptersAdministratorsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class ChaptersAdministratorsDAO(ILogger<ChaptersAdministratorsDAO> logger, UsersContext context) : IChaptersAdministratorsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ChaptersAdministratorsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка администраторов капитулов
    /// </summary>
    /// <returns cref="List{ChapterAdministrator}">Список администраторов капитулов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<ChapterAdministrator>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListChaptersAdministratorsMethod);

            //Получение данных из бд
            List<ChapterAdministrator> data = await _context.ChaptersAdministrators.Where(x => x.DateDeleted == null).ToListAsync();

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