using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkflowCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Expression",
                schema: "kenuser",
                table: "WorkflowConditions",
                type: "varchar(500)",
                nullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "WorkflowApprovalRoles",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 249, DateTimeKind.Local).AddTicks(3490),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 386, DateTimeKind.Local).AddTicks(1520));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RequestApprovals",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 245, DateTimeKind.Local).AddTicks(711),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 377, DateTimeKind.Local).AddTicks(6813));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 287, DateTimeKind.Local).AddTicks(9364),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 421, DateTimeKind.Local).AddTicks(8460));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 286, DateTimeKind.Local).AddTicks(5228),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 420, DateTimeKind.Local).AddTicks(9341));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 295, DateTimeKind.Local).AddTicks(6506),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(6123));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 290, DateTimeKind.Local).AddTicks(4219),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 423, DateTimeKind.Local).AddTicks(8779));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "LeaveCreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveRequisitionWF",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 293, DateTimeKind.Local).AddTicks(9696),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 428, DateTimeKind.Local).AddTicks(4709));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "EffectiveDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 295, DateTimeKind.Local).AddTicks(2169),
            //    comment: "Part of composite unique key index",
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1851),
            //    oldComment: "Part of composite unique key index");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 295, DateTimeKind.Local).AddTicks(1736),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1191));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 292, DateTimeKind.Local).AddTicks(6091),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(6139));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 14, 9, 42, 51, 285, DateTimeKind.Utc).AddTicks(7300),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 12, 12, 30, 48, 420, DateTimeKind.Utc).AddTicks(6769));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 293, DateTimeKind.Local).AddTicks(2658),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 427, DateTimeKind.Local).AddTicks(3490));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 292, DateTimeKind.Local).AddTicks(9406),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(9269));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expression",
                schema: "kenuser",
                table: "WorkflowConditions");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "WorkflowApprovalRoles",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 386, DateTimeKind.Local).AddTicks(1520),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 249, DateTimeKind.Local).AddTicks(3490));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RequestApprovals",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 377, DateTimeKind.Local).AddTicks(6813),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 245, DateTimeKind.Local).AddTicks(711));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 421, DateTimeKind.Local).AddTicks(8460),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 287, DateTimeKind.Local).AddTicks(9364));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 420, DateTimeKind.Local).AddTicks(9341),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 286, DateTimeKind.Local).AddTicks(5228));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(6123),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 295, DateTimeKind.Local).AddTicks(6506));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 423, DateTimeKind.Local).AddTicks(8779),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 290, DateTimeKind.Local).AddTicks(4219));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "LeaveCreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveRequisitionWF",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 428, DateTimeKind.Local).AddTicks(4709),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 293, DateTimeKind.Local).AddTicks(9696));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "EffectiveDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1851),
            //    comment: "Part of composite unique key index",
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 295, DateTimeKind.Local).AddTicks(2169),
            //    oldComment: "Part of composite unique key index");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 430, DateTimeKind.Local).AddTicks(1191),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 295, DateTimeKind.Local).AddTicks(1736));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(6139),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 292, DateTimeKind.Local).AddTicks(6091));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 12, 12, 30, 48, 420, DateTimeKind.Utc).AddTicks(6769),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 14, 9, 42, 51, 285, DateTimeKind.Utc).AddTicks(7300));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 427, DateTimeKind.Local).AddTicks(3490),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 293, DateTimeKind.Local).AddTicks(2658));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 12, 15, 30, 48, 426, DateTimeKind.Local).AddTicks(9269),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 14, 12, 42, 51, 292, DateTimeKind.Local).AddTicks(9406));
        }
    }
}
