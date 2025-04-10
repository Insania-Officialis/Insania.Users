using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.DataAccess;

using Insania.Users.Contracts.DataAccess;
using Insania.Users.Entities;
using Insania.Users.Tests.Base;

namespace Insania.Users.Tests.DataAccess;

/// <summary>
/// Тесты сервиса инициализации данных в бд пользователей
/// </summary>
[TestFixture]
public class InitializationDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис инициализации данных в бд пользователей
    /// </summary>
    private IInitializationDAO InitializationDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными ролей
    /// </summary>
    private IRolesDAO RolesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными пользователей
    /// </summary>
    private IUsersDAO UsersDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными прав доступа
    /// </summary>
    private IAccessRightsDAO AccessRightsDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными игроков
    /// </summary>
    private IPlayersDAO PlayersDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными прав доступа ролей
    /// </summary>
    private IRolesAccessRightsDAO RolesAccessRightsDAO { get; set; }

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
        InitializationDAO = ServiceProvider.GetRequiredService<IInitializationDAO>();
        RolesDAO = ServiceProvider.GetRequiredService<IRolesDAO>();
        UsersDAO = ServiceProvider.GetRequiredService<IUsersDAO>();
        AccessRightsDAO = ServiceProvider.GetRequiredService<IAccessRightsDAO>();
        PlayersDAO = ServiceProvider.GetRequiredService<IPlayersDAO>();
        RolesAccessRightsDAO = ServiceProvider.GetRequiredService<IRolesAccessRightsDAO>();
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
    /// Тест метода инициализации данных
    /// </summary>
    [Test]
    public async Task InitializeTest()
    {
        try
        {
            //Выполнение метода
            await InitializationDAO.Initialize();

            //Получение сущностей
            List<Role> roles = await RolesDAO.GetList();
            List<User> users = await UsersDAO.GetList();
            List<AccessRight> accessRights = await AccessRightsDAO.GetList();
            List<Player> players = await PlayersDAO.GetList();
            List<RoleAccessRight> rolesAccessRights = await RolesAccessRightsDAO.GetList();
            List<UserRole> usersRoles = await UsersRolesDAO.GetList();

            //Проверка результата
            Assert.Multiple(() =>
            {
                Assert.That(roles, Is.Not.Empty);
                Assert.That(users, Is.Not.Empty);
                Assert.That(accessRights, Is.Not.Empty);
                Assert.That(players, Is.Not.Empty);
                Assert.That(rolesAccessRights, Is.Not.Empty);
                Assert.That(usersRoles, Is.Not.Empty);
            });
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}