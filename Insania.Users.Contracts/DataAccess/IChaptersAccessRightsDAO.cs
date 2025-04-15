using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным прав доступа капитулов
/// </summary>
public interface IChaptersAccessRightsDAO
{
    /// <summary>
    /// Метод получения списка прав доступа капитулов
    /// </summary>
    /// <returns cref="List{ChapterAccessRight}">Список прав доступа капитулов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<ChapterAccessRight>> GetList();
}