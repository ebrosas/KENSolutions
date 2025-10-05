using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmploymentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentHistory_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmploymentHistory");

            migrationBuilder.AlterColumn<int>(
                name: "ReportingManager",
                schema: "kenuser",
                table: "EmploymentHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LastDrawnSalary",
                schema: "kenuser",
                table: "EmploymentHistory",
                type: "decimal(14,3)",
                precision: 14,
                scale: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldPrecision: 14,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 14, 11, 39, 25, 46, DateTimeKind.Utc).AddTicks(4091),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 14, 9, 44, 50, 338, DateTimeKind.Utc).AddTicks(4267));

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentHistory_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmploymentHistory",
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
                name: "FK_EmploymentHistory_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmploymentHistory");

            migrationBuilder.AlterColumn<string>(
                name: "ReportingManager",
                schema: "kenuser",
                table: "EmploymentHistory",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastDrawnSalary",
                schema: "kenuser",
                table: "EmploymentHistory",
                type: "nvarchar(max)",
                precision: 14,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,3)",
                oldPrecision: 14,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 14, 9, 44, 50, 338, DateTimeKind.Utc).AddTicks(4267),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 14, 11, 39, 25, 46, DateTimeKind.Utc).AddTicks(4091));

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentHistory_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmploymentHistory",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
