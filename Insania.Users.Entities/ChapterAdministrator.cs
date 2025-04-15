using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности администратора капитула
/// </summary>
[Table("u_chapters_administrators")]
[Comment("Роли пользователей")]
public class ChapterAdministrator : Entity
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности администратора капитула
    /// </summary>
    public ChapterAdministrator() : base()
    {
        ChapterEntity = new();
        AdministratorEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности администратора капитула без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="Chapter" name="chapter">Капитул</param>
    /// <param cref="Administrator" name="administrator">Администратор</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ChapterAdministrator(string username, Chapter chapter, Administrator administrator, DateTime? dateDeleted = null) : base(username, dateDeleted)
    {
        ChapterEntity = chapter;
        ChapterId = chapter.Id;
        AdministratorEntity = administrator;
        AdministratorId = administrator.Id;
    }

    /// <summary>
    /// Конструктор модели сущности администратора капитула с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="Chapter" name="chapter">Капитул</param>
    /// <param cref="Administrator" name="administrator">Администратор</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ChapterAdministrator(long id, string username, Chapter chapter, Administrator administrator, DateTime? dateDeleted = null) : base(id, username, dateDeleted)
    {
        ChapterEntity = chapter;
        ChapterId = chapter.Id;
        AdministratorEntity = administrator;
        AdministratorId = administrator.Id;

    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор капитула
    /// </summary>
    [Column("chapter_id")]
    [Comment("Идентификатор капитула")]
    public long ChapterId { get; private set; }

    /// <summary>
    /// Идентификатор администратора
    /// </summary>
    [Column("administrator_id")]
    [Comment("Идентификатор администратора")]
    public long AdministratorId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство капитула
    /// </summary>
    public Chapter ChapterEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство администратора
    /// </summary>
    public Administrator AdministratorEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи капитула
    /// </summary>
    /// <param cref="Chapter" name="chapter">Капитул</param>
    public void SetChapter(Chapter chapter)
    {
        ChapterEntity = chapter;
        ChapterId = chapter.Id;
    }

    /// <summary>
    /// Метод записи администратора
    /// </summary>
    /// <param cref="Administrator" name="administrator">Администратор</param>
    public void SetAdministrator(Administrator administrator)
    {
        AdministratorEntity = administrator;
        AdministratorId = administrator.Id;
    }
    #endregion
}