using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFamilyMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMember_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyMember");

            migrationBuilder.DropColumn(
                name: "CityCode",
                schema: "kenuser",
                table: "FamilyMember");

            migrationBuilder.AddColumn<int>(
                name: "FamilyId",
                schema: "kenuser",
                table: "FamilyVisa",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign key that references primary key: FamilyMember.AutoId");

            migrationBuilder.AddColumn<string>(
                name: "CityTownName",
                schema: "kenuser",
                table: "FamilyMember",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 13, 14, 38, 25, 914, DateTimeKind.Utc).AddTicks(9266),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 12, 22, 2, 41, 402, DateTimeKind.Utc).AddTicks(558));

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMember_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyMember",
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
                name: "FK_FamilyMember_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyMember");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                schema: "kenuser",
                table: "FamilyVisa");

            migrationBuilder.DropColumn(
                name: "CityTownName",
                schema: "kenuser",
                table: "FamilyMember");

            migrationBuilder.AddColumn<string>(
                name: "CityCode",
                schema: "kenuser",
                table: "FamilyMember",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 12, 22, 2, 41, 402, DateTimeKind.Utc).AddTicks(558),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 13, 14, 38, 25, 914, DateTimeKind.Utc).AddTicks(9266));

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMember_Employee_EmployeeNo",
                schema: "kenuser",
                table: "FamilyMember",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
