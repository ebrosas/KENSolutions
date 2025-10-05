using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldsEmployeeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "BankBranchCode",
                schema: "kenuser",
                table: "Employee",
                newName: "CompanyBranchCode");

            migrationBuilder.AlterColumn<string>(
                name: "PayGrade",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankBranchName",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitleCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 4, 11, 54, 1, 584, DateTimeKind.Utc).AddTicks(9617),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 8, 28, 13, 9, 44, 455, DateTimeKind.Utc).AddTicks(3735));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankBranchName",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "JobTitleCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "CompanyBranchCode",
                schema: "kenuser",
                table: "Employee",
                newName: "BankBranchCode");

            migrationBuilder.AlterColumn<byte>(
                name: "PayGrade",
                schema: "kenuser",
                table: "Employee",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldDefaultValue: "0");

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 28, 13, 9, 44, 455, DateTimeKind.Utc).AddTicks(3735),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 4, 11, 54, 1, 584, DateTimeKind.Utc).AddTicks(9617));
        }
    }
}
