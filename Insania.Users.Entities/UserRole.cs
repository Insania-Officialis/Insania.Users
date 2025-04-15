using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности роли пользователя
/// </summary>
[Table("u_users_roles")]
[Comment("Роли пользователей")]
public class UserRole : Entity
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности роли пользователя
    /// </summary>
    public UserRole() : base()
    {
        UserEntity = new();
        RoleEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности роли пользователя без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="User" name="user">Пользователь</param>
    /// <param cref="Role" name="role">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public UserRole(string username, User user, Role role, DateTime? dateDeleted = null) : base(username, dateDeleted)
    {
        UserEntity = user;
        UserId = user.Id;
        RoleEntity = role;
        RoleId = role.Id;
    }

    /// <summary>
    /// Конструктор модели сущности роли пользователя с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="User" name="user">Пользователь</param>
    /// <param cref="Role" name="role">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public UserRole(long id, string username, User user, Role role, DateTime? dateDeleted = null) : base(id, username, dateDeleted)
    {
        UserEntity = user;
        UserId = user.Id;
        RoleEntity = role;
        RoleId = role.Id;

    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [Column("user_id")]
    [Comment("Идентификатор пользователя")]
    public long UserId { get; private set; }

    /// <summary>
    /// Идентификатор роли
    /// </summary>
    [Column("role_id")]
    [Comment("Идентификатор роли")]
    public long RoleId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство пользователя
    /// </summary>
    public User UserEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство роли
    /// </summary>
    public Role RoleEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи пользователя
    /// </summary>
    /// <param cref="User" name="user">Пользователь</param>
    public void SetUser(User user)
    {
        UserEntity = user;
        UserId = user.Id;
    }

    /// <summary>
    /// Метод записи роли
    /// </summary>
    /// <param cref="Role" name="role">Роль</param>
    public void SetRole(Role role)
    {
        RoleEntity = role;
        RoleId = role.Id;
    }
    #endregion
}