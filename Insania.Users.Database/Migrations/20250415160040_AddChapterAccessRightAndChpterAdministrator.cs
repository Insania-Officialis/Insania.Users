using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Users.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddChapterAccessRightAndChpterAdministrator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "u_chapters_access_rights",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chapter_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор капитула"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    access_right_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор права доступа")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_chapters_access_rights", x => x.id);
                    table.UniqueConstraint("AK_u_chapters_access_rights_access_right_id_chapter_id", x => new { x.access_right_id, x.chapter_id });
                    table.ForeignKey(
                        name: "FK_u_chapters_access_rights_c_access_rights_access_right_id",
                        column: x => x.access_right_id,
                        principalSchema: "insania_users",
                        principalTable: "c_access_rights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_chapters_access_rights_r_chapters_chapter_id",
                        column: x => x.chapter_id,
                        principalSchema: "insania_users",
                        principalTable: "r_chapters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Права доступ капитулов");

            migrationBuilder.CreateTable(
                name: "u_chapters_administrators",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chapter_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор капитула"),
                    administrator_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор администратора"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_chapters_administrators", x => x.id);
                    table.UniqueConstraint("AK_u_chapters_administrators_administrator_id_chapter_id", x => new { x.administrator_id, x.chapter_id });
                    table.ForeignKey(
                        name: "FK_u_chapters_administrators_r_administrators_administrator_id",
                        column: x => x.administrator_id,
                        principalSchema: "insania_users",
                        principalTable: "r_administrators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_chapters_administrators_r_chapters_chapter_id",
                        column: x => x.chapter_id,
                        principalSchema: "insania_users",
                        principalTable: "r_chapters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Роли пользователей");

            migrationBuilder.CreateIndex(
                name: "IX_u_chapters_access_rights_chapter_id",
                schema: "insania_users",
                table: "u_chapters_access_rights",
                column: "chapter_id");

            migrationBuilder.CreateIndex(
                name: "IX_u_chapters_administrators_chapter_id",
                schema: "insania_users",
                table: "u_chapters_administrators",
                column: "chapter_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "u_chapters_access_rights",
                schema: "insania_users");

            migrationBuilder.DropTable(
                name: "u_chapters_administrators",
                schema: "insania_users");
        }
    }
}
