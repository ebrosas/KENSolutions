using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQualificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Qualification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "Qualification");

            migrationBuilder.DropColumn(
                name: "CityCode",
                schema: "kenuser",
                table: "Qualification");

            migrationBuilder.AddColumn<string>(
                name: "CityTownName",
                schema: "kenuser",
                table: "Qualification",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 19, 56, 0, 222, DateTimeKind.Utc).AddTicks(796),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 13, 45, 55, 127, DateTimeKind.Utc).AddTicks(7964));

            migrationBuilder.AddForeignKey(
                name: "FK_Qualification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "Qualification",
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
                name: "FK_Qualification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "Qualification");

            migrationBuilder.DropColumn(
                name: "CityTownName",
                schema: "kenuser",
                table: "Qualification");

            migrationBuilder.AddColumn<string>(
                name: "CityCode",
                schema: "kenuser",
                table: "Qualification",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 13, 45, 55, 127, DateTimeKind.Utc).AddTicks(7964),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 19, 56, 0, 222, DateTimeKind.Utc).AddTicks(796));

            migrationBuilder.AddForeignKey(
                name: "FK_Qualification_Employee_EmployeeNo",
                schema: "kenuser",
                table: "Qualification",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
