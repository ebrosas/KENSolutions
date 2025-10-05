using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLanguageSkillEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "LanguageSkill");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 20, 26, 28, 261, DateTimeKind.Utc).AddTicks(1602),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 20, 10, 30, 205, DateTimeKind.Utc).AddTicks(8167));

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "LanguageSkill",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "LanguageSkill");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 20, 10, 30, 205, DateTimeKind.Utc).AddTicks(8167),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 20, 26, 28, 261, DateTimeKind.Utc).AddTicks(1602));

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "LanguageSkill",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
