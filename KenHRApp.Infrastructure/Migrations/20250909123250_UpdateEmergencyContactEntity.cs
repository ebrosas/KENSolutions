using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmergencyContactEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmergencyContact_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmergencyContact");

            migrationBuilder.DropColumn(
                name: "CityCode",
                schema: "kenuser",
                table: "EmergencyContact");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "kenuser",
                table: "EmergencyContact",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 12, 32, 50, 169, DateTimeKind.Utc).AddTicks(4237),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 4, 20, 38, 47, 531, DateTimeKind.Utc).AddTicks(5158));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Employee_EmployeeNo",
                schema: "kenuser",
                table: "Employee",
                column: "EmployeeNo");

            migrationBuilder.AddForeignKey(
                name: "FK_EmergencyContact_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmergencyContact",
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
                name: "FK_EmergencyContact_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmergencyContact");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Employee_EmployeeNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "kenuser",
                table: "EmergencyContact");

            migrationBuilder.AddColumn<string>(
                name: "CityCode",
                schema: "kenuser",
                table: "EmergencyContact",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 4, 20, 38, 47, 531, DateTimeKind.Utc).AddTicks(5158),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 12, 32, 50, 169, DateTimeKind.Utc).AddTicks(4237));

            migrationBuilder.AddForeignKey(
                name: "FK_EmergencyContact_Employee_EmployeeNo",
                schema: "kenuser",
                table: "EmergencyContact",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
