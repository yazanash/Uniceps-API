using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddRelaionBusinessSubscriptionBusinessServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BusinessSubscriptionModels_ServiceNID",
                table: "BusinessSubscriptionModels",
                column: "ServiceNID");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessSubscriptionModels_BusinessServiceModels_ServiceNID",
                table: "BusinessSubscriptionModels",
                column: "ServiceNID",
                principalTable: "BusinessServiceModels",
                principalColumn: "NID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessSubscriptionModels_BusinessServiceModels_ServiceNID",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropIndex(
                name: "IX_BusinessSubscriptionModels_ServiceNID",
                table: "BusinessSubscriptionModels");
        }
    }
}
