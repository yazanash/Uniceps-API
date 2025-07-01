using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddRelaionMeasurmenetwithPlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BodyMeasurements_PlayerId",
                table: "BodyMeasurements",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BodyMeasurements_PlayerModels_PlayerId",
                table: "BodyMeasurements",
                column: "PlayerId",
                principalTable: "PlayerModels",
                principalColumn: "NID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BodyMeasurements_PlayerModels_PlayerId",
                table: "BodyMeasurements");

            migrationBuilder.DropIndex(
                name: "IX_BodyMeasurements_PlayerId",
                table: "BodyMeasurements");
        }
    }
}
