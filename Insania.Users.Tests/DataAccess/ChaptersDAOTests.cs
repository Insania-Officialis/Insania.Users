using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Tests.Base;

namespace Insania.Users.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными капитулов
/// </summary>
[TestFixture]
public class ChaptersDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными капитулов
    /// </summary>
    private IChaptersDAO ChaptersDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        ChaptersDAO = ServiceProvider.GetRequiredService<IChaptersDAO>();
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
    /// Тест метода получения списка капитулов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<Chapter>? result = await ChaptersDAO.GetList();

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