using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAttendanceSwipeLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SwipeCode",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                newName: "SwipeType");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 385, DateTimeKind.Local).AddTicks(5180),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 269, DateTimeKind.Local).AddTicks(5472));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 378, DateTimeKind.Local).AddTicks(602),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 268, DateTimeKind.Local).AddTicks(193));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 393, DateTimeKind.Local).AddTicks(8558),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 272, DateTimeKind.Local).AddTicks(7822));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 400, DateTimeKind.Local).AddTicks(6934),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 276, DateTimeKind.Local).AddTicks(3697));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 30, 20, 52, 42, 377, DateTimeKind.Utc).AddTicks(2311),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 29, 13, 23, 31, 267, DateTimeKind.Utc).AddTicks(5283));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 401, DateTimeKind.Local).AddTicks(5868),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 276, DateTimeKind.Local).AddTicks(6108));

            migrationBuilder.CreateTable(
                name: "AttendanceTimesheet",
                schema: "kenuser",
                columns: table => new
                {
                    TimesheetId = table.Column<double>(type: "float", nullable: false),
                    EmpNo = table.Column<int>(type: "int", nullable: false),
                    CostCenter = table.Column<string>(type: "varchar(20)", nullable: false),
                    PayGrade = table.Column<string>(type: "varchar(20)", nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeOut = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShavedIn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShavedOut = table.Column<DateTime>(type: "datetime", nullable: true),
                    OTType = table.Column<string>(type: "varchar(20)", nullable: true),
                    OTStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    OTEndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShiftPatCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    SchedShiftCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    ActualShiftCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    NoPayHours = table.Column<int>(type: "int", nullable: true),
                    RemarkCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    AbsenceReasonCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    AbsenceReasonColumn = table.Column<string>(type: "varchar(10)", nullable: true),
                    LeaveType = table.Column<string>(type: "varchar(10)", nullable: true),
                    DIL_Entitlement = table.Column<string>(type: "varchar(10)", nullable: true),
                    CorrectionCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    Processed = table.Column<bool>(type: "bit", nullable: true),
                    ProcessID = table.Column<int>(type: "int", nullable: true),
                    UploadID = table.Column<int>(type: "int", nullable: true),
                    IsPublicHoliday = table.Column<bool>(type: "bit", nullable: true),
                    IsDILHoliday = table.Column<bool>(type: "bit", nullable: true),
                    IsRamadan = table.Column<bool>(type: "bit", nullable: true),
                    IsMuslim = table.Column<bool>(type: "bit", nullable: true),
                    IsDriver = table.Column<bool>(type: "bit", nullable: true),
                    IsDILDayWorker = table.Column<bool>(type: "bit", nullable: true),
                    IsSalaryStaff = table.Column<bool>(type: "bit", nullable: true),
                    IsDayWorkerOrShifter = table.Column<bool>(type: "bit", nullable: true),
                    IsLiasonOfficer = table.Column<bool>(type: "bit", nullable: true),
                    IsLastRow = table.Column<bool>(type: "bit", nullable: true),
                    DurationRequired = table.Column<int>(type: "int", nullable: true),
                    DurationWorked = table.Column<int>(type: "int", nullable: true),
                    DurationWorkedCumulative = table.Column<int>(type: "int", nullable: true),
                    NetMinutes = table.Column<int>(type: "int", nullable: true),
                    CreatedByEmpNo = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 402, DateTimeKind.Local).AddTicks(4645)),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateEmpNo = table.Column<int>(type: "int", nullable: true),
                    LastUpdateUserID = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTimesheet_TimesheetId", x => x.TimesheetId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceTimesheet",
                schema: "kenuser");

            migrationBuilder.RenameColumn(
                name: "SwipeType",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                newName: "SwipeCode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 269, DateTimeKind.Local).AddTicks(5472),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 385, DateTimeKind.Local).AddTicks(5180));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 268, DateTimeKind.Local).AddTicks(193),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 378, DateTimeKind.Local).AddTicks(602));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 272, DateTimeKind.Local).AddTicks(7822),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 393, DateTimeKind.Local).AddTicks(8558));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 276, DateTimeKind.Local).AddTicks(3697),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 400, DateTimeKind.Local).AddTicks(6934));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 29, 13, 23, 31, 267, DateTimeKind.Utc).AddTicks(5283),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 30, 20, 52, 42, 377, DateTimeKind.Utc).AddTicks(2311));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 276, DateTimeKind.Local).AddTicks(6108),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 401, DateTimeKind.Local).AddTicks(5868));
        }
    }
}
