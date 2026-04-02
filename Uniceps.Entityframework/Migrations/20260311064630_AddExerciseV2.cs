using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroupsV2",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroupsV2", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Heads",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MuscleGroupCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heads", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Heads_MuscleGroupsV2_MuscleGroupCode",
                        column: x => x.MuscleGroupCode,
                        principalTable: "MuscleGroupsV2",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExercisesV2",
                columns: table => new
                {
                    ExerciseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MuscleGroupCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MuscleHeadCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MuscleAux1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MuscleAux2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MuscleAux3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mechanism = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercisesV2", x => x.ExerciseId);
                    table.ForeignKey(
                        name: "FK_ExercisesV2_Equipment_EquipmentCode",
                        column: x => x.EquipmentCode,
                        principalTable: "Equipment",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_ExercisesV2_Heads_MuscleHeadCode",
                        column: x => x.MuscleHeadCode,
                        principalTable: "Heads",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_ExercisesV2_MuscleGroupsV2_MuscleGroupCode",
                        column: x => x.MuscleGroupCode,
                        principalTable: "MuscleGroupsV2",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateIndex(
                name: "Index_Equipment",
                table: "ExercisesV2",
                column: "EquipmentCode");

            migrationBuilder.CreateIndex(
                name: "Index_MuscleGroupV2",
                table: "ExercisesV2",
                column: "MuscleGroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesV2_MuscleHeadCode",
                table: "ExercisesV2",
                column: "MuscleHeadCode");

            migrationBuilder.CreateIndex(
                name: "IX_Heads_MuscleGroupCode",
                table: "Heads",
                column: "MuscleGroupCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExercisesV2");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Heads");

            migrationBuilder.DropTable(
                name: "MuscleGroupsV2");
        }
    }
}
