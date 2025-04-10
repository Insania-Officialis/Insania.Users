using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Users.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "insania_users");

            migrationBuilder.CreateTable(
                name: "c_access_rights",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_access_rights", x => x.id);
                    table.UniqueConstraint("AK_c_access_rights_alias", x => x.alias);
                },
                comment: "Права доступа");

            migrationBuilder.CreateTable(
                name: "c_roles",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_roles", x => x.id);
                    table.UniqueConstraint("AK_c_roles_alias", x => x.alias);
                },
                comment: "Роли");

            migrationBuilder.CreateTable(
                name: "r_users",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false, comment: "Логин"),
                    password = table.Column<string>(type: "text", nullable: false, comment: "Пароль"),
                    phone = table.Column<string>(type: "text", nullable: true, comment: "Номер телефона"),
                    email = table.Column<string>(type: "text", nullable: true, comment: "Электронная почта"),
                    link_vk = table.Column<string>(type: "text", nullable: true, comment: "Ссылка в ВК"),
                    last_name = table.Column<string>(type: "text", nullable: true, comment: "Фамилия"),
                    first_name = table.Column<string>(type: "text", nullable: true, comment: "Имя"),
                    patronymic = table.Column<string>(type: "text", nullable: true, comment: "Отчество"),
                    gender = table.Column<bool>(type: "boolean", nullable: true, comment: "Пол(истина - мужской/ложь - женский/null - неизвестно)"),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true, comment: "Дата рождения"),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак блокировки пользователя"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_users", x => x.id);
                    table.UniqueConstraint("AK_r_users_login", x => x.login);
                },
                comment: "Пользователи");

            migrationBuilder.CreateTable(
                name: "u_roles_access_rights",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор роли"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    access_right_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор права доступа")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_roles_access_rights", x => x.id);
                    table.UniqueConstraint("AK_u_roles_access_rights_access_right_id_role_id", x => new { x.access_right_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_u_roles_access_rights_c_access_rights_access_right_id",
                        column: x => x.access_right_id,
                        principalSchema: "insania_users",
                        principalTable: "c_access_rights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_roles_access_rights_c_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "insania_users",
                        principalTable: "c_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Права доступ ролей");

            migrationBuilder.CreateTable(
                name: "r_players",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор пользователя"),
                    loyalty_points = table.Column<int>(type: "integer", nullable: false, comment: "Баллы верности"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_players", x => x.id);
                    table.ForeignKey(
                        name: "FK_r_players_r_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "insania_users",
                        principalTable: "r_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Игроки");

            migrationBuilder.CreateTable(
                name: "u_users_roles",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор пользователя"),
                    role_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор роли"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_users_roles", x => x.id);
                    table.UniqueConstraint("AK_u_users_roles_role_id_user_id", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_u_users_roles_c_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "insania_users",
                        principalTable: "c_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_users_roles_r_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "insania_users",
                        principalTable: "r_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Роли пользователей");

            migrationBuilder.CreateIndex(
                name: "IX_r_players_user_id",
                schema: "insania_users",
                table: "r_players",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_u_roles_access_rights_role_id",
                schema: "insania_users",
                table: "u_roles_access_rights",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_u_users_roles_user_id",
                schema: "insania_users",
                table: "u_users_roles",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "r_players",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "u_roles_access_rights",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "u_users_roles",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "c_access_rights",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "c_roles",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "r_users",
                schema: "insania_users");
        }
    }
}
