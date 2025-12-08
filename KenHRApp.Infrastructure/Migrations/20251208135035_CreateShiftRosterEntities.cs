using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateShiftRosterEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 3, DateTimeKind.Local).AddTicks(2687),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 12, 7, 16, 15, 58, 849, DateTimeKind.Local).AddTicks(9257));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 2, DateTimeKind.Local).AddTicks(4811),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 12, 7, 16, 15, 58, 849, DateTimeKind.Local).AddTicks(653));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 8, 13, 50, 34, 2, DateTimeKind.Utc).AddTicks(2859),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 12, 7, 13, 15, 58, 848, DateTimeKind.Utc).AddTicks(8441));

            migrationBuilder.CreateTable(
                name: "MasterShiftPatternTitle",
                schema: "kenuser",
                columns: table => new
                {
                    ShiftPatternId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftPatternCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    ShiftPatternDescription = table.Column<string>(type: "varchar(300)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDayShift = table.Column<bool>(type: "bit", nullable: true),
                    IsFlexiTime = table.Column<bool>(type: "bit", nullable: true),
                    CreatedByEmpNo = table.Column<int>(type: "int", nullable: true),
                    CreatedByName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 4, DateTimeKind.Local).AddTicks(7206)),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateEmpNo = table.Column<int>(type: "int", nullable: true),
                    LastUpdateUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterShiftPatternTitle_ShiftPatternId", x => x.ShiftPatternId);
                    table.UniqueConstraint("AK_MasterShiftPatternTitle_ShiftPatternCode", x => x.ShiftPatternCode);
                });

            migrationBuilder.CreateTable(
                name: "MasterShiftPattern",
                schema: "kenuser",
                columns: table => new
                {
                    ShiftPointerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftPointer = table.Column<int>(type: "int", nullable: false),
                    ShiftDescription = table.Column<string>(type: "varchar(200)", nullable: true),
                    ShiftCode = table.Column<string>(type: "varchar(10)", nullable: false),
                    ShiftPatternCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Foreign key that references alternate key: MasterShiftPatternTitle.ShiftPatternCode")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterShiftPattern_ShiftPointerId", x => x.ShiftPointerId);
                    table.ForeignKey(
                        name: "FK_MasterShiftPattern_MasterShiftPatternTitle_ShiftPatternCode",
                        column: x => x.ShiftPatternCode,
                        principalSchema: "kenuser",
                        principalTable: "MasterShiftPatternTitle",
                        principalColumn: "ShiftPatternCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterShiftTime",
                schema: "kenuser",
                columns: table => new
                {
                    ShiftTimingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftCode = table.Column<string>(type: "varchar(10)", nullable: false),
                    ShiftDescription = table.Column<string>(type: "varchar(200)", nullable: false),
                    ArrivalFrom = table.Column<TimeSpan>(type: "time", nullable: true),
                    ArrivalTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    DepartFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    DepartTo = table.Column<TimeSpan>(type: "time", nullable: true),
                    DurationNormal = table.Column<int>(type: "int", nullable: false),
                    RArrivalFrom = table.Column<TimeSpan>(type: "time", nullable: true),
                    RArrivalTo = table.Column<TimeSpan>(type: "time", nullable: true),
                    RDepartFrom = table.Column<TimeSpan>(type: "time", nullable: true),
                    RDepartTo = table.Column<TimeSpan>(type: "time", nullable: true),
                    DurationRamadan = table.Column<int>(type: "int", nullable: true),
                    CreatedByEmpNo = table.Column<int>(type: "int", nullable: true),
                    CreatedByName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateEmpNo = table.Column<int>(type: "int", nullable: true),
                    LastUpdateUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "varchar(50)", nullable: true),
                    ShiftPatternCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Foreign key that references alternate key: MasterShiftPatternTitle.ShiftPatternCode")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterShiftTime_ShiftTimingId", x => x.ShiftTimingId);
                    table.ForeignKey(
                        name: "FK_MasterShiftTime_MasterShiftPatternTitle_ShiftPatternCode",
                        column: x => x.ShiftPatternCode,
                        principalSchema: "kenuser",
                        principalTable: "MasterShiftPatternTitle",
                        principalColumn: "ShiftPatternCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterShiftPattern_CompoKeys",
                schema: "kenuser",
                table: "MasterShiftPattern",
                columns: new[] { "ShiftPatternCode", "ShiftCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterShiftPatternTitle_UniqueKey",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                column: "ShiftPatternCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterShiftTime_CompoKeys",
                schema: "kenuser",
                table: "MasterShiftTime",
                columns: new[] { "ShiftPatternCode", "ShiftCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterShiftPattern",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "MasterShiftTime",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "MasterShiftPatternTitle",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 12, 7, 16, 15, 58, 849, DateTimeKind.Local).AddTicks(9257),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 3, DateTimeKind.Local).AddTicks(2687));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 12, 7, 16, 15, 58, 849, DateTimeKind.Local).AddTicks(653),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 2, DateTimeKind.Local).AddTicks(4811));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 7, 13, 15, 58, 848, DateTimeKind.Utc).AddTicks(8441),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 12, 8, 13, 50, 34, 2, DateTimeKind.Utc).AddTicks(2859));
        }
    }
}
