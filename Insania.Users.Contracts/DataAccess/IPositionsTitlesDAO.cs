using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным званий должностей
/// </summary>
public interface IPositionsTitlesDAO
{
    /// <summary>
    /// Метод получения списка званий должностей
    /// </summary>
    /// <returns cref="List{PositionTitle}">Список званий должностей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<PositionTitle>> GetList();
}