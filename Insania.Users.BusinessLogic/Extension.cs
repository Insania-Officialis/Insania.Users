using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.BusinessLogic;
using Insania.Users.DataAccess;

namespace Insania.Users.BusinessLogic;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с бизнес-логикой в зоне пользователей
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с бизнес-логикой в зоне пользователей
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddUsersBL(this IServiceCollection services) =>
        services
            .AddUsersDAO() //сервисы работы с данными в зоне пользователей
            .AddScoped<IAuthenticationBL, AuthenticationBL>() //сервис работы с бизнес-логикой аутентификации
            .AddScoped<IUsersBL, UsersBL>() //сервис работы с бизнес-логикой пользователей
        ;
}