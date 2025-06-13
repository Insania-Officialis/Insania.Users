using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Entities;
using Insania.Users.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Users.DataAccess;

/// <summary>
/// Сервис работы с данными ролей
/// </summary>
/// <param cref="ILogger{RolesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="UsersContext" name="context">Контекст базы данных пользователей</param>
public class RolesDAO(ILogger<RolesDAO> logger, UsersContext context) : IRolesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<RolesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly UsersContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка ролей
    /// </summary>
    /// <returns cref="List{Role}">Список ролей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Role>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListRolesMethod);

            //Получение данных из бд
            List<Role> data = await _context.Roles.Where(x => x.DateDeleted == null).ToListAsync();

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