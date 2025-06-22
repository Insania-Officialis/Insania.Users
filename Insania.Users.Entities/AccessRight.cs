using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности права доступа
/// </summary>
[Table("c_access_rights")]
[Comment("Права доступа")]
public class AccessRight : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности права доступа
    /// </summary>
    public AccessRight() : base()
    {
        Controller = string.Empty;
        Action = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности права доступа без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="controller">Контроллер</param>
    /// <param cref="string" name="action">Действие</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public AccessRight(ITransliterationSL transliteration, string username, string name, string controller, string action, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Controller = controller;
        Action = action;
    }

    /// <summary>
    /// Конструктор модели сущности права доступа с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="controller">Контроллер</param>
    /// <param cref="string" name="action">Действие</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public AccessRight(ITransliterationSL transliteration, long id, string username, string name, string controller, string action, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Controller = controller;
        Action = action;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Контроллер
    /// </summary>
    [Column("controller")]
    [Comment("Контроллер")]
    public string Controller { get; protected set; }

    /// <summary>
    /// Действие
    /// </summary>
    [Column("action")]
    [Comment("Действие")]
    public string Action { get; protected set; }
    #endregion

    #region Методы

    /// <summary>
    /// Метод установки зоны
    /// </summary>
    /// <param cref="string" name="controller">Контроллер</param>
    public void SetController(string controller) => Controller = controller;

    /// <summary>
    /// Метод установки действия
    /// </summary>
    /// <param cref="string" name="action">Действие</param>
    public void SetAction(string action) => Action = action;
    #endregion
}