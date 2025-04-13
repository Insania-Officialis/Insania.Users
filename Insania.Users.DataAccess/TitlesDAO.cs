using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными званий
/// </summary>
/// <param cref="ILogger{TitlesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class TitlesDAO(ILogger<TitlesDAO> logger, UsersContext context) : ITitlesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<TitlesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка званий
    /// </summary>
    /// <returns cref="List{Title}">Список званий</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Title>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListTitlesMethod);

            //Получение данных из бд
            List<Title> data = await _context.Titles.Where(x => x.DateDeleted == null).ToListAsync();

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