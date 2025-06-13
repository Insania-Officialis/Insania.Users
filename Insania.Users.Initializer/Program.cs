using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Serilog;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Shared.Messages;
using Insania.Shared.Services;

using Insania.Users.DataAccess;
using Insania.Users.Database.Contexts;
using Insania.Users.Models.Settings;

//Запуск хоста
CreateHostBuilder(args).Build().Run();

//Построение хоста
static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).ConfigureServices(async (hostContext, services) =>
    {
        //Добавление конфигурации в проект
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();

        //Установка игнорирования типов даты и времени
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //Внедрение зависимостей сервисов
        services.AddSingleton(configuration); //конфигурация
        services.AddScoped<ITransliterationSL, TransliterationSL>(); //сервис транслитерации
        services.AddScoped<IInitializationDAO, InitializationDAO>(); //сервис инициализации данных в бд пользователей

        //Добавление контекстов бд в коллекцию сервисов
        services.AddDbContext<UsersContext>(options =>
        {
            string connectionString = configuration.GetConnectionString("Users") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
            options.UseNpgsql(connectionString);
        }); //бд пользователей
        services.AddDbContext<LogsApiUsersContext>(options =>
        {
            string connectionString = configuration.GetConnectionString("LogsApiUsers") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
            options.UseNpgsql(connectionString);
        }); //бд логов сервиса пользователей

        //Добавление параметров логирования
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        //Добавление параметров инициализации данных
        IConfigurationSection? settings = configuration.GetSection("InitializationDataSettings");
        services.Configure<InitializationDataSettings>(settings);

        //Создание поставщика сервисов
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        //Инициализация данных, если не установлен признак инициализации структуры
        IOptions<InitializationDataSettings> initializeDataSettings = serviceProvider.GetRequiredService<IOptions<InitializationDataSettings>>();
        await serviceProvider.GetRequiredService<IInitializationDAO>().Initialize();
    }
);