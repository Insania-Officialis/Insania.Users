using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities.Base;

/// <summary>
/// Абстрактная модель сущности права доступа сущности
/// </summary>
public abstract class EntityAccessRight : Entity
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор абстрактной модели сущности права доступа сущности
    /// </summary>
    public EntityAccessRight() : base()
    {
        AccessRightEntity = new();
    }

    /// <summary>
    /// Конструктор абстрактной модели сущности права доступа сущности без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public EntityAccessRight(string username, AccessRight accessRight, DateTime? dateDeleted = null) : base(username, dateDeleted)
    {
        AccessRightEntity = accessRight;
        AccessRightId = accessRight.Id;
    }

    /// <summary>
    /// Конструктор абстрактной сущности права доступа сущности с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public EntityAccessRight(long id, string username, AccessRight accessRight, DateTime? dateDeleted = null) : base(id, username, dateDeleted)
    {
        AccessRightEntity = accessRight;
        AccessRightId = accessRight.Id;

    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор права доступа
    /// </summary>
    [Column("access_right_id")]
    [Comment("Идентификатор права доступа")]
    public long AccessRightId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство права доступа
    /// </summary>
    public AccessRight AccessRightEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи права доступа
    /// </summary>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    public void SetAccessRight(AccessRight accessRight)
    {
        AccessRightEntity = accessRight;
        AccessRightId = accessRight.Id;
    }
    #endregion
}