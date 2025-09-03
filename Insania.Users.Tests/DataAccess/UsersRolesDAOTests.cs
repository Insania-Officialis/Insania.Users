using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными ролей пользователей
/// </summary>
[TestFixture]
public class UsersRolesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными ролей пользователей
    /// </summary>
    private IUsersRolesDAO UsersRolesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        UsersRolesDAO = ServiceProvider.GetRequiredService<IUsersRolesDAO>();
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
    /// Тест метода получения списка ролей пользователей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<UserRole>? result = await UsersRolesDAO.GetList();

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
    /// Тест метода получения списка ролей пользователей по идентификатору пользователя
    /// </summary>
    /// <param cref="long?" name="userId">Идентификатор пользователя</param>
    [TestCase(-1)]
    [TestCase(2)]
    [TestCase(4)]
    public async Task GetListByUserIdTest(long? userId)
    {
        try
        {
            //Получение результата
            List<UserRole>? result = await UsersRolesDAO.GetList(userId);

            //Проверка результата

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (userId)
            {
                case -1:
                case 2:
                    Assert.That(result, Is.Empty);
                    break;
                case 4:
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