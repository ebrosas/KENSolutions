using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOtherDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherDocument_Employee_EmployeeNo",
                schema: "kenuser",
                table: "OtherDocument");

            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "kenuser",
                table: "OtherDocument");

            migrationBuilder.AddColumn<string>(
                name: "ContentTypeCode",
                schema: "kenuser",
                table: "OtherDocument",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 14, 9, 44, 50, 338, DateTimeKind.Utc).AddTicks(4267),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 13, 14, 38, 25, 914, DateTimeKind.Utc).AddTicks(9266));

            migrationBuilder.AddForeignKey(
                name: "FK_OtherDocument_Employee_EmployeeNo",
                schema: "kenuser",
                table: "OtherDocument",
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
                name: "FK_OtherDocument_Employee_EmployeeNo",
                schema: "kenuser",
                table: "OtherDocument");

            migrationBuilder.DropColumn(
                name: "ContentTypeCode",
                schema: "kenuser",
                table: "OtherDocument");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "kenuser",
                table: "OtherDocument",
                type: "nvarchar(max)",
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
                oldDefaultValue: new DateTime(2025, 9, 14, 9, 44, 50, 338, DateTimeKind.Utc).AddTicks(4267));

            migrationBuilder.AddForeignKey(
                name: "FK_OtherDocument_Employee_EmployeeNo",
                schema: "kenuser",
                table: "OtherDocument",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
