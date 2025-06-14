using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessProfiles_AspNetUsers_UserId1",
                table: "BusinessProfiles");

            migrationBuilder.DropIndex(
                name: "IX_BusinessProfiles_UserId1",
                table: "BusinessProfiles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BusinessProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BusinessProfiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProfiles_UserId",
                table: "BusinessProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessProfiles_AspNetUsers_UserId",
                table: "BusinessProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessProfiles_AspNetUsers_UserId",
                table: "BusinessProfiles");

            migrationBuilder.DropIndex(
                name: "IX_BusinessProfiles_UserId",
                table: "BusinessProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BusinessProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "BusinessProfiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProfiles_UserId1",
                table: "BusinessProfiles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessProfiles_AspNetUsers_UserId1",
                table: "BusinessProfiles",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
