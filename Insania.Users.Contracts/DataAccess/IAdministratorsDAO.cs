using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным администраторов
/// </summary>
public interface IAdministratorsDAO
{
    /// <summary>
    /// Метод получения списка администраторов
    /// </summary>
    /// <returns cref="List{Administrator}">Список администраторов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Administrator>> GetList();
}