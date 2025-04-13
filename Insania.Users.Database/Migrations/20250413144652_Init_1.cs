using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Users.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "c_positions",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    activity_area = table.Column<string>(type: "text", nullable: true, comment: "Сфера деятельности"),
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
                    table.PrimaryKey("PK_c_positions", x => x.id);
                    table.UniqueConstraint("AK_c_positions_alias", x => x.alias);
                },
                comment: "Должности");

            migrationBuilder.CreateTable(
                name: "c_titles",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coefficient_accrual_honor_points = table.Column<double>(type: "double precision", nullable: false, comment: "Коэффициент начисления баллов почёта"),
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
                    table.PrimaryKey("PK_c_titles", x => x.id);
                    table.UniqueConstraint("AK_c_titles_alias", x => x.alias);
                },
                comment: "Звания");

            migrationBuilder.CreateTable(
                name: "r_chapters",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    parent_id = table.Column<long>(type: "bigint", nullable: true, comment: "Идентификатор родителя"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_chapters", x => x.id);
                    table.UniqueConstraint("AK_r_chapters_name", x => x.name);
                    table.ForeignKey(
                        name: "FK_r_chapters_r_chapters_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "insania_users",
                        principalTable: "r_chapters",
                        principalColumn: "id");
                },
                comment: "Капитулы");

            migrationBuilder.CreateTable(
                name: "u_positions_titles",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    position_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор должности"),
                    title_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор звания"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_positions_titles", x => x.id);
                    table.UniqueConstraint("AK_u_positions_titles_position_id_title_id", x => new { x.position_id, x.title_id });
                    table.ForeignKey(
                        name: "FK_u_positions_titles_c_positions_position_id",
                        column: x => x.position_id,
                        principalSchema: "insania_users",
                        principalTable: "c_positions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_positions_titles_c_titles_title_id",
                        column: x => x.title_id,
                        principalSchema: "insania_users",
                        principalTable: "c_titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Звания должностей");

            migrationBuilder.CreateTable(
                name: "r_administrators",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор пользователя"),
                    honor_points = table.Column<int>(type: "integer", nullable: false, comment: "Баллы почёта"),
                    position_title_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор звания должности"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_administrators", x => x.id);
                    table.ForeignKey(
                        name: "FK_r_administrators_r_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "insania_users",
                        principalTable: "r_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_r_administrators_u_positions_titles_position_title_id",
                        column: x => x.position_title_id,
                        principalSchema: "insania_users",
                        principalTable: "u_positions_titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Администраторы");

            migrationBuilder.CreateIndex(
                name: "IX_r_administrators_position_title_id",
                schema: "insania_users",
                table: "r_administrators",
                column: "position_title_id");

            migrationBuilder.CreateIndex(
                name: "IX_r_administrators_user_id",
                schema: "insania_users",
                table: "r_administrators",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_r_chapters_parent_id",
                schema: "insania_users",
                table: "r_chapters",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_u_positions_titles_title_id",
                schema: "insania_users",
                table: "u_positions_titles",
                column: "title_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "r_administrators",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "r_chapters",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "u_positions_titles",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "c_positions",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "c_titles",
                schema: "insania_users");
        }
    }
}
