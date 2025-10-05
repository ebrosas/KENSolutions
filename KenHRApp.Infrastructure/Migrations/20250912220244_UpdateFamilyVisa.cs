using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFamilyVisa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyVisa_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyVisa");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 12, 22, 2, 41, 402, DateTimeKind.Utc).AddTicks(558),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 12, 20, 28, 31, 545, DateTimeKind.Utc).AddTicks(2857));

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyVisa_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyVisa",
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
                name: "FK_FamilyVisa_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyVisa");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 12, 20, 28, 31, 545, DateTimeKind.Utc).AddTicks(2857),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 12, 22, 2, 41, 402, DateTimeKind.Utc).AddTicks(558));

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyVisa_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyVisa",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
