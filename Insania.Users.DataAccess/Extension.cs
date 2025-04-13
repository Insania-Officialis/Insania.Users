using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.DataAccess;

namespace Insania.Users.DataAccess;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с данными в зоне пользователей
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с данными в зоне пользователей
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddUsersDAO(this IServiceCollection services) =>
        services
            .AddScoped<IRolesDAO, RolesDAO>() //сервис работы с данными ролей
            .AddScoped<IUsersDAO, UsersDAO>() //сервис работы с данными пользователей
            .AddScoped<IAccessRightsDAO, AccessRightsDAO>() //сервис работы с данными прав доступа
            .AddScoped<IPlayersDAO, PlayersDAO>() //сервис работы с данными игроков
            .AddScoped<IRolesAccessRightsDAO, RolesAccessRightsDAO>() //сервис работы с данными прав доступа ролей
            .AddScoped<IUsersRolesDAO, UsersRolesDAO>() //сервис работы с данными ролей пользователей
            .AddScoped<IPositionsDAO, PositionsDAO>() //сервис работы с данными должностей
            .AddScoped<ITitlesDAO, TitlesDAO>() //сервис работы с данными званий
            .AddScoped<IPositionsTitlesDAO, PositionsTitlesDAO>() //сервис работы с данными званий должностей
            .AddScoped<IAdministratorsDAO, AdministratorsDAO>() //сервис работы с данными администраторов
            .AddScoped<IChaptersDAO, ChaptersDAO>() //сервис работы с данными капитулов
            .AddScoped<IPositionsTitlesAccessRightsDAO, PositionsTitlesAccessRightsDAO>() //сервис работы с данными прав доступа званий должностей
        ;
}