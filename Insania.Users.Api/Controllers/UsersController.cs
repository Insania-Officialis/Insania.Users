using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Models.Responses.Base;

using Insania.Users.Contracts.BusinessLogic;
using Insania.Users.Messages;

namespace Insania.Users.Api.Controllers;

/// <summary>
/// Контроллер работы с пользователями
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IUsersBL" name="usersService">Сервис работы с бизнес-логикой пользователей</param>
[Route("users")]
public class UsersController(ILogger<UsersController> logger, IUsersBL usersService) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<UsersController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой пользователей
    /// </summary>
    private readonly IUsersBL _usersService = usersService;
    #endregion

    #region Методы
    /// <summary>
    /// Метод проверки логина
    /// </summary>
    /// <param cref="string" name="login">Логин</param>
    /// <returns cref="OkResult">Успешно. true - успешно, логин не найден; false - не успешно, логин найден</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("check_login")]
    public async Task<IActionResult> CheckLogin([FromQuery] string? login)
    {
        try
        {
            //Проверки
            if (string.IsNullOrWhiteSpace(login)) throw new Exception(ErrorMessages.EmptyLogin);

            //Получение результата проверки логина
            BaseResponse? result = await _usersService.CheckLogin(login);

            //Возврат ответа
            return Ok(result);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessages.Error, ex);

            //Возврат ошибки
            return BadRequest(new BaseResponseError(ex.Message));
        }
    }
    #endregion
}