using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Shared.Services;

using Insania.Users.BusinessLogic;
using Insania.Users.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Models.Mapper;
using Insania.Users.Models.Settings;

namespace Insania.Users.Tests.Base;

/// <summary>
/// Базовый класс тестирования
/// </summary>
public abstract class BaseTest
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор базового класса тестирования
    /// </summary>
    public BaseTest()
    {
        //Создание коллекции сервисов
        IServiceCollection services = new ServiceCollection();

        //Создание коллекции ключей конфигурации
        Dictionary<string, string> configurationKeys = new()
        {
           {"LoggingOptions:FilePath", DetermineLogPath()},
           {"InitializationDataSettings:ScriptsPath", DetermineScriptsPath()},
           {"InitializationDataSettings:InitStructure", "false"},
           {"InitializationDataSettings:Tables:Roles", "true"},
           {"InitializationDataSettings:Tables:Users", "true"},
           {"InitializationDataSettings:Tables:AccessRights", "true"},
           {"InitializationDataSettings:Tables:Players", "true"},
           {"InitializationDataSettings:Tables:RolesAccessRights", "true"},
           {"InitializationDataSettings:Tables:UsersRoles", "true"},
           {"InitializationDataSettings:Tables:Positions", "true"},
           {"InitializationDataSettings:Tables:Titles", "true"},
           {"InitializationDataSettings:Tables:PositionsTitles", "true"},
           {"InitializationDataSettings:Tables:Administrators", "true"},
           {"InitializationDataSettings:Tables:Chapters", "true"},
           {"InitializationDataSettings:Tables:PositionsTitlesAccessRights", "true"},
           {"InitializationDataSettings:Tables:ChaptersAccessRights", "true"},
           {"InitializationDataSettings:Tables:ChaptersAdministrators", "true"},
           {"TokenSettings:Issuer", "Users.Test"},
           {"TokenSettings:Audience", "Users.Test"},
           {"TokenSettings:Key", "This key is generated for tests in the user zone"},
           {"TokenSettings:Expires", "7"},
        };

        //Создание экземпляра конфигурации в памяти
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationKeys!).Build();

        //Установка игнорирования типов даты и времени
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //Внедрение зависимостей сервисов
        services.AddSingleton(_ => configuration); //конфигурация
        services.AddScoped<ITransliterationSL, TransliterationSL>(); //сервис транслитерации
        services.AddScoped<IInitializationDAO, InitializationDAO>(); //сервис инициализации данных в бд пользователей
        services.AddUsersBL(); //сервисы работы с бизнес-логикой в зоне пользователей

        //Добавление контекстов бд в коллекцию сервисов
        services.AddDbContext<UsersContext>(options => options.UseInMemoryDatabase(databaseName: "insania_users").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд пользователей
        services.AddDbContext<LogsApiUsersContext>(options => options.UseInMemoryDatabase(databaseName: "insania_logs_api_users").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд логов сервиса пользователей

        //Добавление параметров логирования
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        //Добавление параметров преобразования моделей
        services.AddAutoMapper(cfg => { cfg.AddProfile<UsersMappingProfile>(); });

        //Добавление параметров инициализации данных
        IConfigurationSection? initializationDataSettings = configuration.GetSection("InitializationDataSettings");
        services.Configure<InitializationDataSettings>(initializationDataSettings);

        //Добавление параметров токенов
        IConfigurationSection? tokenSettings = configuration.GetSection("TokenSettings");
        services.Configure<TokenSettings>(tokenSettings);

        //Создание поставщика сервисов
        ServiceProvider = services.BuildServiceProvider();

        //Выполнение инициализации данных
        IInitializationDAO initialization = ServiceProvider.GetRequiredService<IInitializationDAO>();
        initialization.Initialize().Wait();
    }
    #endregion

    #region Поля
    /// <summary>
    /// Поставщик сервисов
    /// </summary>
    protected IServiceProvider ServiceProvider { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод определения пути для логов
    /// </summary>
    /// <returns cref="string">Путь для сохранения логов</returns>
    private static string DetermineLogPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        //Возврат нужного пути
        if (isRunningInDocker) return "/logs/log.txt";
        else return "G:\\Program\\Insania\\Logs\\Users.Tests\\log.txt";
    }

    /// <summary>
    /// Метод определения пути для скриптов
    /// </summary>
    /// <returns cref="string">Путь к скриптам</returns>
    private static string DetermineScriptsPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        if (isRunningInDocker) return "/src/Insania.Users.Database/Scripts";
        else return "G:\\Program\\Insania\\Insania.Users\\Insania.Users.Database\\Scripts";
    }
    #endregion
}