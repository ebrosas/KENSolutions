using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateLeaveEntitlement : Migration
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
                defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 922, DateTimeKind.Local).AddTicks(3554),
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
                defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 921, DateTimeKind.Local).AddTicks(3046),
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
                defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 924, DateTimeKind.Local).AddTicks(4718),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 506, DateTimeKind.Local).AddTicks(8014));

            migrationBuilder.AlterColumn<string>(
                name: "PlannedLeave",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "char(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 931, DateTimeKind.Local).AddTicks(1356),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(5869));

            migrationBuilder.AlterColumn<string>(
                name: "LeaveApprovalFlag",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "char(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HalfDayLeaveFlag",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "char(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 930, DateTimeKind.Local).AddTicks(2093),
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
                defaultValue: new DateTime(2026, 2, 7, 11, 45, 8, 921, DateTimeKind.Utc).AddTicks(696),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 7, 10, 17, 45, 503, DateTimeKind.Utc).AddTicks(7362));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 930, DateTimeKind.Local).AddTicks(8738),
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
                defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 930, DateTimeKind.Local).AddTicks(5063),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(1457));

            migrationBuilder.CreateTable(
                name: "LeaveEntitlement",
                schema: "kenuser",
                columns: table => new
                {
                    LeaveEntitlementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveEntitlemnt = table.Column<double>(type: "float", nullable: false, comment: "Part of composite unique key index"),
                    SickLeaveEntitlemnt = table.Column<double>(type: "float", nullable: false),
                    LeaveUOM = table.Column<string>(type: "char(1)", nullable: false, comment: "Part of composite unique key index"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 931, DateTimeKind.Local).AddTicks(4268)),
                    LeaveCreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("LeaveEntitlementId", x => x.LeaveEntitlementId);
                    table.ForeignKey(
                        name: "FK_LeaveEntitlement_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveEntitlement_CompoKeys",
                schema: "kenuser",
                table: "LeaveEntitlement",
                columns: new[] { "EmployeeNo", "LeaveEntitlemnt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveEntitlement_EmployeeNo",
                schema: "kenuser",
                table: "LeaveEntitlement",
                column: "EmployeeNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveEntitlement",
                schema: "kenuser");

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
                oldDefaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 922, DateTimeKind.Local).AddTicks(3554));

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
                oldDefaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 921, DateTimeKind.Local).AddTicks(3046));

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
                oldDefaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 924, DateTimeKind.Local).AddTicks(4718));

            migrationBuilder.AlterColumn<string>(
                name: "PlannedLeave",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 7, 13, 17, 45, 509, DateTimeKind.Local).AddTicks(5869),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 931, DateTimeKind.Local).AddTicks(1356));

            migrationBuilder.AlterColumn<string>(
                name: "LeaveApprovalFlag",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HalfDayLeaveFlag",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldNullable: true);

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
                oldDefaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 930, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 7, 10, 17, 45, 503, DateTimeKind.Utc).AddTicks(7362),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 7, 11, 45, 8, 921, DateTimeKind.Utc).AddTicks(696));

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
                oldDefaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 930, DateTimeKind.Local).AddTicks(8738));

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
                oldDefaultValue: new DateTime(2026, 2, 7, 14, 45, 8, 930, DateTimeKind.Local).AddTicks(5063));
        }
    }
}
