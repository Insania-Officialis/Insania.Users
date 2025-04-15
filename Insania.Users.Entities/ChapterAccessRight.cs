using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Users.Entities.Base;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности права доступа капитула
/// </summary>
[Table("u_chapters_access_rights")]
[Comment("Права доступ капитулов")]
public class ChapterAccessRight : EntityAccessRight
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности права доступа капитула
    /// </summary>
    public ChapterAccessRight() : base()
    {
        ChapterEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности права доступа капитула без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="Chapter" name="chapter">Капитул</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ChapterAccessRight(string username, AccessRight accessRight, Chapter chapter, DateTime? dateDeleted = null) : base(username, accessRight, dateDeleted)
    {
        ChapterEntity = chapter;
        ChapterId = chapter.Id;
    }

    /// <summary>
    /// Конструктор абстрактной модели сущности права доступа капитула с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="AccessRight" name="accessRight">Право доступа</param>
    /// <param cref="Chapter" name="chapter">Капитул</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ChapterAccessRight(long id, string username, AccessRight accessRight, Chapter chapter, DateTime? dateDeleted = null) : base(id, username, accessRight, dateDeleted)
    {
        ChapterEntity = chapter;
        ChapterId = chapter.Id;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор капитула
    /// </summary>
    [Column("chapter_id")]
    [Comment("Идентификатор капитула")]
    public long ChapterId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство капитула
    /// </summary>
    public Chapter ChapterEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи капитула
    /// </summary>
    /// <param cref="Chapter" name="chapter">Капитул</param>
    public void SetUser(Chapter chapter)
    {
        ChapterEntity = chapter;
        ChapterId = chapter.Id;
    }
    #endregion
}