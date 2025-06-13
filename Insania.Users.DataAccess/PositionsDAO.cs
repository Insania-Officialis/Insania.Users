using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными должностей
/// </summary>
/// <param cref="ILogger{PositionsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class PositionsDAO(ILogger<PositionsDAO> logger, UsersContext context) : IPositionsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<PositionsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка должностей
    /// </summary>
    /// <returns cref="List{Position}">Список должностей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Position>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListPositionsMethod);

            //Получение данных из бд
            List<Position> data = await _context.Positions.Where(x => x.DateDeleted == null).ToListAsync();

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