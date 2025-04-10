using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным прав доступа
/// </summary>
public interface IAccessRightsDAO
{
    /// <summary>
    /// Метод получения списка прав доступа
    /// </summary>
    /// <returns cref="List{AccessRight}">Список прав доступа</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<AccessRight>> GetList();
}