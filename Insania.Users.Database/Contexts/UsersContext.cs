using Microsoft.EntityFrameworkCore;

using Insania.Users.Entities;

namespace Insania.Users.Database.Contexts;

/// <summary>
/// Контекст бд пользователей
/// </summary>
public class UsersContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд пользователей
    /// </summary>
    public UsersContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд пользователей с опциями
    /// </summary>
    /// <param cref="DbContextOptions{UsersContext}" name="options">Параметры</param>
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Пользователи
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// Роли
    /// </summary>
    public virtual DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Права доступа
    /// </summary>
    public virtual DbSet<AccessRight> AccessRights { get; set; }

    /// <summary>
    /// Роли пользователей
    /// </summary>
    public virtual DbSet<UserRole> UsersRoles { get; set; }

    /// <summary>
    /// Права доступа ролей
    /// </summary>
    public virtual DbSet<RoleAccessRight> RolesAccessRights { get; set; }

    /// <summary>
    /// Игроки
    /// </summary>
    public virtual DbSet<Player> Players { get; set; }

    /// <summary>
    /// Должности
    /// </summary>
    public virtual DbSet<Position> Positions { get; set; }

    /// <summary>
    /// Звания
    /// </summary>
    public virtual DbSet<Title> Titles { get; set; }

    /// <summary>
    /// Звания должностей
    /// </summary>
    public virtual DbSet<PositionTitle> PositionsTitles { get; set; }

    /// <summary>
    /// Администраторы
    /// </summary>
    public virtual DbSet<Administrator> Administrators { get; set; }

    /// <summary>
    /// Капитулы
    /// </summary>
    public virtual DbSet<Chapter> Chapters { get; set; }

    /// <summary>
    /// Права доступа званий должностей
    /// </summary>
    public virtual DbSet<PositionTitleAccessRight> PositionsTitlesAccessRights { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_users");

        //Создание ограничения уникальности на логин пользователя
        modelBuilder.Entity<User>().HasAlternateKey(x => x.Login);

        //Создание ограничения уникальности на псевдоним роли
        modelBuilder.Entity<Role>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним права доступа
        modelBuilder.Entity<AccessRight>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на роль пользователя
        modelBuilder.Entity<UserRole>().HasAlternateKey(x => new { x.RoleId, x.UserId });

        //Создание ограничения уникальности на право доступа роли
        modelBuilder.Entity<RoleAccessRight>().HasAlternateKey(x => new { x.AccessRightId, x.RoleId });

        //Создание ограничения уникальности на псевдоним должности
        modelBuilder.Entity<Position>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним звания
        modelBuilder.Entity<Title>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на звание должности
        modelBuilder.Entity<PositionTitle>().HasAlternateKey(x => new { x.PositionId, x.TitleId });

        //Создание ограничения уникальности на наименование капитула
        modelBuilder.Entity<Chapter>().HasAlternateKey(x => x.Name);

        //Создание ограничения уникальности на право доступа звания должности
        modelBuilder.Entity<PositionTitleAccessRight>().HasAlternateKey(x => new { x.AccessRightId, x.PositionTitleId });
    }
    #endregion
}