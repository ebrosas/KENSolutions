using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAttendanceTimesheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_LeaveEntitlement_CompoKeys",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 198, DateTimeKind.Local).AddTicks(6094),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 90, DateTimeKind.Local).AddTicks(2030));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 197, DateTimeKind.Local).AddTicks(7058),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 88, DateTimeKind.Local).AddTicks(9948));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 200, DateTimeKind.Local).AddTicks(4370),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 94, DateTimeKind.Local).AddTicks(4866));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "LeaveCreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveRequisitionWF",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(8854),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 100, DateTimeKind.Local).AddTicks(1818));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 203, DateTimeKind.Local).AddTicks(1508),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 100, DateTimeKind.Local).AddTicks(8936));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(2037),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 98, DateTimeKind.Local).AddTicks(6358));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 2, 9, 13, 28, 26, 197, DateTimeKind.Utc).AddTicks(3672),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 2, 8, 12, 58, 27, 88, DateTimeKind.Utc).AddTicks(6235));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(7314),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 99, DateTimeKind.Local).AddTicks(8153));

            migrationBuilder.DropColumn(
                name: "TimesheetId",
                table: "AttendanceTimesheet");

            migrationBuilder.AddColumn<string>(
                name: "TimesheetId",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "bigint",
                nullable: true)
                .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AlterColumn<long>(
            //    name: "TimesheetId",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(double),
            //    oldType: "float")
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(4687),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 99, DateTimeKind.Local).AddTicks(2722));

            //migrationBuilder.CreateIndex(
            //    name: "IX_LeaveEntitlement_CompoKeys",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    columns: new[] { "EmployeeNo", "LeaveEntitlemnt", "LeaveUOM" },
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_LeaveEntitlement_CompoKeys",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 90, DateTimeKind.Local).AddTicks(2030),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 198, DateTimeKind.Local).AddTicks(6094));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 88, DateTimeKind.Local).AddTicks(9948),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 197, DateTimeKind.Local).AddTicks(7058));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 94, DateTimeKind.Local).AddTicks(4866),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 200, DateTimeKind.Local).AddTicks(4370));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "LeaveCreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveRequisitionWF",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 100, DateTimeKind.Local).AddTicks(1818),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(8854));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 100, DateTimeKind.Local).AddTicks(8936),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 203, DateTimeKind.Local).AddTicks(1508));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 98, DateTimeKind.Local).AddTicks(6358),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(2037));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 2, 8, 12, 58, 27, 88, DateTimeKind.Utc).AddTicks(6235),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 2, 9, 13, 28, 26, 197, DateTimeKind.Utc).AddTicks(3672));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 99, DateTimeKind.Local).AddTicks(8153),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(7314));

            migrationBuilder.AlterColumn<double>(
                name: "TimesheetId",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 2, 8, 15, 58, 27, 99, DateTimeKind.Local).AddTicks(2722),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 2, 9, 16, 28, 26, 202, DateTimeKind.Local).AddTicks(4687));

            //migrationBuilder.CreateIndex(
            //    name: "IX_LeaveEntitlement_CompoKeys",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    columns: new[] { "EmployeeNo", "LeaveEntitlemnt" },
            //    unique: true);
        }
    }
}
