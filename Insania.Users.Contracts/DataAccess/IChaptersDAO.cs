using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным капитулов
/// </summary>
public interface IChaptersDAO
{
    /// <summary>
    /// Метод получения списка капитулов
    /// </summary>
    /// <returns cref="List{Chapter}">Список капитулов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Chapter>> GetList();
}