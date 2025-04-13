using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Users.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "u_positions_titles_access_rights",
                schema: "insania_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    position_title_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор звания должности"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    access_right_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор права доступа")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_positions_titles_access_rights", x => x.id);
                    table.UniqueConstraint("AK_u_positions_titles_access_rights_access_right_id_position_t~", x => new { x.access_right_id, x.position_title_id });
                    table.ForeignKey(
                        name: "FK_u_positions_titles_access_rights_c_access_rights_access_rig~",
                        column: x => x.access_right_id,
                        principalSchema: "insania_users",
                        principalTable: "c_access_rights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_positions_titles_access_rights_u_positions_titles_positio~",
                        column: x => x.position_title_id,
                        principalSchema: "insania_users",
                        principalTable: "u_positions_titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Права доступ званий должностей");

            migrationBuilder.CreateIndex(
                name: "IX_u_positions_titles_access_rights_position_title_id",
                schema: "insania_users",
                table: "u_positions_titles_access_rights",
                column: "position_title_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "u_positions_titles_access_rights",
                schema: "insania_users");
        }
    }
}
