using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Models.Responses.Base;

using Insania.Users.Contracts.BusinessLogic;
using Insania.Users.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesUsers = Insania.Users.Messages.ErrorMessages;

namespace Insania.Users.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой пользователей
/// </summary>
[TestFixture]
public class UsersBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с бизнес-логикой пользователей
    /// </summary>
    private IUsersBL UsersBL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        UsersBL = ServiceProvider.GetRequiredService<IUsersBL>();
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
    /// Тест метода проверки наличия логина
    /// </summary>
    /// <param cref="string?" name="login">Логин для проверки</param>
    [TestCase(null)]
    [TestCase("free")]
    [TestCase("test")]
    public async Task CheckLoginTest(string? login)
    {
        try
        {
            //Получение результата
            BaseResponse? result = await UsersBL.CheckLogin(login);

            //Проверка результата
            switch(login)
            {
                case "free": Assert.That(result, Is.Not.Null); Assert.That(result.Success, Is.True); break;
                case "test": Assert.That(result, Is.Not.Null); Assert.That(result.Success, Is.False); break;
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
    #endregion
}