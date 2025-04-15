using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Tests.Base;

namespace Insania.Users.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными администраторов капитулов
/// </summary>
[TestFixture]
public class ChaptersAdministratorsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными администраторов капитулов
    /// </summary>
    private IChaptersAdministratorsDAO ChaptersAdministratorsDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        ChaptersAdministratorsDAO = ServiceProvider.GetRequiredService<IChaptersAdministratorsDAO>();
    }

    /// <summary>
    /// Метод, вызываемый после тестов
    /// </summary>
    [TearDown]
    public void TearDown()
    {

    }
    #endregion

    #region Методы тестирования
    /// <summary>
    /// Тест метода получения списка администраторов капитулов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<ChapterAdministrator>? result = await ChaptersAdministratorsDAO.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}