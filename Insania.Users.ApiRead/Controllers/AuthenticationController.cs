using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Models.Responses.Base;

using Insania.Users.Contracts.BusinessLogic;
using Insania.Users.Models.Responses;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с аутентификацией
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IAuthenticationBL" name="authentication">Сервис работы с бизнес-логикой аутентификации</param>
[Route("authentication")]
public class AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationBL authentication) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<AuthenticationController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой аутентификации
    /// </summary>
    private readonly IAuthenticationBL _authenticationService = authentication;
    #endregion

    #region Методы
    /// <summary>
    /// Метод аутентификации
    /// </summary>
    /// <param cref="string" name="login">Логин</param>
    /// <param cref="string" name="password">Пароль</param>
    /// <returns cref="OkResult">Успешно. Success: true - успешно; false - не успешно. Token: токен</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("login")]
    public async Task<IActionResult> Authentication([FromQuery] string? login, [FromQuery] string? password)
    {
        try
        {
            //Проверки
            if (string.IsNullOrWhiteSpace(login)) throw new Exception(ErrorMessagesUsers.EmptyLogin);
            if (string.IsNullOrWhiteSpace(password)) throw new Exception(ErrorMessagesUsers.EmptyPassword);

            //Получение результата аутентификации
            AuthenticationInfo? result = await _authenticationService.Authentication(login, password);

            //Возврат ответа
            return Ok(result);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessagesShared.Error, ex);

            //Возврат ошибки
            return BadRequest(new BaseResponseError(ex.Message));
        }
    }
    #endregion
}