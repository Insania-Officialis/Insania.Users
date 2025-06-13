using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Contracts.BusinessLogic;
using Insania.Users.Entities;
using Insania.Users.Messages;
using Insania.Users.Models.Responses;
using Insania.Users.Models.Settings;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.BusinessLogic;

/// <summary>
/// Сервис аутентификации
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IUsersDAO" name="usersDAO">Сервис работы с данными пользователей</param>
/// <param  cref="IOptions{TokenSettings}" name="settings">Параметры токена</param>
public class AuthenticationBL(ILogger<AuthenticationBL> logger, IUsersDAO usersDAO, IOptions<TokenSettings> settings) : IAuthenticationBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    public readonly ILogger<AuthenticationBL> _logger = logger;

    /// <summary>
    /// Сервис работы с данными пользователей
    /// </summary>
    public readonly IUsersDAO _usersDAO = usersDAO;

    /// <summary>
    /// Параметры токена
    /// </summary>
    private readonly IOptions<TokenSettings> _settings = settings;
    #endregion

    #region Внешние методы
    /// <summary>
    /// Метод аутентификации
    /// </summary>
    /// <param cref="string?" name="login">Логин</param>
    /// <param cref="string?" name="password">Пароль</param>
    /// <returns cref="AuthenticationInfo?">Информация об аутентификации</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<AuthenticationInfo?> Authentication(string? login, string? password)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredAuthenticationMethod);

            //Проверки
            if (string.IsNullOrWhiteSpace(login)) throw new Exception(ErrorMessagesUsers.EmptyLogin);
            if (string.IsNullOrWhiteSpace(password)) throw new Exception(ErrorMessagesUsers.EmptyPassword);

            //Формирование переменной результата
            string? result = null;

            //Получение пользователя по логину
            User user = await _usersDAO.GetByLogin(login) ?? throw new Exception(ErrorMessagesUsers.NotFoundUser);

            //Проверки пользователя
            if (user.DateDeleted <= DateTime.Now) throw new Exception(ErrorMessagesUsers.DeletedUser);
            if (user.IsBlocked == true) throw new Exception(ErrorMessagesUsers.BlockedUser);
            if (user.Password != password) throw new Exception(ErrorMessagesUsers.IncorrectPassword);

            //Генерация токена
            result = CreateToken(login);

            //Возврат результата
            return new(true, result);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessagesShared.Error, ex);

            //Возврат ошибки
            throw;
        }
    }
    #endregion

    #region Внутренние методы
    /// <summary>
    /// Метод создания токена
    /// </summary>
    /// <param cref="string" name="login">Логин</param>
    /// <returns cref="string">Токен</returns>
    /// <exception cref="Exception">Исключение</exception>
    public string CreateToken(string login)
    {
        //Проверки
        if (string.IsNullOrWhiteSpace(login)) throw new Exception(ErrorMessages.EmptyLogin);

        //Получение параметров генерации токена
        var claims = new List<Claim> { new(ClaimTypes.Name, login) };
        if (string.IsNullOrWhiteSpace(_settings.Value.Issuer)) throw new Exception(ErrorMessages.EmptyIssuer);
        if (string.IsNullOrWhiteSpace(_settings.Value.Audience)) throw new Exception(ErrorMessages.EmptyAudience);
        if (_settings.Value.Expires == null) throw new Exception(ErrorMessages.EmptyExpires);
        if (string.IsNullOrWhiteSpace(_settings.Value.Key)) throw new Exception(ErrorMessages.EmptyKeyToken);

        //Создание JWT-токена
        var jwt = new JwtSecurityToken(
                issuer: _settings.Value.Issuer,
                audience: _settings.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_settings.Value.Expires ?? 0),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(_settings.Value.Key), SecurityAlgorithms.HmacSha256));

        //Возврат созданного токена
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// Метод генерации ключа
    /// </summary>
    /// <param cref="string" name="key">Ключ</param>
    /// <returns cref="SymmetricSecurityKey">Закодированный ключ</returns>
    private static SymmetricSecurityKey GetSymmetricSecurityKey(string key) => new(Encoding.UTF8.GetBytes(key));
    #endregion
}