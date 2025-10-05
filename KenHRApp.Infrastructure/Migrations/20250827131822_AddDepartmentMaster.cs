using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReportingManagerCode",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeNo",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DepartmentMaster",
                schema: "kenuser",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    DepartmentName = table.Column<string>(type: "varchar(120)", nullable: false, comment: "Part of composite unique key index"),
                    GroupCode = table.Column<string>(type: "varchar(20)", nullable: true, comment: "Part of composite unique key index"),
                    Description = table.Column<string>(type: "varchar(150)", nullable: true),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    SuperintendentEmpNo = table.Column<int>(type: "int", nullable: true, comment: "Part of composite unique key index"),
                    ManagerEmpNo = table.Column<int>(type: "int", nullable: true, comment: "Part of composite unique key index"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentMaster_DeptId", x => x.DepartmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentMaster_CompoKeys",
                schema: "kenuser",
                table: "DepartmentMaster",
                columns: new[] { "DepartmentCode", "DepartmentName", "GroupCode", "SuperintendentEmpNo", "ManagerEmpNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentMaster",
                schema: "kenuser");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "kenuser",
                table: "EmployeeMasters");

            migrationBuilder.AlterColumn<int>(
                name: "ReportingManagerCode",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeNo",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                schema: "kenuser",
                table: "EmployeeMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
