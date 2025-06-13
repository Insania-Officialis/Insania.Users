using Microsoft.Extensions.DependencyInjection;

using Insania.Users.Contracts.BusinessLogic;
using Insania.Users.Models.Responses;
using Insania.Users.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой аутентификации
/// </summary>
[TestFixture]
public class AuthenticationBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с бизнес-логикой аутентификации
    /// </summary>
    private IAuthenticationBL AuthenticationBL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        AuthenticationBL = ServiceProvider.GetRequiredService<IAuthenticationBL>();
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
    /// Тест метода аутентификации
    /// </summary>
    /// <param cref="string?" name="login">Логин</param>
    /// <param cref="string?" name="password">Пароль</param>
    [TestCase(null, null)]
    [TestCase("empty", null)]
    [TestCase("notFound", "1")]
    [TestCase("deleted", "1")]
    [TestCase("blocked", "1")]
    [TestCase("test", "2")]
    [TestCase("test", "1")]
    public async Task AuthenticationTest(string? login, string? password)
    {
        try
        {
            //Получение результата
            AuthenticationInfo? result = await AuthenticationBL.Authentication(login, password);

            //Проверка результата
            switch (login, password)
            {
                case ("test", "1"):
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result?.Success, Is.True);
                    Assert.That(string.IsNullOrWhiteSpace(result?.Token), Is.False);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (login, password)
            {
                case (null, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesUsers.EmptyLogin)); break;
                case ("empty", null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesUsers.EmptyPassword)); break;
                case ("notFound", "1"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesUsers.NotFoundUser)); break;
                case ("deleted", "1"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesUsers.DeletedUser)); break;
                case ("blocked", "1"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesUsers.BlockedUser)); break;
                case ("test", "2"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesUsers.IncorrectPassword)); break;
                default: throw;
            }
        }
    }
    #endregion
}