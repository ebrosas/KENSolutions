using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEmployeeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "kenuser",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Employee",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "kenuser",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                schema: "kenuser",
                table: "Employee",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstNameAr",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "kenuser",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "kenuser",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastNameAr",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationCode",
                schema: "kenuser",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleNameAr",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "kenuser",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                schema: "kenuser",
                table: "Employee",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "kenuser",
                table: "Employee",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "FirstNameAr",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastNameAr",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MiddleNameAr",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "kenuser",
                table: "Employee");
        }
    }
}
