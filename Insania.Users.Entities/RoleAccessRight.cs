using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Users.Entities.Base;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности права доступа роли
/// </summary>
[Table("u_roles_access_rights")]
[Comment("Права доступ ролей")]
public class RoleAccessRight : EntityAccessRight
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности права доступа роли
    /// </summary>
    public RoleAccessRight() : base()
    {
        RoleEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности права доступа роли без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="Role" name="role">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public RoleAccessRight(string username, AccessRight accessRight, Role role, DateTime? dateDeleted = null) : base(username, accessRight, dateDeleted)
    {
        RoleEntity = role;
        RoleId = role.Id;
    }

    /// <summary>
    /// Конструктор абстрактной модели сущности права доступа роли с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="Role" name="role">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public RoleAccessRight(long id, string username, AccessRight accessRight, Role role, DateTime? dateDeleted = null) : base(id, username, accessRight, dateDeleted)
    {
        RoleEntity = role;
        RoleId = role.Id;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор роли
    /// </summary>
    [Column("role_id")]
    [Comment("Идентификатор роли")]
    public long RoleId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство роли
    /// </summary>
    public Role RoleEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи роли
    /// </summary>
    /// <param cref="Role" name="role">Роль</param>
    public void SetUser(Role role)
    {
        RoleEntity = role;
        RoleId = role.Id;
    }
    #endregion
}