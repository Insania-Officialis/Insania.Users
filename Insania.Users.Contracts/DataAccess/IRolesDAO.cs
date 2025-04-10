using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным ролей
/// </summary>
public interface IRolesDAO
{
    /// <summary>
    /// Метод получения списка ролей
    /// </summary>
    /// <returns cref="List{Role}">Список ролей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Role>> GetList();
}