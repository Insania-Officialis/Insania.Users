using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным прав доступа званий должностей
/// </summary>
public interface IPositionsTitlesAccessRightsDAO
{
    /// <summary>
    /// Метод получения списка прав доступа званий должностей
    /// </summary>
    /// <returns cref="List{PositionTitleAccessRight}">Список прав доступа званий должностей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<PositionTitleAccessRight>> GetList();
}