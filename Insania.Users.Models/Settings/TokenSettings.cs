namespace Insania.Users.Models.Settings;

/// <summary>
/// Модель параметров токена доступа
/// </summary>
public class TokenSettings
{
    /// <summary>
    /// Издатель
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// Слушатель
    /// </summary>
    public string? Audience { get; set; }

    /// <summary>
    /// Ключ
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// Срок действия
    /// </summary>
    public double? Expires { get; set; }
}