using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddIntIdToBodyMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BodyMeasurements",
                table: "BodyMeasurements");

            migrationBuilder.AddColumn<int>(
                name: "MId",
                table: "BodyMeasurements",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BodyMeasurements",
                table: "BodyMeasurements",
                column: "MId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BodyMeasurements",
                table: "BodyMeasurements");

            migrationBuilder.DropColumn(
                name: "MId",
                table: "BodyMeasurements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BodyMeasurements",
                table: "BodyMeasurements",
                column: "Id");
        }
    }
}
