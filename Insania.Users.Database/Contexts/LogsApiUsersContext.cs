using Microsoft.EntityFrameworkCore;

using Insania.Users.Entities;

namespace Insania.Users.Database.Contexts;

/// <summary>
/// Контекст бд логов сервиса пользователей
/// </summary>
public class LogsApiUsersContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд логов сервиса пользователей
    /// </summary>
    public LogsApiUsersContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд логов сервиса пользователей с опциями
    /// </summary>
    /// <param cref="DbContextOptions{LogsApiUsersContext}" name="options">Параметры</param>
    public LogsApiUsersContext(DbContextOptions<LogsApiUsersContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Пользователи
    /// </summary>
    public virtual DbSet<LogApiUsers> Logs { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_logs_api_users");
        
        //Добавление gin-индекса на поле с входными данными логов
        modelBuilder.Entity<LogApiUsers>().HasIndex(x => x.DataIn).HasMethod("gin");

        //Добавление gin-индекса на поле с выходными данными логов
        modelBuilder.Entity<LogApiUsers>().HasIndex(x => x.DataOut).HasMethod("gin");
    }
    #endregion
}