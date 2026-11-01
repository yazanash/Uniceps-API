using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDataToDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TelegramUserStates");

            migrationBuilder.DropColumn(
                name: "ReceiptFileId",
                table: "TelegramUserStates");

            migrationBuilder.DropColumn(
                name: "TransferImageUrl",
                table: "TelegramUserStates");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "CashPaymentRequests");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CashPaymentRequests");

            migrationBuilder.DropColumn(
                name: "ReceiptFileId",
                table: "CashPaymentRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TelegramUserStates",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptFileId",
                table: "TelegramUserStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferImageUrl",
                table: "TelegramUserStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "CashPaymentRequests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CashPaymentRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptFileId",
                table: "CashPaymentRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
