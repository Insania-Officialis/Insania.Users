using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;

namespace Insania.Users.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с соединениями
/// </summary>
[Route("connections")]
public class ConnectionsController(ILogger<ConnectionsController> logger) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ConnectionsController> _logger = logger;
    #endregion

    #region Методы
    /// <summary>
    /// Метод проверки соединения
    /// </summary>
    /// <returns cref="OkResult">Успешно</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpHead]
    [Route("check")]
    public IActionResult Check()
    {
        try
        {
            //Возврат ответа
            return Ok();
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessages.Error, ex);

            //Возврат ошибки
            return BadRequest();
        }
    }
    #endregion
}