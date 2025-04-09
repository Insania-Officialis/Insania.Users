using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Users.Entities;

/// <summary>
/// Модель сущности пользователя
/// </summary>
[Table("r_users")]
[Comment("Пользователи")]
public class User : Reestr
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности пользователи
    /// </summary>
    public User() : base()
    {
        Login = string.Empty;
        Password = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности пользователи без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string?" name="login">Логин</param>
    /// <param cref="string?" name="password">Пароль</param>
    /// <param cref="string?" name="phone">Номер телефона</param>
    /// <param cref="string?" name="email">Электронная почта</param>
    /// <param cref="string?" name="linkVK">Ссылка в ВК</param>
    /// <param cref=string?" name="lastName">Фамилия</param>
    /// <param cref="string?" name="firstName">Имя</param>
    /// <param cref="string?" name="patronymic">Отчество</param>
    /// <param cref="bool?" name="gender">Пол(истина - мужской/ложь - женский/null - неизвестно)</param>
    /// <param cref="string?" name="birthDate">Дата рождения</param>
    /// <param cref="bool" name="isBlocked">Признак блокировки пользователя</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public User(string username, bool isSystem, string login, string password, string? phone = null, string? email = null, string? linkVK = null, string? lastName = null, string? firstName = null, string? patronymic = null, bool? gender = null, DateOnly? birthDate = null, bool isBlocked = false, DateTime? dateDeleted = null) : base(username, isSystem, dateDeleted)
    {
        Login = login;
        Password = password;
        Phone = phone;
        Email = email;
        LinkVK = linkVK;
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        Gender = gender;
        BirthDate = birthDate;
        IsBlocked = isBlocked;
    }

    /// <summary>
    /// Конструктор модели сущности пользователи с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string?" name="login">Логин</param>
    /// <param cref="string?" name="password">Пароль</param>
    /// <param cref="string?" name="phone">Номер телефона</param>
    /// <param cref="string?" name="email">Электронная почта</param>
    /// <param cref="string?" name="linkVK">Ссылка в ВК</param>
    /// <param cref=string?" name="lastName">Фамилия</param>
    /// <param cref="string?" name="firstName">Имя</param>
    /// <param cref="string?" name="patronymic">Отчество</param>
    /// <param cref="bool?" name="gender">Пол(истина - мужской/ложь - женский/null - неизвестно)</param>
    /// <param cref="string?" name="birthDate">Дата рождения</param>
    /// <param cref="bool" name="isBlocked">Признак блокировки пользователя</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public User(long id, string username, bool isSystem, string login, string password, string? phone = null, string? email = null, string? linkVK = null, string? lastName = null, string? firstName = null, string? patronymic = null, bool? gender = null, DateOnly? birthDate = null, bool isBlocked = false, DateTime? dateDeleted = null) : base(id, username, isSystem, dateDeleted)
    {
        Login = login;
        Password = password;
        Phone = phone;
        Email = email;
        LinkVK = linkVK;
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        Gender = gender;
        BirthDate = birthDate;
        IsBlocked = isBlocked;
    }
    #endregion

    #region Поля
    /// <summary>
    ///	Логин
    /// </summary>
    [Column("login")]
    [Comment("Логин")]
    public string Login { get; private set; }

    /// <summary>
    ///	Пароль
    /// </summary>
    [Column("password")]
    [Comment("Пароль")]
    public string Password { get; private set; }

    /// <summary>
    ///	Номер телефона
    /// </summary>
    [Column("phone")]
    [Comment("Номер телефона")]
    public string? Phone { get; private set; }

    /// <summary>
    ///	Электронная почта
    /// </summary>
    [Column("email")]
    [Comment("Электронная почта")]
    public string? Email { get; private set; }

    /// <summary>
    ///	Ссылка в ВК
    /// </summary>
    [Column("link_vk")]
    [Comment("Ссылка в ВК")]
    public string? LinkVK { get; private set; }

    /// <summary>
    ///	Фамилия
    /// </summary>
    [Column("last_name")]
    [Comment("Фамилия")]
    public string? LastName { get; private set; }

    /// <summary>
    ///	Имя
    /// </summary>
    [Column("first_name")]
    [Comment("Имя")]
    public string? FirstName { get; private set; }

    /// <summary>
    /// Отчество
    /// </summary>
    [Column("patronymic")]
    [Comment("Отчество")]
    public string? Patronymic { get; private set; }

    /// <summary>
    /// Пол
    /// </summary>
    /// <remarks
    ///(истина - мужской/ложь - женский/null - неизвестно)
    /// </remarks>
    [Column("gender")]
    [Comment("Пол(истина - мужской/ложь - женский/null - неизвестно)")]
    public bool? Gender { get; private set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    [Column("birth_date")]
    [Comment("Дата рождения")]
    public DateOnly? BirthDate { get; private set; }

    /// <summary>
    /// Признак блокировки пользователя
    /// </summary>
    [Column("is_blocked")]
    [Comment("Признак блокировки пользователя")]
    public bool IsBlocked { get; private set; }
    #endregion

    #region Автогенерируемые поля
    /// <summary>
    /// Полное имя
    /// </summary>
    [NotMapped]
    public string? FullName
    {
        get => string.IsNullOrWhiteSpace(LastName) ? Login : LastName + " " + (!string.IsNullOrWhiteSpace(FirstName) ? FirstName + " " : string.Empty) + (!string.IsNullOrWhiteSpace(Patronymic) ? Patronymic + " " : string.Empty);
    }

    /// <summary>
    /// Инициалы
    /// </summary>
    [NotMapped]
    public string? Initials
    {
        get => string.IsNullOrWhiteSpace(LastName) ? Login : LastName + " " + (!string.IsNullOrWhiteSpace(FirstName) ? FirstName[0] + ". " : string.Empty) + (!string.IsNullOrWhiteSpace(Patronymic) ? Patronymic[0] + ". " : string.Empty);
    }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи логина
    /// </summary>
    /// <param cref="string" name="login">Логин</param>
    public void SetLogin(string login)
    {
        Login = login;
    }

    /// <summary>
    /// Метод записи пароля
    /// </summary>
    /// <param cref="string" name="password">Пароль</param>
    public void SetPassword(string password)
    {
        Password = password;
    }

    /// <summary>
    /// Метод записи номера телефона
    /// </summary>
    /// <param cref="string?" name="phone">Номер телефона</param>
    public void SetPhone(string? phone)
    {
        Phone = phone;
    }

    /// <summary>
    /// Метод записи электронной почты
    /// </summary>
    /// <param cref="string?" name="email">Электронная почта</param>
    public void SetEmail(string? email)
    {
        Email = email;
    }

    /// <summary>
    /// Метод записи ссылки в ВК
    /// </summary>
    /// <param cref="string?" name="linkVK">Ссылка в ВК</param>
    public void SetLinkVK(string? linkVK)
    {
        LinkVK = linkVK;
    }

    /// <summary>
    /// Метод записи фамилии
    /// </summary>
    /// <param cref="string?" name="lastName">Фамилия</param>
    public void SetLastName(string? lastName)
    {
        LastName = lastName;
    }

    /// <summary>
    /// Метод записи имени
    /// </summary>
    /// <param cref="string?" name="firstName">Имя</param>
    public void SetFirstName(string? firstName)
    {
        FirstName = firstName;
    }

    /// <summary>
    /// Метод записи отчества
    /// </summary>
    /// <param cref="string?" name="patronymic">Отчество</param>
    public void SetPatronymic(string? patronymic)
    {
        Patronymic = patronymic;
    }

    /// <summary>
    /// Метод записи пола
    /// </summary>
    /// <param cref="bool?" name="gender">Пол(истина - мужской/ложь - женский/null - неизвестно)</param>
    public void SetGender(bool? gender)
    {
        Gender = gender;
    }

    /// <summary>
    /// Метод записи даты рождения
    /// </summary>
    /// <param cref="DateOnly?" name="birthDate">Дата рождения</param>
    public void SetBirthDate(DateOnly? birthDate)
    {
        BirthDate = birthDate;
    }

    /// <summary>
    /// Метод записи признака блокировки пользователя
    /// </summary>
    /// <param cref="bool" name="isBlocked">Признак блокировки пользователя</param>
    public void SetIsBlocked(bool isBlocked)
    {
        IsBlocked = isBlocked;
    }
    #endregion
}