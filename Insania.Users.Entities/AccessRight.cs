using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Insania.Shared.Entities;
using Insania.Shared.Contracts.Services;

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

    }

    /// <summary>
    /// Конструктор модели сущности права доступа без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public AccessRight(ITransliterationSL transliteration, string username, string name, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности права доступа с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public AccessRight(ITransliterationSL transliteration, long id, string username, string name, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {

    }
    #endregion
}