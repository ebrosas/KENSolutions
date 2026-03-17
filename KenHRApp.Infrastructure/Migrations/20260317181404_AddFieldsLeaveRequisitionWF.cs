using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsLeaveRequisitionWF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 824, DateTimeKind.Local).AddTicks(9069),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 694, DateTimeKind.Local).AddTicks(9216));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 823, DateTimeKind.Local).AddTicks(665),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 692, DateTimeKind.Local).AddTicks(9933));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 839, DateTimeKind.Local).AddTicks(1330),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 706, DateTimeKind.Local).AddTicks(1311));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 831, DateTimeKind.Local).AddTicks(3261),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 698, DateTimeKind.Local).AddTicks(6761));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 837, DateTimeKind.Local).AddTicks(7337),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 704, DateTimeKind.Local).AddTicks(7477));

            migrationBuilder.AddColumn<Guid>(
                name: "LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 838, DateTimeKind.Local).AddTicks(6831),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 705, DateTimeKind.Local).AddTicks(5958));

            migrationBuilder.AddColumn<Guid>(
                name: "LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(3014),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 702, DateTimeKind.Local).AddTicks(5117));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 3, 17, 18, 14, 2, 822, DateTimeKind.Utc).AddTicks(3902),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 3, 16, 18, 41, 19, 692, DateTimeKind.Utc).AddTicks(5475));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 836, DateTimeKind.Local).AddTicks(3511),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(5257));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(8427),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(369));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.DropColumn(
                name: "LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveAttachments");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 694, DateTimeKind.Local).AddTicks(9216),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 824, DateTimeKind.Local).AddTicks(9069));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 692, DateTimeKind.Local).AddTicks(9933),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 823, DateTimeKind.Local).AddTicks(665));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 706, DateTimeKind.Local).AddTicks(1311),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 839, DateTimeKind.Local).AddTicks(1330));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 698, DateTimeKind.Local).AddTicks(6761),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 831, DateTimeKind.Local).AddTicks(3261));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 704, DateTimeKind.Local).AddTicks(7477),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 837, DateTimeKind.Local).AddTicks(7337));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 705, DateTimeKind.Local).AddTicks(5958),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 838, DateTimeKind.Local).AddTicks(6831));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 702, DateTimeKind.Local).AddTicks(5117),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(3014));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 3, 16, 18, 41, 19, 692, DateTimeKind.Utc).AddTicks(5475),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 3, 17, 18, 14, 2, 822, DateTimeKind.Utc).AddTicks(3902));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(5257),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 836, DateTimeKind.Local).AddTicks(3511));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(369),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(8427));
        }
    }
}
