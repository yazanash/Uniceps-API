using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRoutineTemplateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "RoutineTemplates");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "RoutineTemplates");

            migrationBuilder.RenameColumn(
                name: "TitleAr",
                table: "RoutineTemplates",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "DescriptionAr",
                table: "RoutineTemplates",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "TargetLanguage",
                table: "RoutineTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetLanguage",
                table: "RoutineTemplates");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "RoutineTemplates",
                newName: "TitleAr");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "RoutineTemplates",
                newName: "DescriptionAr");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "RoutineTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "RoutineTemplates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
