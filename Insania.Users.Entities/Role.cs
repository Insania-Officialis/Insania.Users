using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;
using Insania.Shared.Contracts.Services;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности роли
/// </summary>
[Table("c_roles")]
[Comment("Роли")]
public class Role : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности роли
    /// </summary>
    public Role() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности роли без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Role(ITransliterationSL transliteration, string username, string name, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности роли с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Role(ITransliterationSL transliteration, long id, string username, string name, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {

    }
    #endregion
}