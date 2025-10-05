using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePresentPermanentCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermanentCityCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentCityCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "PermanentCity",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentCity",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(100)",
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
                oldDefaultValue: new DateTime(2025, 9, 4, 11, 54, 1, 584, DateTimeKind.Utc).AddTicks(9617));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermanentCity",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentCity",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "PermanentCityCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentCityCode",
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
                defaultValue: new DateTime(2025, 9, 4, 11, 54, 1, 584, DateTimeKind.Utc).AddTicks(9617),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 4, 20, 38, 47, 531, DateTimeKind.Utc).AddTicks(5158));
        }
    }
}
