using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeSkillEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeSkill");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 20, 10, 30, 205, DateTimeKind.Utc).AddTicks(8167),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 19, 56, 0, 222, DateTimeKind.Utc).AddTicks(796));

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeSkill",
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
                name: "FK_EmployeeSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeSkill");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 19, 56, 0, 222, DateTimeKind.Utc).AddTicks(796),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 20, 10, 30, 205, DateTimeKind.Utc).AddTicks(8167));

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkill_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeSkill",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
