using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEmployeeCertificationStateCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityCode",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.DropColumn(
                name: "StateCode",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.AddColumn<string>(
                name: "CityTownName",
                schema: "kenuser",
                table: "EmployeeCertification",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
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
                defaultValue: new DateTime(2025, 9, 12, 20, 28, 31, 545, DateTimeKind.Utc).AddTicks(2857),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 12, 20, 4, 10, 680, DateTimeKind.Utc).AddTicks(6969));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityTownName",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "kenuser",
                table: "EmployeeCertification");

            migrationBuilder.AddColumn<string>(
                name: "CityCode",
                schema: "kenuser",
                table: "EmployeeCertification",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateCode",
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
                oldDefaultValue: new DateTime(2025, 9, 12, 20, 28, 31, 545, DateTimeKind.Utc).AddTicks(2857));
        }
    }
}
