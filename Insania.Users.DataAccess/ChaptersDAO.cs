using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными капитулов
/// </summary>
/// <param cref="ILogger{ChaptersDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class ChaptersDAO(ILogger<ChaptersDAO> logger, UsersContext context) : IChaptersDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ChaptersDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка капитулов
    /// </summary>
    /// <returns cref="List{Chapter}">Список капитулов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Chapter>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListChaptersMethod);

            //Получение данных из бд
            List<Chapter> data = await _context.Chapters.Where(x => x.DateDeleted == null).ToListAsync();

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