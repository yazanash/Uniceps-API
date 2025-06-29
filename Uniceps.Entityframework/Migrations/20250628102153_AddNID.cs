using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddNID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
               name: "NID",
               table: "UserDevices",
               type: "uniqueidentifier",
               nullable: false,
               defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "SystemSubscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanNID",
                table: "SystemSubscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "Sets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "RoutineItemNID",
                table: "Sets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "Routines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "RoutineItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "DayNID",
                table: "RoutineItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "PlayerModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "OTPModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "NormalProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "Days",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "RoutineNID",
                table: "Days",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "BusinessSubscriptionModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<string>(
                name: "BusinessId",
                table: "BusinessSubscriptionModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceNID",
                table: "BusinessSubscriptionModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "BusinessServiceModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<Guid>(
                name: "NID",
                table: "BusinessProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");
            migrationBuilder.Sql(@"
            UPDATE Days
            SET RoutineNID = Routines.NID
            FROM Days
            INNER JOIN Routines ON
            Days.RoutineId = Routines.Id
        ");
            migrationBuilder.Sql(@"
            UPDATE RoutineItems
            SET DayNID = Days.NID
            FROM RoutineItems
            INNER JOIN Days ON
            RoutineItems.DayId = Days.Id
        ");
            migrationBuilder.Sql(@"
            UPDATE Sets
            SET RoutineItemNID = RoutineItems.NID
            FROM Sets
            INNER JOIN RoutineItems ON
            Sets.RoutineItemId = RoutineItems.Id
        ");
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Routines_RoutineId",
                table: "Days");

            migrationBuilder.DropForeignKey(
                name: "FK_RoutineItems_Days_DayId",
                table: "RoutineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_RoutineItems_RoutineItemId",
                table: "Sets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDevices",
                table: "UserDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemSubscriptions",
                table: "SystemSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sets",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_RoutineItemId",
                table: "Sets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routines",
                table: "Routines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoutineItems",
                table: "RoutineItems");

            migrationBuilder.DropIndex(
                name: "IX_RoutineItems_DayId",
                table: "RoutineItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerModels",
                table: "PlayerModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plans",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OTPModels",
                table: "OTPModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NormalProfiles",
                table: "NormalProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Days",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_RoutineId",
                table: "Days");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessSubscriptionModels",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessServiceModels",
                table: "BusinessServiceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessProfiles",
                table: "BusinessProfiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "RoutineItemId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoutineItems");

            migrationBuilder.DropColumn(
                name: "DayId",
                table: "RoutineItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlayerModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OTPModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "NormalProfiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "RoutineId",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BusinessServiceModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BusinessProfiles");

           
            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDevices",
                table: "UserDevices",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemSubscriptions",
                table: "SystemSubscriptions",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sets",
                table: "Sets",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routines",
                table: "Routines",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoutineItems",
                table: "RoutineItems",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerModels",
                table: "PlayerModels",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plans",
                table: "Plans",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTPModels",
                table: "OTPModels",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NormalProfiles",
                table: "NormalProfiles",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Days",
                table: "Days",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessSubscriptionModels",
                table: "BusinessSubscriptionModels",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessServiceModels",
                table: "BusinessServiceModels",
                column: "NID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessProfiles",
                table: "BusinessProfiles",
                column: "NID");

            migrationBuilder.CreateTable(
                name: "BodyMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeightCm = table.Column<double>(type: "float", nullable: false),
                    WeightKg = table.Column<double>(type: "float", nullable: false),
                    WaistCm = table.Column<double>(type: "float", nullable: false),
                    ChestCm = table.Column<double>(type: "float", nullable: false),
                    HipsCm = table.Column<double>(type: "float", nullable: false),
                    NeckCm = table.Column<double>(type: "float", nullable: false),
                    ShouldersCm = table.Column<double>(type: "float", nullable: false),
                    LeftArmCm = table.Column<double>(type: "float", nullable: false),
                    RightArmCm = table.Column<double>(type: "float", nullable: false),
                    LeftThighCm = table.Column<double>(type: "float", nullable: false),
                    RightThighCm = table.Column<double>(type: "float", nullable: false),
                    LeftLegCm = table.Column<double>(type: "float", nullable: false),
                    RightLegCm = table.Column<double>(type: "float", nullable: false),
                    MeasuredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    WeightKg = table.Column<double>(type: "float", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sets_RoutineItemNID",
                table: "Sets",
                column: "RoutineItemNID");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_DayNID",
                table: "RoutineItems",
                column: "DayNID");

            migrationBuilder.CreateIndex(
                name: "IX_Days_RoutineNID",
                table: "Days",
                column: "RoutineNID");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Routines_RoutineNID",
                table: "Days",
                column: "RoutineNID",
                principalTable: "Routines",
                principalColumn: "NID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineItems_Days_DayNID",
                table: "RoutineItems",
                column: "DayNID",
                principalTable: "Days",
                principalColumn: "NID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_RoutineItems_RoutineItemNID",
                table: "Sets",
                column: "RoutineItemNID",
                principalTable: "RoutineItems",
                principalColumn: "NID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SystemSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "SystemSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Sets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RoutineItemId",
                table: "Sets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Routines",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoutineItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "RoutineItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlayerModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Plans",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OTPModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "NormalProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Days",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RoutineId",
                table: "Days",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BusinessSubscriptionModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "BusinessSubscriptionModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BusinessServiceModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BusinessProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
            migrationBuilder.Sql(@"
    UPDATE Days
    SET RoutineId = Routines.Id
    FROM Days
    INNER JOIN Routines ON
    Days.RoutineNID = Routines.NID
");
            migrationBuilder.Sql(@"
    UPDATE RoutineItems
    SET DayId = Days.Id
    FROM RoutineItems
    INNER JOIN Days ON
    RoutineItems.DayNID = Days.NID
");
            migrationBuilder.Sql(@"
    UPDATE Sets
    SET RoutineItemId = RoutineItems.Id
    FROM Sets
    INNER JOIN RoutineItems ON
    Sets.RoutineItemNID = RoutineItems.NID
");
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Routines_RoutineNID",
                table: "Days");

            migrationBuilder.DropForeignKey(
                name: "FK_RoutineItems_Days_DayNID",
                table: "RoutineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_RoutineItems_RoutineItemNID",
                table: "Sets");

            migrationBuilder.DropTable(
                name: "BodyMeasurements");

            migrationBuilder.DropTable(
                name: "WorkoutLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDevices",
                table: "UserDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemSubscriptions",
                table: "SystemSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sets",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_RoutineItemNID",
                table: "Sets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routines",
                table: "Routines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoutineItems",
                table: "RoutineItems");

            migrationBuilder.DropIndex(
                name: "IX_RoutineItems_DayNID",
                table: "RoutineItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerModels",
                table: "PlayerModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plans",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OTPModels",
                table: "OTPModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NormalProfiles",
                table: "NormalProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Days",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_RoutineNID",
                table: "Days");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessSubscriptionModels",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessServiceModels",
                table: "BusinessServiceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessProfiles",
                table: "BusinessProfiles");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "PlanNID",
                table: "SystemSubscriptions");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "RoutineItemNID",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "RoutineItems");

            migrationBuilder.DropColumn(
                name: "DayNID",
                table: "RoutineItems");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "PlayerModels");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "OTPModels");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "NormalProfiles");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "RoutineNID",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropColumn(
                name: "ServiceNID",
                table: "BusinessSubscriptionModels");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "BusinessServiceModels");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "BusinessProfiles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserDevices",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");


            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDevices",
                table: "UserDevices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemSubscriptions",
                table: "SystemSubscriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sets",
                table: "Sets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routines",
                table: "Routines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoutineItems",
                table: "RoutineItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerModels",
                table: "PlayerModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plans",
                table: "Plans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTPModels",
                table: "OTPModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NormalProfiles",
                table: "NormalProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Days",
                table: "Days",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessSubscriptionModels",
                table: "BusinessSubscriptionModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessServiceModels",
                table: "BusinessServiceModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessProfiles",
                table: "BusinessProfiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_RoutineItemId",
                table: "Sets",
                column: "RoutineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_DayId",
                table: "RoutineItems",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_Days_RoutineId",
                table: "Days",
                column: "RoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Routines_RoutineId",
                table: "Days",
                column: "RoutineId",
                principalTable: "Routines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineItems_Days_DayId",
                table: "RoutineItems",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_RoutineItems_RoutineItemId",
                table: "Sets",
                column: "RoutineItemId",
                principalTable: "RoutineItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
