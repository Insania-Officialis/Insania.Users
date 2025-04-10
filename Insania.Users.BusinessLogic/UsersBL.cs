using Microsoft.Extensions.Logging;

using Insania.Users.Contracts.BusinessLogic;
using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Messages;
using Insania.Shared.Models.Responses.Base;

namespace Insania.Users.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой пользователей
/// </summary>
/// <param cref="ILogger{UsersBL}" name="logger">Сервис логгирования</param>
/// <param cref="IUsersDAO" name="usersDAO">Сервис работы с данными пользователей</param>
public class UsersBL(ILogger<UsersBL> logger, IUsersDAO usersDAO) : IUsersBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<UsersBL> _logger = logger;

    /// <summary>
    /// Сервис работы с данными пользователей
    /// </summary>
    private readonly IUsersDAO _usersDAO = usersDAO;
    #endregion

    #region Методы
    /// <summary>
    /// Метод проверки наличия логина
    /// </summary>
    /// <param cref="string?" name="login">Логин для проверки</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>success: true-указанный логин не существует; false-логин уже занят</remarks>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponse> CheckLogin(string? login)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListUsersMethod);

            //Проверки
            if (string.IsNullOrWhiteSpace(login)) throw new Exception(ErrorMessages.EmptyLogin);

            //Получение данных
            User? data = await _usersDAO.GetByLogin(login);

            //Формирование ответа
            BaseResponse? response = null;
            if (data != null) response = new(false);
            else response = new(true);

            //Возврат ответа
            return response;
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