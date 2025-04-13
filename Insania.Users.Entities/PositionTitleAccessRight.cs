using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Users.Entities.Base;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности права доступа звания должности
/// </summary>
[Table("u_positions_titles_access_rights")]
[Comment("Права доступ званий должностей")]
public class PositionTitleAccessRight : EntityAccessRight
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности права доступа звания должности
    /// </summary>
    public PositionTitleAccessRight() : base()
    {
        PositionTitleEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности права доступа звания должности без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="PositionTitle" name="positionTitle">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public PositionTitleAccessRight(string username, AccessRight accessRight, PositionTitle positionTitle, DateTime? dateDeleted = null) : base(username, accessRight, dateDeleted)
    {
        PositionTitleEntity = positionTitle;
        PositionTitleId = positionTitle.Id;
    }

    /// <summary>
    /// Конструктор абстрактной модели сущности права доступа звания должности с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="PositionTitle" name="positionTitle">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public PositionTitleAccessRight(long id, string username, AccessRight accessRight, PositionTitle positionTitle, DateTime? dateDeleted = null) : base(id, username, accessRight, dateDeleted)
    {
        PositionTitleEntity = positionTitle;
        PositionTitleId = positionTitle.Id;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор звания должности
    /// </summary>
    [Column("position_title_id")]
    [Comment("Идентификатор звания должности")]
    public long PositionTitleId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство звания должности
    /// </summary>
    public PositionTitle PositionTitleEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи звания должности
    /// </summary>
    /// <param cref="PositionTitle" name="positionTitle">Роль</param>
    public void SetPositionTitle(PositionTitle positionTitle)
    {
        PositionTitleEntity = positionTitle;
        PositionTitleId = positionTitle.Id;
    }
    #endregion
}