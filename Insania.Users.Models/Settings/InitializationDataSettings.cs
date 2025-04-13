namespace Insania.Users.Models.Settings;

/// <summary>
/// Модель параметров инициализации данных
/// </summary>
public class InitializationDataSettings
{
    /// <summary>
    /// Признак инициализации структуры
    /// </summary>
    /// <remarks>
    /// Нужен для запуска миграций, при true не происходит инициализация данных
    /// </remarks>
    public bool? InitStructure { get; set; }

    /// <summary>
    /// Путь к файлам скриптов
    /// </summary>
    public string? ScriptsPath { get; set; }

    /// <summary>
    /// Включение в инициализацию таблиц
    /// </summary>
    public InitializationDataSettingsIncludeTables? Tables { get; set; }

    /// <summary>
    /// Включение в инициализацию баз данных
    /// </summary>
    public InitializationDataSettingsIncludeDatabases? Databases { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию таблиц
/// </summary>
public class InitializationDataSettingsIncludeTables
{
    /// <summary>
    /// Роли
    /// </summary>
    public bool? Roles { get; set; }

    /// <summary>
    /// Пользователи
    /// </summary>
    public bool? Users { get; set; }

    /// <summary>
    /// Права доступа
    /// </summary>
    public bool? AccessRights { get; set; }

    /// <summary>
    /// Игроки
    /// </summary>
    public bool? Players { get; set; }

    /// <summary>
    /// Права доступа ролей
    /// </summary>
    public bool? RolesAccessRights { get; set; }

    /// <summary>
    /// Роли пользователей
    /// </summary>
    public bool? UsersRoles { get; set; }

    /// <summary>
    /// Должности
    /// </summary>
    public bool? Positions { get; set; }

    /// <summary>
    /// Звания
    /// </summary>
    public bool? Titles { get; set; }

    /// <summary>
    /// Звания должностей
    /// </summary>
    public bool? PositionsTitles { get; set; }

    /// <summary>
    /// Администраторы
    /// </summary>
    public bool? Administrators { get; set; }

    /// <summary>
    /// Капитулы
    /// </summary>
    public bool? Chapters { get; set; }

    /// <summary>
    /// Права доступа званий должностей
    /// </summary>
    public bool? PositionsTitlesAccessRights { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию баз данных
/// </summary>
public class InitializationDataSettingsIncludeDatabases
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public bool? Users { get; set; }

    /// <summary>
    /// Логи сервиса пользователей
    /// </summary>
    public bool? LogsApiUsers { get; set; }
}