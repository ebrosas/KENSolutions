using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToEmployeeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "PayGrade",
                schema: "kenuser",
                table: "Employee",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<string>(
                name: "EmploymentTypeCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstAttendanceModeCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "kenuser",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondAttendanceModeCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondReportingManagerCode",
                schema: "kenuser",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdAttendanceModeCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 28, 13, 9, 44, 455, DateTimeKind.Utc).AddTicks(3735),
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentTypeCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "FirstAttendanceModeCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "RoleCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SecondAttendanceModeCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SecondReportingManagerCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ThirdAttendanceModeCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AlterColumn<byte>(
                name: "PayGrade",
                schema: "kenuser",
                table: "Employee",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 8, 28, 13, 9, 44, 455, DateTimeKind.Utc).AddTicks(3735));
        }
    }
}
