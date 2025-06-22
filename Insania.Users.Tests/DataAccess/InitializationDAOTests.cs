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

    /// <summary>
    /// Сервис работы с данными должностей
    /// </summary>
    private IPositionsDAO PositionsDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными званий
    /// </summary>
    private ITitlesDAO TitlesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными званий должностей
    /// </summary>
    private IPositionsTitlesDAO PositionsTitlesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными администраторов
    /// </summary>
    private IAdministratorsDAO AdministratorsDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными капитулов
    /// </summary>
    private IChaptersDAO ChaptersDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными прав доступа званий должностей
    /// </summary>
    private IPositionsTitlesAccessRightsDAO PositionsTitlesAccessRightsDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными прав доступа капитулов
    /// </summary>
    private IChaptersAccessRightsDAO ChaptersAccessRightsDAO { get; set; }

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
        InitializationDAO = ServiceProvider.GetRequiredService<IInitializationDAO>();
        RolesDAO = ServiceProvider.GetRequiredService<IRolesDAO>();
        UsersDAO = ServiceProvider.GetRequiredService<IUsersDAO>();
        AccessRightsDAO = ServiceProvider.GetRequiredService<IAccessRightsDAO>();
        PlayersDAO = ServiceProvider.GetRequiredService<IPlayersDAO>();
        RolesAccessRightsDAO = ServiceProvider.GetRequiredService<IRolesAccessRightsDAO>();
        UsersRolesDAO = ServiceProvider.GetRequiredService<IUsersRolesDAO>();
        PositionsDAO = ServiceProvider.GetRequiredService<IPositionsDAO>();
        TitlesDAO = ServiceProvider.GetRequiredService<ITitlesDAO>();
        PositionsTitlesDAO = ServiceProvider.GetRequiredService<IPositionsTitlesDAO>();
        AdministratorsDAO = ServiceProvider.GetRequiredService<IAdministratorsDAO>();
        ChaptersDAO = ServiceProvider.GetRequiredService<IChaptersDAO>();
        PositionsTitlesAccessRightsDAO = ServiceProvider.GetRequiredService<IPositionsTitlesAccessRightsDAO>();
        ChaptersAccessRightsDAO = ServiceProvider.GetRequiredService<IChaptersAccessRightsDAO>();
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
            List<Position> positions = await PositionsDAO.GetList();
            List<Title> titles = await TitlesDAO.GetList();
            List<PositionTitle> positionsTitles = await PositionsTitlesDAO.GetList();
            List<Administrator> administrators = await AdministratorsDAO.GetList();
            List<Chapter> chapters = await ChaptersDAO.GetList();
            List<PositionTitleAccessRight> positionsTitlesAccessRights = await PositionsTitlesAccessRightsDAO.GetList();
            List<ChapterAccessRight> chaptersAccessRights = await ChaptersAccessRightsDAO.GetList();
            List<ChapterAdministrator> chaptersAdministrators = await ChaptersAdministratorsDAO.GetList();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(roles, Is.Not.Empty);
                Assert.That(users, Is.Not.Empty);
                Assert.That(accessRights, Is.Not.Empty);
                Assert.That(players, Is.Not.Empty);
                Assert.That(rolesAccessRights, Is.Not.Empty);
                Assert.That(usersRoles, Is.Not.Empty);
                Assert.That(positions, Is.Not.Empty);
                Assert.That(titles, Is.Not.Empty);
                Assert.That(positionsTitles, Is.Not.Empty);
                Assert.That(administrators, Is.Not.Empty);
                Assert.That(chapters, Is.Not.Empty);
                //Assert.That(positionsTitlesAccessRights, Is.Not.Empty);
                //Assert.That(chaptersAccessRights, Is.Not.Empty);
                Assert.That(chaptersAdministrators, Is.Not.Empty);
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