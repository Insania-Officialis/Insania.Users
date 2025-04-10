using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным игроков
/// </summary>
public interface IPlayersDAO
{
    /// <summary>
    /// Метод получения списка игроков
    /// </summary>
    /// <returns cref="List{Player}">Список игроков</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Player>> GetList();
}