using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalMonthToPayrollPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PayrollPeriod_CompoKeys",
                schema: "kenuser",
                table: "PayrollPeriod");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 483, DateTimeKind.Local).AddTicks(3795),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 347, DateTimeKind.Local).AddTicks(3747));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 481, DateTimeKind.Local).AddTicks(4767),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 346, DateTimeKind.Local).AddTicks(1096));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 495, DateTimeKind.Local).AddTicks(9125),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 357, DateTimeKind.Local).AddTicks(5830));

            migrationBuilder.AddColumn<int>(
                name: "FiscalMonth",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 487, DateTimeKind.Local).AddTicks(7017),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 351, DateTimeKind.Local).AddTicks(9836));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 494, DateTimeKind.Local).AddTicks(5090),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 356, DateTimeKind.Local).AddTicks(7881));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 495, DateTimeKind.Local).AddTicks(2868),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 357, DateTimeKind.Local).AddTicks(2392));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 491, DateTimeKind.Local).AddTicks(9172),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 355, DateTimeKind.Local).AddTicks(5222));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 11, 22, 7, 480, DateTimeKind.Utc).AddTicks(8799),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 12, 11, 10, 44, 345, DateTimeKind.Utc).AddTicks(7702));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 494, DateTimeKind.Local).AddTicks(969),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 356, DateTimeKind.Local).AddTicks(5031));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 493, DateTimeKind.Local).AddTicks(887),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 356, DateTimeKind.Local).AddTicks(582));

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPeriod_CompoKeys",
                schema: "kenuser",
                table: "PayrollPeriod",
                columns: new[] { "FiscalYear", "FiscalMonth", "PayrollStartDate", "PayrollEndDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PayrollPeriod_CompoKeys",
                schema: "kenuser",
                table: "PayrollPeriod");

            migrationBuilder.DropColumn(
                name: "FiscalMonth",
                schema: "kenuser",
                table: "PayrollPeriod");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 347, DateTimeKind.Local).AddTicks(3747),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 483, DateTimeKind.Local).AddTicks(3795));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 346, DateTimeKind.Local).AddTicks(1096),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 481, DateTimeKind.Local).AddTicks(4767));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 357, DateTimeKind.Local).AddTicks(5830),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 495, DateTimeKind.Local).AddTicks(9125));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 351, DateTimeKind.Local).AddTicks(9836),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 487, DateTimeKind.Local).AddTicks(7017));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 356, DateTimeKind.Local).AddTicks(7881),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 494, DateTimeKind.Local).AddTicks(5090));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 357, DateTimeKind.Local).AddTicks(2392),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 495, DateTimeKind.Local).AddTicks(2868));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 355, DateTimeKind.Local).AddTicks(5222),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 491, DateTimeKind.Local).AddTicks(9172));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 11, 10, 44, 345, DateTimeKind.Utc).AddTicks(7702),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 12, 11, 22, 7, 480, DateTimeKind.Utc).AddTicks(8799));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 356, DateTimeKind.Local).AddTicks(5031),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 494, DateTimeKind.Local).AddTicks(969));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 356, DateTimeKind.Local).AddTicks(582),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 22, 7, 493, DateTimeKind.Local).AddTicks(887));

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPeriod_CompoKeys",
                schema: "kenuser",
                table: "PayrollPeriod",
                columns: new[] { "FiscalYear", "PayrollStartDate", "PayrollEndDate" },
                unique: true);
        }
    }
}
