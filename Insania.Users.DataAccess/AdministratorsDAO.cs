using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными администраторов
/// </summary>
/// <param cref="ILogger{AdministratorsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class AdministratorsDAO(ILogger<AdministratorsDAO> logger, UsersContext context) : IAdministratorsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<AdministratorsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка игроков
    /// </summary>
    /// <returns cref="List{Administrator}">Список администраторов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Administrator>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListAdministratorsMethod);

            //Получение данных из бд
            List<Administrator> data = await _context.Administrators.Where(x => x.DateDeleted == null).ToListAsync();

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