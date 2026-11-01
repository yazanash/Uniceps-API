using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class RefactorBot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "TelegramUserStates");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "CashPaymentRequests",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "TelegramUserStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TelegramUserStates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "TelegramUserStates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TelegramUserStates");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CashPaymentRequests",
                newName: "SubscriptionId");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "TelegramUserStates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
