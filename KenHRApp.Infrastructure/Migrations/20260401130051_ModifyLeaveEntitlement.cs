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
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 713, DateTimeKind.Local).AddTicks(9497),
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
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 711, DateTimeKind.Local).AddTicks(8626),
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
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 728, DateTimeKind.Local).AddTicks(7152),
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
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 718, DateTimeKind.Local).AddTicks(4565),
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
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 725, DateTimeKind.Local).AddTicks(8325),
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
                defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 728, DateTimeKind.Local).AddTicks(1740),
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
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
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
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 723, DateTimeKind.Local).AddTicks(2195),
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
            //    defaultValue: new DateTime(2026, 4, 1, 13, 0, 49, 711, DateTimeKind.Utc).AddTicks(4200),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 3, 18, 13, 9, 22, 306, DateTimeKind.Utc).AddTicks(4015));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 724, DateTimeKind.Local).AddTicks(4472),
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
            //    defaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 723, DateTimeKind.Local).AddTicks(8220),
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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 713, DateTimeKind.Local).AddTicks(9497));

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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 711, DateTimeKind.Local).AddTicks(8626));

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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 728, DateTimeKind.Local).AddTicks(7152));

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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 718, DateTimeKind.Local).AddTicks(4565));

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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 725, DateTimeKind.Local).AddTicks(8325));

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
                oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 728, DateTimeKind.Local).AddTicks(1740));

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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 723, DateTimeKind.Local).AddTicks(2195));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 3, 18, 13, 9, 22, 306, DateTimeKind.Utc).AddTicks(4015),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 1, 13, 0, 49, 711, DateTimeKind.Utc).AddTicks(4200));

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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 724, DateTimeKind.Local).AddTicks(4472));

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
            //    oldDefaultValue: new DateTime(2026, 4, 1, 16, 0, 49, 723, DateTimeKind.Local).AddTicks(8220));

            migrationBuilder.CreateIndex(
                name: "IX_LeaveEntitlement_CompoKeys",
                schema: "kenuser",
                table: "LeaveEntitlement",
                columns: new[] { "EmployeeNo", "LeaveEntitlemnt", "LeaveUOM" },
                unique: true);
        }
    }
}
