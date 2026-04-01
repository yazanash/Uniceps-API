using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "LicenseActivations",
                type: "datetime2",
                nullable: true);
            migrationBuilder.Sql(@"
        UPDATE Products 
        SET Slug = LOWER(REPLACE(Name, ' ', '-')) + '-' + 
                   CAST(Platform AS NVARCHAR) + '-' + 
                   CAST(AppId AS NVARCHAR)
        WHERE Slug IS NULL OR Slug = ''");
            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Slug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "LicenseActivations");
        }
    }
}
