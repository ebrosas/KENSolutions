using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMasterShiftPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MasterShiftPattern_CompoKeys",
                schema: "kenuser",
                table: "MasterShiftPattern");

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
                oldDefaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 3, DateTimeKind.Local).AddTicks(2687));

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
                oldDefaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 2, DateTimeKind.Local).AddTicks(4811));

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
                oldDefaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 4, DateTimeKind.Local).AddTicks(7206));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 14, 13, 4, 33, 557, DateTimeKind.Utc).AddTicks(8207),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 12, 8, 13, 50, 34, 2, DateTimeKind.Utc).AddTicks(2859));

            migrationBuilder.CreateIndex(
                name: "IX_MasterShiftPattern_CompoKeys",
                schema: "kenuser",
                table: "MasterShiftPattern",
                columns: new[] { "ShiftPatternCode", "ShiftCode", "ShiftPointer" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MasterShiftPattern_CompoKeys",
                schema: "kenuser",
                table: "MasterShiftPattern");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 3, DateTimeKind.Local).AddTicks(2687),
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
                defaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 2, DateTimeKind.Local).AddTicks(4811),
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
                defaultValue: new DateTime(2025, 12, 8, 16, 50, 34, 4, DateTimeKind.Local).AddTicks(7206),
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
                defaultValue: new DateTime(2025, 12, 8, 13, 50, 34, 2, DateTimeKind.Utc).AddTicks(2859),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 12, 14, 13, 4, 33, 557, DateTimeKind.Utc).AddTicks(8207));

            migrationBuilder.CreateIndex(
                name: "IX_MasterShiftPattern_CompoKeys",
                schema: "kenuser",
                table: "MasterShiftPattern",
                columns: new[] { "ShiftPatternCode", "ShiftCode" },
                unique: true);
        }
    }
}
