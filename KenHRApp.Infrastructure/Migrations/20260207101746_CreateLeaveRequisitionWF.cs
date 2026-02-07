using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateLeaveRequisitionWF : Migration
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
                defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 504, DateTimeKind.Local).AddTicks(9150),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 933, DateTimeKind.Local).AddTicks(1384));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 504, DateTimeKind.Local).AddTicks(241),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 929, DateTimeKind.Local).AddTicks(9744));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 506, DateTimeKind.Local).AddTicks(8014),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 941, DateTimeKind.Local).AddTicks(346));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 508, DateTimeKind.Local).AddTicks(8564),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 948, DateTimeKind.Local).AddTicks(9757));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 7, 10, 17, 45, 503, DateTimeKind.Utc).AddTicks(7362),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 30, 21, 25, 47, 928, DateTimeKind.Utc).AddTicks(9368));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(4155),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 950, DateTimeKind.Local).AddTicks(8463));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(1457),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 949, DateTimeKind.Local).AddTicks(9514));

            migrationBuilder.CreateTable(
                name: "LeaveRequisitionWF",
                schema: "kenuser",
                columns: table => new
                {
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveInstanceID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LeaveType = table.Column<string>(type: "varchar(20)", nullable: false),
                    LeaveEmpNo = table.Column<int>(type: "int", nullable: false),
                    LeaveEmpName = table.Column<string>(type: "varchar(150)", nullable: true),
                    LeaveEmpEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    LeaveStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LeaveEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LeaveResumeDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LeaveEmpCostCenter = table.Column<string>(type: "varchar(20)", nullable: true),
                    LeaveRemarks = table.Column<string>(type: "varchar(500)", nullable: true),
                    LeaveConstraints = table.Column<bool>(type: "bit", nullable: true),
                    LeaveStatusCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    LeaveApprovalFlag = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    LeaveVisaRequired = table.Column<bool>(type: "bit", nullable: true),
                    LeavePayAdv = table.Column<bool>(type: "bit", nullable: true),
                    LeaveIsFTMember = table.Column<bool>(type: "bit", nullable: true),
                    LeaveBalance = table.Column<double>(type: "float", nullable: true),
                    LeaveDuration = table.Column<double>(type: "float", nullable: true),
                    NoOfHolidays = table.Column<int>(type: "int", nullable: true),
                    NoOfWeekends = table.Column<int>(type: "int", nullable: true),
                    PlannedLeave = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    LeavePlannedNo = table.Column<int>(type: "int", nullable: true),
                    HalfDayLeaveFlag = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    LeaveCreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(5869)),
                    LeaveCreatedBy = table.Column<int>(type: "int", nullable: true),
                    LeaveCreatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LeaveCreatedEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    LeaveUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LeaveUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LeaveUpdatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LeaveUpdatedEmail = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequisitionWF_LeaveRequestId", x => x.LeaveRequestId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequisitionWF_CompoKeys",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                columns: new[] { "LeaveEmpNo", "LeaveType", "LeaveStartDate", "LeaveEndDate", "LeaveStatusCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveRequisitionWF",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 933, DateTimeKind.Local).AddTicks(1384),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 504, DateTimeKind.Local).AddTicks(9150));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 929, DateTimeKind.Local).AddTicks(9744),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 504, DateTimeKind.Local).AddTicks(241));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 941, DateTimeKind.Local).AddTicks(346),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 506, DateTimeKind.Local).AddTicks(8014));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 948, DateTimeKind.Local).AddTicks(9757),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 508, DateTimeKind.Local).AddTicks(8564));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 30, 21, 25, 47, 928, DateTimeKind.Utc).AddTicks(9368),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 7, 10, 17, 45, 503, DateTimeKind.Utc).AddTicks(7362));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 950, DateTimeKind.Local).AddTicks(8463),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(4155));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 949, DateTimeKind.Local).AddTicks(9514),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(1457));
        }
    }
}
