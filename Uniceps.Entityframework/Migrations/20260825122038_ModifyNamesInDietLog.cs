using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class ModifyNamesInDietLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Protien",
                table: "DietLogs",
                newName: "Protein");

            migrationBuilder.RenameColumn(
                name: "IngreientName",
                table: "DietLogs",
                newName: "IngredientName");

            migrationBuilder.RenameColumn(
                name: "Clories",
                table: "DietLogs",
                newName: "Calories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Protein",
                table: "DietLogs",
                newName: "Protien");

            migrationBuilder.RenameColumn(
                name: "IngredientName",
                table: "DietLogs",
                newName: "IngreientName");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "DietLogs",
                newName: "Clories");
        }
    }
}
