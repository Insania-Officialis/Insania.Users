using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным званий
/// </summary>
public interface ITitlesDAO
{
    /// <summary>
    /// Метод получения списка званий
    /// </summary>
    /// <returns cref="List{Title}">Список званий</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Title>> GetList();
}