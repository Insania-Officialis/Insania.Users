using Insania.Shared.Models.Responses.Base;

namespace Insania.Users.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой пользователей
/// </summary>
public interface IUsersBL
{
    /// <summary>
    /// Метод проверки наличия логина
    /// </summary>
    /// <param cref="string?" name="login">Логин для проверки</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>success: true-указанный логин не существует; false-логин уже занят</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponse> CheckLogin(string? login);
}