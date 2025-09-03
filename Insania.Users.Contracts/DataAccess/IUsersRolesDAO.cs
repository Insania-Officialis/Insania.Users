using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным ролей пользователей
/// </summary>
public interface IUsersRolesDAO
{
    /// <summary>
    /// Метод получения списка ролей пользователей
    /// </summary>
    /// <param cref="long?" name="userId">Идентификатор пользователя</param>
    /// <returns cref="List{UserRole}">Список ролей пользователей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<UserRole>> GetList(long? userId = null);
}