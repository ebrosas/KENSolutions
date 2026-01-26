using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateHolidayEntity : Migration
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
                defaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 55, DateTimeKind.Local).AddTicks(1502),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 7, 22, 42, 23, 426, DateTimeKind.Local).AddTicks(584));

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
                oldDefaultValue: new DateTime(2026, 1, 7, 22, 42, 23, 422, DateTimeKind.Local).AddTicks(9110));

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
                oldDefaultValue: new DateTime(2026, 1, 7, 22, 42, 23, 436, DateTimeKind.Local).AddTicks(213));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 26, 11, 37, 54, 53, DateTimeKind.Utc).AddTicks(8099),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 7, 19, 42, 23, 421, DateTimeKind.Utc).AddTicks(9104));

            migrationBuilder.CreateTable(
                name: "Holiday",
                schema: "kenuser",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayDesc = table.Column<string>(type: "varchar(200)", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    HolidayType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedByEmpNo = table.Column<int>(type: "int", nullable: true),
                    CreatedByName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 59, DateTimeKind.Local).AddTicks(3344)),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateEmpNo = table.Column<int>(type: "int", nullable: true),
                    LastUpdateUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday_HolidayId", x => x.HolidayId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_CompoKeys",
                schema: "kenuser",
                table: "Holiday",
                columns: new[] { "HolidayDesc", "HolidayDate", "HolidayType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holiday",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 7, 22, 42, 23, 426, DateTimeKind.Local).AddTicks(584),
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
                defaultValue: new DateTime(2026, 1, 7, 22, 42, 23, 422, DateTimeKind.Local).AddTicks(9110),
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
                defaultValue: new DateTime(2026, 1, 7, 22, 42, 23, 436, DateTimeKind.Local).AddTicks(213),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 26, 14, 37, 54, 57, DateTimeKind.Local).AddTicks(2603));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 7, 19, 42, 23, 421, DateTimeKind.Utc).AddTicks(9104),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 26, 11, 37, 54, 53, DateTimeKind.Utc).AddTicks(8099));
        }
    }
}
