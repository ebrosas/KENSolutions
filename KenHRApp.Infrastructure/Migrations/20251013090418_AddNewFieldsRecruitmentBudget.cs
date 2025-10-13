using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsRecruitmentBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 10, 13, 12, 4, 16, 963, DateTimeKind.Local).AddTicks(507),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 10, 12, 15, 14, 35, 926, DateTimeKind.Local).AddTicks(9672));

            migrationBuilder.AddColumn<string>(
                name: "BudgetDescription",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NewIndentCount",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OnHold",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 13, 9, 4, 16, 962, DateTimeKind.Utc).AddTicks(7736),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 10, 12, 12, 14, 35, 926, DateTimeKind.Utc).AddTicks(6273));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetDescription",
                schema: "kenuser",
                table: "RecruitmentBudget");

            migrationBuilder.DropColumn(
                name: "NewIndentCount",
                schema: "kenuser",
                table: "RecruitmentBudget");

            migrationBuilder.DropColumn(
                name: "OnHold",
                schema: "kenuser",
                table: "RecruitmentBudget");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 10, 12, 15, 14, 35, 926, DateTimeKind.Local).AddTicks(9672),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 10, 13, 12, 4, 16, 963, DateTimeKind.Local).AddTicks(507));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 12, 12, 14, 35, 926, DateTimeKind.Utc).AddTicks(6273),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 10, 13, 9, 4, 16, 962, DateTimeKind.Utc).AddTicks(7736));
        }
    }
}
