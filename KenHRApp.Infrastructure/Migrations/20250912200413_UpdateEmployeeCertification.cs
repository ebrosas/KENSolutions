using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeCertification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCertification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.DropColumn(
                name: "Stream",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.AddColumn<string>(
                name: "StreamCode",
                schema: "kenuser",
                table: "EmployeeCertification",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 12, 20, 4, 10, 680, DateTimeKind.Utc).AddTicks(6969),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 20, 26, 28, 261, DateTimeKind.Utc).AddTicks(1602));

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCertification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeCertification",
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
                name: "FK_EmployeeCertification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.DropColumn(
                name: "StreamCode",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.AddColumn<string>(
                name: "Stream",
                schema: "kenuser",
                table: "EmployeeCertification",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 20, 26, 28, 261, DateTimeKind.Utc).AddTicks(1602),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 12, 20, 4, 10, 680, DateTimeKind.Utc).AddTicks(6969));

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCertification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmployeeCertification",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
