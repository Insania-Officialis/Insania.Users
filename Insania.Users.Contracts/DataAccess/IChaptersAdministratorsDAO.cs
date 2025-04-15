using Insania.Users.Entities;

namespace Insania.Users.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным администраторов капитулов
/// </summary>
public interface IChaptersAdministratorsDAO
{
    /// <summary>
    /// Метод получения списка администраторов капитулов
    /// </summary>
    /// <returns cref="List{ChapterAdministrator}">Список администраторов капитулов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<ChapterAdministrator>> GetList();
}