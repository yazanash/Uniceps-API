using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkoutLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutLogs_WorkoutSessions_WorkoutSessionId",
                table: "WorkoutLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutLogs_WorkoutSessionId",
                table: "WorkoutLogs");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "WorkoutLogs",
                newName: "FinishedReps");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutSessionId",
                table: "WorkoutLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExerciseId",
                table: "WorkoutLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsUrgent",
                table: "Releases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_WorkoutSessionId_ExerciseId_ExerciseIndex_SetIndex",
                table: "WorkoutLogs",
                columns: new[] { "WorkoutSessionId", "ExerciseId", "ExerciseIndex", "SetIndex" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutLogs_WorkoutSessions_WorkoutSessionId",
                table: "WorkoutLogs",
                column: "WorkoutSessionId",
                principalTable: "WorkoutSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutLogs_WorkoutSessions_WorkoutSessionId",
                table: "WorkoutLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutLogs_WorkoutSessionId_ExerciseId_ExerciseIndex_SetIndex",
                table: "WorkoutLogs");

            migrationBuilder.DropColumn(
                name: "IsUrgent",
                table: "Releases");

            migrationBuilder.RenameColumn(
                name: "FinishedReps",
                table: "WorkoutLogs",
                newName: "SessionId");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutSessionId",
                table: "WorkoutLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ExerciseId",
                table: "WorkoutLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_WorkoutSessionId",
                table: "WorkoutLogs",
                column: "WorkoutSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutLogs_WorkoutSessions_WorkoutSessionId",
                table: "WorkoutLogs",
                column: "WorkoutSessionId",
                principalTable: "WorkoutSessions",
                principalColumn: "Id");
        }
    }
}
