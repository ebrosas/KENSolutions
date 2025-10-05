using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityProofEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityProof_Employee_EmployeeNo",
                schema: "kenuser",
                table: "IdentityProof");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 13, 45, 55, 127, DateTimeKind.Utc).AddTicks(7964),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 12, 32, 50, 169, DateTimeKind.Utc).AddTicks(4237));

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityProof_Employee_EmployeeNo",
                schema: "kenuser",
                table: "IdentityProof",
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
                name: "FK_IdentityProof_Employee_EmployeeNo",
                schema: "kenuser",
                table: "IdentityProof");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 9, 12, 32, 50, 169, DateTimeKind.Utc).AddTicks(4237),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 9, 13, 45, 55, 127, DateTimeKind.Utc).AddTicks(7964));

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityProof_Employee_EmployeeNo",
                schema: "kenuser",
                table: "IdentityProof",
                column: "EmployeeNo",
                principalSchema: "kenuser",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
