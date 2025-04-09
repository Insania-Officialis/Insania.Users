using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности игрока
/// </summary>
[Table("r_players")]
[Comment("Игроки")]
public class Player : Reestr
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности игрока
    /// </summary>
    public Player() : base()
    {
        UserEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности игрока без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="User" name="user">Пользователь</param>
    /// <param cref="int" name="loyaltyPoints">Баллы верности</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Player(string username, bool isSystem, User user, int loyaltyPoints, DateTime? dateDeleted = null) : base(username, isSystem, dateDeleted)
    {
        UserEntity = user;
        UserId = user.Id;
        LoyaltyPoints = loyaltyPoints;
    }

    /// <summary>
    /// Конструктор модели сущности игрока с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="User" name="user">Пользователь</param>
    /// <param cref="int" name="loyaltyPoints">Баллы верности</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Player(long id, string username, bool isSystem, User user, int loyaltyPoints, DateTime? dateDeleted = null) : base(id, username, isSystem, dateDeleted)
    {
        UserEntity = user;
        UserId = user.Id;
        LoyaltyPoints = loyaltyPoints;
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
    /// Баллы верности
    /// </summary>
    [Column("loyalty_points")]
    [Comment("Баллы верности")]
    public int LoyaltyPoints { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство пользователя
    /// </summary>
    public User UserEntity { get; private set; }
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
    /// Метод записи баллов верности
    /// </summary>
    /// <param cref="int" name="loyaltyPoints">Баллы верности</param>
    public void SetLoyaltyPoints(int loyaltyPoints)
    {
        LoyaltyPoints = loyaltyPoints;
    }
    #endregion
}