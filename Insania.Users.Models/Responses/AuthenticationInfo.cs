using Insania.Shared.Models.Responses.Base;

namespace Insania.Users.Models.Responses;

/// <summary>
/// Модель информации об аутентификации
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="string?" name="token">Токен</param>
public class AuthenticationInfo(bool success, string? token) : BaseResponse(success)
{
    /// <summary>
    /// Токен доступа
    /// </summary>
    public string? Token { get; set; } = token;
}