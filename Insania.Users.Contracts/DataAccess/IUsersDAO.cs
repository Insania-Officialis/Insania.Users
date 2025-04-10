using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными пользователей
/// </summary>
public interface IUsersDAO
{
    /// <summary>
    /// Метод получения пользователя по логину
    /// </summary>
    /// <param cref="string?" name="login">Логин</param>
    /// <returns cref="User?">Пользователь</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<User?> GetByLogin(string? login);

    /// <summary>
    /// Метод получения списка пользователей
    /// </summary>
    /// <returns cref="List{User}">Список пользователей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<User>> GetList();
}