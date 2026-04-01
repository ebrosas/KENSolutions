using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyLeaveEntitlement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveEntitlement_CompoKeys",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.DropColumn(
                name: "SickLeaveEntitlemnt",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.RenameColumn(
                name: "LeaveEntitlemnt",
                schema: "kenuser",
                table: "LeaveEntitlement",
                newName: "ALEntitlementCount");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 17, DateTimeKind.Local).AddTicks(7740),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 308, DateTimeKind.Local).AddTicks(96));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 15, DateTimeKind.Local).AddTicks(9937),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 306, DateTimeKind.Local).AddTicks(7810));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 34, DateTimeKind.Local).AddTicks(1303),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 320, DateTimeKind.Local).AddTicks(7767));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 21, DateTimeKind.Local).AddTicks(9453),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 311, DateTimeKind.Local).AddTicks(5999));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "LeaveCreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveRequisitionWF",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 30, DateTimeKind.Local).AddTicks(1607),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 317, DateTimeKind.Local).AddTicks(6837));

            migrationBuilder.AlterColumn<string>(
                name: "LeaveUOM",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldComment: "Part of composite unique key index");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 33, DateTimeKind.Local).AddTicks(5840),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 320, DateTimeKind.Local).AddTicks(2348));

            migrationBuilder.AddColumn<string>(
                name: "ALRenewalType",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 33, DateTimeKind.Local).AddTicks(6851),
                comment: "Part of composite unique key index");

            migrationBuilder.AddColumn<double>(
                name: "SLEntitlementCount",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SLRenewalType",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SickLeaveUOM",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "varchar(20)",
                nullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 26, DateTimeKind.Local).AddTicks(9254),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 315, DateTimeKind.Local).AddTicks(3668));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 1, 18, 59, 23, 15, DateTimeKind.Utc).AddTicks(4640),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 3, 18, 13, 9, 22, 306, DateTimeKind.Utc).AddTicks(4015));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 27, DateTimeKind.Local).AddTicks(9927),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 316, DateTimeKind.Local).AddTicks(5100));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 27, DateTimeKind.Local).AddTicks(4515),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 315, DateTimeKind.Local).AddTicks(9006));

            migrationBuilder.CreateIndex(
                name: "IX_LeaveEntitlement_CompoKeys",
                schema: "kenuser",
                table: "LeaveEntitlement",
                columns: new[] { "EmployeeNo", "EffectiveDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveEntitlement_CompoKeys",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.DropColumn(
                name: "ALRenewalType",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.DropColumn(
                name: "SLEntitlementCount",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.DropColumn(
                name: "SLRenewalType",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.DropColumn(
                name: "SickLeaveUOM",
                schema: "kenuser",
                table: "LeaveEntitlement");

            migrationBuilder.RenameColumn(
                name: "ALEntitlementCount",
                schema: "kenuser",
                table: "LeaveEntitlement",
                newName: "LeaveEntitlemnt");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 308, DateTimeKind.Local).AddTicks(96),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 17, DateTimeKind.Local).AddTicks(7740));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 306, DateTimeKind.Local).AddTicks(7810),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 15, DateTimeKind.Local).AddTicks(9937));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 320, DateTimeKind.Local).AddTicks(7767),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 34, DateTimeKind.Local).AddTicks(1303));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 311, DateTimeKind.Local).AddTicks(5999),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 21, DateTimeKind.Local).AddTicks(9453));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "LeaveCreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveRequisitionWF",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 317, DateTimeKind.Local).AddTicks(6837),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 30, DateTimeKind.Local).AddTicks(1607));

            migrationBuilder.AlterColumn<string>(
                name: "LeaveUOM",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "char(1)",
                nullable: false,
                comment: "Part of composite unique key index",
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 320, DateTimeKind.Local).AddTicks(2348),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 33, DateTimeKind.Local).AddTicks(5840));

            migrationBuilder.AddColumn<double>(
                name: "SickLeaveEntitlemnt",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 315, DateTimeKind.Local).AddTicks(3668),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 26, DateTimeKind.Local).AddTicks(9254));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 3, 18, 13, 9, 22, 306, DateTimeKind.Utc).AddTicks(4015),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 1, 18, 59, 23, 15, DateTimeKind.Utc).AddTicks(4640));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 316, DateTimeKind.Local).AddTicks(5100),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 27, DateTimeKind.Local).AddTicks(9927));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 18, 16, 9, 22, 315, DateTimeKind.Local).AddTicks(9006),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 27, DateTimeKind.Local).AddTicks(4515));

            migrationBuilder.CreateIndex(
                name: "IX_LeaveEntitlement_CompoKeys",
                schema: "kenuser",
                table: "LeaveEntitlement",
                columns: new[] { "EmployeeNo", "LeaveEntitlemnt", "LeaveUOM" },
                unique: true);
        }
    }
}
