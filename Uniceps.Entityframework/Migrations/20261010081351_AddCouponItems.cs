using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddCouponItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Coupon",
                table: "TelegramUserStates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                table: "TelegramUserStates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CouponValue",
                table: "SystemSubscriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                table: "CashPaymentRequests",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "TelegramUserStates");

            migrationBuilder.DropColumn(
                name: "CouponValue",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "CashPaymentRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Coupon",
                table: "TelegramUserStates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
