using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными пользователей
/// </summary>
[TestFixture]
public class UsersDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными пользователей
    /// </summary>
    private IUsersDAO UsersDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        UsersDAO = ServiceProvider.GetRequiredService<IUsersDAO>();
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
    /// Тест метода получения пользователя по логину
    /// </summary>
    /// <param cref="string?" name="login">Логин</param>
    [TestCase(null)]
    [TestCase("free")]
    [TestCase("test")]
    public async Task GetByLoginTest(string? login)
    {
        try
        {
            //Получение результата
            User? result = await UsersDAO.GetByLogin(login);

            //Проверка результата
            switch (login)
            {
                case "free": Assert.That(result, Is.Null); break;
                case "test": Assert.That(result, Is.Not.Null); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (login)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesUsers.EmptyLogin)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода получения списка пользователей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<User>? result = await UsersDAO.GetList();

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