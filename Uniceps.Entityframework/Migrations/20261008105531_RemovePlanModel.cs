using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlanModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
              name: "Name",
              table: "PlanItems",
              type: "nvarchar(max)",
              nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "PlanItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
        UPDATE pi
        SET pi.Name = p.Name,
            pi.ProductId = p.ProductId
        FROM PlanItems pi
        INNER JOIN Plans p ON pi.PlanNID = p.NID
    ");
            migrationBuilder.DropForeignKey(
                name: "FK_PlanItems_Plans_PlanNID",
                table: "PlanItems");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_PlanItems_PlanNID",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "PlanNID",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "PlanNID",
                table: "PlanItems");

            migrationBuilder.AddColumn<int>(
                name: "PlanItemId",
                table: "CashPaymentRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PlanItems");

            migrationBuilder.DropColumn(
                name: "PlanItemId",
                table: "CashPaymentRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanNID",
                table: "SystemSubscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlanNID",
                table: "PlanItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    NID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TargetUserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.NID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanItems_PlanNID",
                table: "PlanItems",
                column: "PlanNID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanItems_Plans_PlanNID",
                table: "PlanItems",
                column: "PlanNID",
                principalTable: "Plans",
                principalColumn: "NID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
