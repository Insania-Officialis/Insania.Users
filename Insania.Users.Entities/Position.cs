using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности должности
/// </summary>
[Table("c_positions")]
[Comment("Должности")]
public class Position : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности должности
    /// </summary>
    public Position() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности должности без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    /// <param cref="string?" name="activityArea">Сфера деятельности</param>
    public Position(ITransliterationSL transliteration, string username, string name, string? activityArea = null, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        ActivityArea = activityArea;
    }

    /// <summary>
    /// Конструктор модели сущности должности с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    /// <param cref="string?" name="activityArea">Сфера деятельности</param>
    public Position(ITransliterationSL transliteration, long id, string username, string name, string? activityArea = null, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        ActivityArea = activityArea;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Сфера деятельности
    /// </summary>
    [Column("activity_area")]
    [Comment("Сфера деятельности")]
    public string? ActivityArea { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи сферы деятельности
    /// </summary>
    /// <param cref="string?" name="activityArea">Сфера деятельности</param>
    public void SetActivityArea(string? activityArea)
    {
        ActivityArea = activityArea;
    }
    #endregion
}