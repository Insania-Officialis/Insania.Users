using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности администратора
/// </summary>
[Table("r_administrators")]
[Comment("Администраторы")]
public class Administrator : Reestr
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности администратора
    /// </summary>
    public Administrator() : base()
    {
        UserEntity = new();
        PositionTitleEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности администратора без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="User" name="user">Пользователь</param>
    /// <param cref="int" name="honorPoints">Баллы почёта</param>
    /// <param cref="PositionTitle" name="positionTitle">Звание должности</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Administrator(string username, bool isSystem, User user, int honorPoints, PositionTitle positionTitle, DateTime? dateDeleted = null) : base(username, isSystem, dateDeleted)
    {
        UserEntity = user;
        UserId = user.Id;
        HonorPoints = honorPoints;
        PositionTitleEntity = positionTitle;
        PositionTitleId = positionTitle.Id;
    }

    /// <summary>
    /// Конструктор модели сущности администратора с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="User" name="user">Пользователь</param>
    /// <param cref="int" name="honorPoints">Баллы почёта</param>
    /// <param cref="PositionTitle" name="positionTitle">Звание должности</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Administrator(long id, string username, bool isSystem, User user, int honorPoints, PositionTitle positionTitle, DateTime? dateDeleted = null) : base(id, username, isSystem, dateDeleted)
    {
        UserEntity = user;
        UserId = user.Id;
        HonorPoints = honorPoints;
        PositionTitleEntity = positionTitle;
        PositionTitleId = positionTitle.Id;
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
    /// Баллы почёта
    /// </summary>
    [Column("honor_points")]
    [Comment("Баллы почёта")]
    public int HonorPoints { get; private set; }

    /// <summary>
    /// Идентификатор звания должности
    /// </summary>
    [Column("position_title_id")]
    [Comment("Идентификатор звания должности")]
    public long PositionTitleId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство пользователя
    /// </summary>
    public User UserEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство звания должности
    /// </summary>
    public PositionTitle PositionTitleEntity { get; private set; }
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
    /// Метод записи баллов почёта
    /// </summary>
    /// <param cref="int" name="honorPoints">Баллы почёта</param>
    public void SetHonorPoints(int honorPoints)
    {
        HonorPoints = honorPoints;
    }

    /// <summary>
    /// Метод записи звания должности
    /// </summary>
    /// <param cref="PositionTitle" name="positionTitle">Звание должности</param>
    public void SetPositionTitle(PositionTitle positionTitle)
    {
        PositionTitleEntity = positionTitle;
        PositionTitleId = positionTitle.Id;
    }
    #endregion
}