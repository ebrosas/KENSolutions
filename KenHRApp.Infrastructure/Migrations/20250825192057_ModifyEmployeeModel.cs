using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEmployeeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeStatusID",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeStatusCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeMasters",
                schema: "kenuser",
                columns: table => new
                {
                    EmployeeNo = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmploymentTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportingManagerCode = table.Column<int>(type: "int", nullable: true),
                    ReportingManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeStatusCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeMasters",
                schema: "kenuser");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeStatusCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "EmployeeStatusID",
                schema: "kenuser",
                table: "Employee",
                type: "tinyint",
                nullable: true);
        }
    }
}
