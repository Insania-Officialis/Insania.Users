using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными прав доступа ролей
/// </summary>
[TestFixture]
public class RolesAccessRightsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными прав доступа ролей
    /// </summary>
    private IRolesAccessRightsDAO RolesAccessRightsDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        RolesAccessRightsDAO = ServiceProvider.GetRequiredService<IRolesAccessRightsDAO>();
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
    /// Тест метода получения списка прав доступа ролей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<RoleAccessRight>? result = await RolesAccessRightsDAO.GetList();

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

    /// <summary>
    /// Тест метода получения списка прав доступа ролей по идентификатору роли
    /// </summary>
    /// <param cref="long?" name="roleId">Идентификатор роли</param>
    [TestCase(-1)]
    [TestCase(4)]
    [TestCase(1)]
    public async Task GetListByUserIdTest(long roleId)
    {
        try
        {
            //Получение результата
            List<RoleAccessRight>? result = await RolesAccessRightsDAO.GetList(roleId);

            //Проверка результата

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (roleId)
            {
                case -1:
                case 4:
                    Assert.That(result, Is.Empty);
                    break;
                case 1:
                    Assert.That(result, Is.Not.Empty);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}