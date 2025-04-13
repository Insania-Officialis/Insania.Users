using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности звания должности
/// </summary>
[Table("u_positions_titles")]
[Comment("Звания должностей")]
public class PositionTitle : Entity
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности звания должности
    /// </summary>
    public PositionTitle() : base()
    {
        PositionEntity = new();
        TitleEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности звания должности без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="Role" name="role">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public PositionTitle(string username, Position position, Title title, DateTime? dateDeleted = null) : base(username, dateDeleted)
    {
        PositionEntity = position;
        PositionId = position.Id;
        TitleEntity = title;
        TitleId = title.Id;
    }

    /// <summary>
    /// Конструктор абстрактной модели сущности звания должности с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="Role" name="role">Роль</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public PositionTitle(long id, string username, Position position, Title title, DateTime? dateDeleted = null) : base(id, username, dateDeleted)
    {
        PositionEntity = position;
        PositionId = position.Id;
        TitleEntity = title;
        TitleId = title.Id;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор должности
    /// </summary>
    [Column("position_id")]
    [Comment("Идентификатор должности")]
    public long PositionId { get; private set; }

    /// <summary>
    /// Идентификатор звания
    /// </summary>
    [Column("title_id")]
    [Comment("Идентификатор звания")]
    public long TitleId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство должности
    /// </summary>
    public Position PositionEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство звания
    /// </summary>
    public Title TitleEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи должности
    /// </summary>
    /// <param cref="Position" name="position">Должность</param>
    public void SetPosition(Position position)
    {
        PositionEntity = position;
        PositionId = position.Id;
    }

    /// <summary>
    /// Метод записи звания
    /// </summary>
    /// <param cref="Title" name="title">Звание</param>
    public void SetPosition(Title title)
    {
        TitleEntity = title;
        TitleId = title.Id;
    }
    #endregion
}