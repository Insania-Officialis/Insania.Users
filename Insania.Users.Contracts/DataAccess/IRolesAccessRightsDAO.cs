using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным прав доступа ролей
/// </summary>
public interface IRolesAccessRightsDAO
{
    /// <summary>
    /// Метод получения списка прав доступа ролей
    /// </summary>
    /// <param cref="long?" name="roleId">Идентификатор роли</param>
    /// <returns cref="List{RoleAccessRight}">Список прав доступа ролей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<RoleAccessRight>> GetList(long? roleId = null);
}