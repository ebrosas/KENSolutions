using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateMasterShiftPattern : Migration
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
                defaultValue: new DateTime(2026, 1, 7, 22, 42, 23, 426, DateTimeKind.Local).AddTicks(584),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 12, 14, 16, 4, 33, 558, DateTimeKind.Local).AddTicks(9439));

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
                oldDefaultValue: new DateTime(2025, 12, 14, 16, 4, 33, 558, DateTimeKind.Local).AddTicks(804));

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
                oldDefaultValue: new DateTime(2025, 12, 14, 16, 4, 33, 560, DateTimeKind.Local).AddTicks(8121));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 7, 19, 42, 23, 421, DateTimeKind.Utc).AddTicks(9104),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 12, 14, 13, 4, 33, 557, DateTimeKind.Utc).AddTicks(8207));

            migrationBuilder.CreateTable(
                name: "ShiftPatternChange",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpNo = table.Column<int>(type: "int", nullable: false),
                    ShiftPatternCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    ShiftPointer = table.Column<int>(type: "int", nullable: false),
                    ChangeType = table.Column<string>(type: "varchar(20)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedByEmpNo = table.Column<int>(type: "int", nullable: true),
                    CreatedByName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateEmpNo = table.Column<int>(type: "int", nullable: true),
                    LastUpdateUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftPatternChange_AutoId", x => x.AutoId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftPatternChange_CompoKeys",
                schema: "kenuser",
                table: "ShiftPatternChange",
                columns: new[] { "EmpNo", "ShiftPatternCode", "ShiftPointer", "EffectiveDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftPatternChange",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 12, 14, 16, 4, 33, 558, DateTimeKind.Local).AddTicks(9439),
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
                defaultValue: new DateTime(2025, 12, 14, 16, 4, 33, 558, DateTimeKind.Local).AddTicks(804),
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
                defaultValue: new DateTime(2025, 12, 14, 16, 4, 33, 560, DateTimeKind.Local).AddTicks(8121),
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
                defaultValue: new DateTime(2025, 12, 14, 13, 4, 33, 557, DateTimeKind.Utc).AddTicks(8207),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 7, 19, 42, 23, 421, DateTimeKind.Utc).AddTicks(9104));
        }
    }
}
