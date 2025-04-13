using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным должностей
/// </summary>
public interface IPositionsDAO
{
    /// <summary>
    /// Метод получения списка должностей
    /// </summary>
    /// <returns cref="List{Position}">Список должностей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Position>> GetList();
}