using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddRelaionBusinessSubscriptionsWithPlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId",
                table: "BusinessSubscriptionModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessSubscriptionModels_PlayerId",
                table: "BusinessSubscriptionModels",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessSubscriptionModels_PlayerModels_PlayerId",
                table: "BusinessSubscriptionModels",
                column: "PlayerId",
                principalTable: "PlayerModels",
                principalColumn: "NID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessSubscriptionModels_PlayerModels_PlayerId",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropIndex(
                name: "IX_BusinessSubscriptionModels_PlayerId",
                table: "BusinessSubscriptionModels");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "BusinessSubscriptionModels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
