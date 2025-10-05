using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCompanyBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyBranchCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "CompanyBranch",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 29, 20, 4, 24, 423, DateTimeKind.Utc).AddTicks(9460),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 14, 11, 57, 14, 738, DateTimeKind.Utc).AddTicks(2247));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyBranch",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "CompanyBranchCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 14, 11, 57, 14, 738, DateTimeKind.Utc).AddTicks(2247),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 29, 20, 4, 24, 423, DateTimeKind.Utc).AddTicks(9460));
        }
    }
}
