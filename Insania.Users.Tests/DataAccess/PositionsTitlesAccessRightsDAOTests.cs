using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Tests.Base;

namespace Insania.Users.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными прав доступа званий должностей
/// </summary>
[TestFixture]
public class PositionsTitlesAccessRightsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными прав доступа званий должностей
    /// </summary>
    private IPositionsTitlesAccessRightsDAO PositionsTitlesAccessRightsDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        PositionsTitlesAccessRightsDAO = ServiceProvider.GetRequiredService<IPositionsTitlesAccessRightsDAO>();
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
    /// Тест метода получения списка прав доступа званий должностей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<PositionTitleAccessRight>? result = await PositionsTitlesAccessRightsDAO.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            //Assert.That(result, Is.Not.Empty);
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}