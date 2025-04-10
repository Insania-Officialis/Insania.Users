using Insania.Users.Models.Responses;

namespace Insania.Users.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой аутентификации
/// </summary>
public interface IAuthenticationBL
{
    /// <summary>
    /// Метод аутентификации
    /// </summary>
    /// <param cref="string?" name="login">Логин</param>
    /// <param cref="string?" name="password">Пароль</param>
    /// <returns cref="AuthenticationInfo?">Информация об аутентификации</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<AuthenticationInfo?> Authentication(string? login, string? password);
}