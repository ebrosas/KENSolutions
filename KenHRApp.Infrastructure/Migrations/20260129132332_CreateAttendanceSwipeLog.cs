using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAttendanceSwipeLog : Migration
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
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 269, DateTimeKind.Local).AddTicks(5472),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 55, DateTimeKind.Local).AddTicks(1502));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 268, DateTimeKind.Local).AddTicks(193),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 54, DateTimeKind.Local).AddTicks(1033));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 272, DateTimeKind.Local).AddTicks(7822),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 57, DateTimeKind.Local).AddTicks(2603));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 276, DateTimeKind.Local).AddTicks(3697),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 59, DateTimeKind.Local).AddTicks(3344));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 29, 13, 23, 31, 267, DateTimeKind.Utc).AddTicks(5283),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 26, 11, 37, 54, 53, DateTimeKind.Utc).AddTicks(8099));

            migrationBuilder.CreateTable(
                name: "AttendanceSwipeLog",
                schema: "kenuser",
                columns: table => new
                {
                    SwipeID = table.Column<double>(type: "float", nullable: false),
                    EmpNo = table.Column<int>(type: "int", nullable: false),
                    SwipeDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    SwipeTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    SwipeCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    LocationCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    ReaderCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    StatusCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    SwipeLogDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 276, DateTimeKind.Local).AddTicks(6108))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceSwipeLog_SwipeID", x => x.SwipeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSwipeLog_CompoKeys",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                columns: new[] { "EmpNo", "SwipeDate", "SwipeTime" },
                unique: true,
                filter: "[SwipeTime] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceSwipeLog",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 55, DateTimeKind.Local).AddTicks(1502),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 269, DateTimeKind.Local).AddTicks(5472));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 54, DateTimeKind.Local).AddTicks(1033),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 268, DateTimeKind.Local).AddTicks(193));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 57, DateTimeKind.Local).AddTicks(2603),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 272, DateTimeKind.Local).AddTicks(7822));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 59, DateTimeKind.Local).AddTicks(3344),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 29, 16, 23, 31, 276, DateTimeKind.Local).AddTicks(3697));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 26, 11, 37, 54, 53, DateTimeKind.Utc).AddTicks(8099),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 29, 13, 23, 31, 267, DateTimeKind.Utc).AddTicks(5283));
        }
    }
}
