using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPayrollPeriod : Migration
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
                defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 347, DateTimeKind.Local).AddTicks(3747),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 272, DateTimeKind.Local).AddTicks(601));

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
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 270, DateTimeKind.Local).AddTicks(3688));

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
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 275, DateTimeKind.Local).AddTicks(3871));

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
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 279, DateTimeKind.Local).AddTicks(8491));

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
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 280, DateTimeKind.Local).AddTicks(2873));

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
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 278, DateTimeKind.Local).AddTicks(6904));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 12, 11, 10, 44, 345, DateTimeKind.Utc).AddTicks(7702),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 9, 18, 56, 41, 269, DateTimeKind.Utc).AddTicks(8994));

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
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 279, DateTimeKind.Local).AddTicks(5595));

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
                oldDefaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 279, DateTimeKind.Local).AddTicks(1141));

            migrationBuilder.CreateTable(
                name: "PayrollPeriod",
                schema: "kenuser",
                columns: table => new
                {
                    PayrollPeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    PayrollStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PayrollEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmpNo = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 357, DateTimeKind.Local).AddTicks(5830)),
                    LastUpdateEmpNo = table.Column<int>(type: "int", nullable: true),
                    LastUpdateUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollPeriod_PayrollPeriodId", x => x.PayrollPeriodId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPeriod_CompoKeys",
                schema: "kenuser",
                table: "PayrollPeriod",
                columns: new[] { "FiscalYear", "PayrollStartDate", "PayrollEndDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollPeriod",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 272, DateTimeKind.Local).AddTicks(601),
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
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 270, DateTimeKind.Local).AddTicks(3688),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 346, DateTimeKind.Local).AddTicks(1096));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 275, DateTimeKind.Local).AddTicks(3871),
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
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 279, DateTimeKind.Local).AddTicks(8491),
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
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 280, DateTimeKind.Local).AddTicks(2873),
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
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 278, DateTimeKind.Local).AddTicks(6904),
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
                defaultValue: new DateTime(2026, 2, 9, 18, 56, 41, 269, DateTimeKind.Utc).AddTicks(8994),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 12, 11, 10, 44, 345, DateTimeKind.Utc).AddTicks(7702));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 279, DateTimeKind.Local).AddTicks(5595),
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
                defaultValue: new DateTime(2026, 2, 9, 21, 56, 41, 279, DateTimeKind.Local).AddTicks(1141),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 12, 14, 10, 44, 356, DateTimeKind.Local).AddTicks(582));
        }
    }
}
