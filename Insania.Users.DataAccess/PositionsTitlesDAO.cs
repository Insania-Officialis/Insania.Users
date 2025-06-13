using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными званий должностей
/// </summary>
/// <param cref="ILogger{PositionsTitlesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class PositionsTitlesDAO(ILogger<PositionsTitlesDAO> logger, UsersContext context) : IPositionsTitlesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<PositionsTitlesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка званий должностей
    /// </summary>
    /// <returns cref="List{PositionTitle}">Список званий должностей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<PositionTitle>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListPositionsTitlesMethod);

            //Получение данных из бд
            List<PositionTitle> data = await _context.PositionsTitles.Where(x => x.DateDeleted == null).ToListAsync();

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