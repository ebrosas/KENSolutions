using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "kenuser",
                table: "SupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Requester",
                schema: "kenuser",
                table: "SupportTickets",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "kenuser",
                table: "SupportTickets",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 573, DateTimeKind.Local).AddTicks(4848),
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
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 572, DateTimeKind.Local).AddTicks(4608),
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
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(2848),
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
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 575, DateTimeKind.Local).AddTicks(2679),
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
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 578, DateTimeKind.Local).AddTicks(1232),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 30, DateTimeKind.Local).AddTicks(1607));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "EffectiveDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(937),
            //    comment: "Part of composite unique key index",
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 33, DateTimeKind.Local).AddTicks(6851),
            //    oldComment: "Part of composite unique key index");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(539),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 33, DateTimeKind.Local).AddTicks(5840));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(564),
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
            //    defaultValue: new DateTime(2026, 4, 7, 9, 14, 58, 572, DateTimeKind.Utc).AddTicks(142),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 1, 18, 59, 23, 15, DateTimeKind.Utc).AddTicks(4640));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(5968),
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
            //    defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(3329),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 27, DateTimeKind.Local).AddTicks(4515));

            migrationBuilder.CreateTable(
                name: "RequestApprovals",
                schema: "kenuser",
                columns: table => new
                {
                    ApprovalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestTypeCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    RequisitionNo = table.Column<long>(type: "bigint", nullable: false),
                    RoutineSequence = table.Column<int>(type: "int", nullable: true),
                    AssignedEmpNo = table.Column<int>(type: "int", nullable: false),
                    AssignedEmpName = table.Column<string>(type: "varchar(150)", nullable: true),
                    ApprovalRole = table.Column<string>(type: "varchar(150)", nullable: true),
                    ActionRole = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsHold = table.Column<bool>(type: "bit", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(500)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 550, DateTimeKind.Local).AddTicks(4215)),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedUserID = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestApproval_ApprovalID", x => x.ApprovalId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestApproval_CompoKeys",
                schema: "kenuser",
                table: "RequestApprovals",
                columns: new[] { "RequestTypeCode", "RequisitionNo", "AssignedEmpNo", "ActionRole" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestApprovals",
                schema: "kenuser");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "kenuser",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Requester",
                schema: "kenuser",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "kenuser",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 573, DateTimeKind.Local).AddTicks(4848));

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 572, DateTimeKind.Local).AddTicks(4608));

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(2848));

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 575, DateTimeKind.Local).AddTicks(2679));

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 578, DateTimeKind.Local).AddTicks(1232));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "EffectiveDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 33, DateTimeKind.Local).AddTicks(6851),
            //    comment: "Part of composite unique key index",
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(937),
            //    oldComment: "Part of composite unique key index");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 4, 1, 21, 59, 23, 33, DateTimeKind.Local).AddTicks(5840),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 579, DateTimeKind.Local).AddTicks(539));

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(564));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 4, 1, 18, 59, 23, 15, DateTimeKind.Utc).AddTicks(4640),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 4, 7, 9, 14, 58, 572, DateTimeKind.Utc).AddTicks(142));

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(5968));

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
            //    oldDefaultValue: new DateTime(2026, 4, 7, 12, 14, 58, 577, DateTimeKind.Local).AddTicks(3329));
        }
    }
}
