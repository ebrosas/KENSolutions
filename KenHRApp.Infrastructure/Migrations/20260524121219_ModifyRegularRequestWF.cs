using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRegularRequestWF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusCode",
                schema: "kenuser",
                table: "RegularRequestWFs",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StatusHandlingCode",
                schema: "kenuser",
                table: "RegularRequestWFs",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                schema: "kenuser",
                table: "RegularRequestWFs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusCode",
                schema: "kenuser",
                table: "RegularRequestWFs");

            migrationBuilder.DropColumn(
                name: "StatusHandlingCode",
                schema: "kenuser",
                table: "RegularRequestWFs");

            migrationBuilder.DropColumn(
                name: "StatusID",
                schema: "kenuser",
                table: "RegularRequestWFs");
        }
    }
}
