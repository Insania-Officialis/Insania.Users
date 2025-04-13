using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности капитула
/// </summary>
[Table("r_chapters")]
[Comment("Капитулы")]
public class Chapter : Reestr
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности капитула
    /// </summary>
    public Chapter() : base()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности капитула без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="Chapter?" name="parent">Родитель</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Chapter(string username, bool isSystem, string name, Chapter? parent, DateTime? dateDeleted = null) : base(username, isSystem, dateDeleted)
    {
        Name = name;
        Parent = parent;
        ParentId = parent?.Id;
    }

    /// <summary>
    /// Конструктор модели сущности капитула с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="Chapter?" name="parent">Родитель</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Chapter(long id, string username, bool isSystem, string name, Chapter? parent, DateTime? dateDeleted = null) : base(id, username, isSystem, dateDeleted)
    {
        Name = name;
        Parent = parent;
        ParentId = parent?.Id;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Идентификатор родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Идентификатор родителя")]
    public long? ParentId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public Chapter? Parent { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param cref="string" name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param cref="Chapter?" name="parent">Родитель</param>
    public void SetUser(Chapter? parent)
    {
        Parent = parent;
        ParentId = parent?.Id;
    }
    #endregion
}