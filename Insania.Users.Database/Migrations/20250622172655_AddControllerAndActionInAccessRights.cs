using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.Users.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddControllerAndActionInAccessRights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "action",
                schema: "insania_users",
                table: "c_access_rights",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Действие");

            migrationBuilder.AddColumn<string>(
                name: "controller",
                schema: "insania_users",
                table: "c_access_rights",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Контроллер");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "action",
                schema: "insania_users",
                table: "c_access_rights");

            migrationBuilder.DropColumn(
                name: "controller",
                schema: "insania_users",
                table: "c_access_rights");
        }
    }
}
