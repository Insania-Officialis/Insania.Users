using Insania.Users.Entities;

namespace Insania.Users.Contracts.Services;

/// <summary>
/// Интерфейс сервиса фонового логгирования в бд
/// </summary>
public interface ILoggingSL
{
    /// <summary>
    /// Метод постановки лога в очередь на обработку
    /// </summary>
    /// <param cref="LogApiUsers" name="log">Лог для записи</param>
    /// <returns cref="ValueTask">Задание</returns>
    ValueTask QueueLogAsync(LogApiUsers log);
}