using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности звания
/// </summary>
[Table("c_titles")]
[Comment("Звания")]
public class Title : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности звания
    /// </summary>
    public Title() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности звания без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="double" name="coefficientAccrualHonorPoints">Коэффициент начисления баллов почёта</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Title(ITransliterationSL transliteration, string username, string name, double coefficientAccrualHonorPoints, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        CoefficientAccrualHonorPoints = coefficientAccrualHonorPoints;
    }

    /// <summary>
    /// Конструктор модели сущности звания с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="double" name="coefficientAccrualHonorPoints">Коэффициент начисления баллов почёта</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Title(ITransliterationSL transliteration, long id, string username, string name, double coefficientAccrualHonorPoints, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        CoefficientAccrualHonorPoints = coefficientAccrualHonorPoints;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Коэффициент начисления баллов почёта
    /// </summary>
    [Column("coefficient_accrual_honor_points")]
    [Comment("Коэффициент начисления баллов почёта")]
    public double CoefficientAccrualHonorPoints { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи коэффициента начисления баллов почёта
    /// </summary>
    /// <param cref="double" name="coefficientAccrualHonorPoints">Коэффициент начисления баллов почёта</param>
    public void SetCoefficientAccrualHonorPoints(double coefficientAccrualHonorPoints)
    {
        CoefficientAccrualHonorPoints = coefficientAccrualHonorPoints;
    }
    #endregion
}