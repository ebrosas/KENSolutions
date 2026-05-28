using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldRegularRequestWF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoPayHours",
                schema: "kenuser",
                table: "RegularRequestWFs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShiftTiming",
                schema: "kenuser",
                table: "RegularRequestWFs",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkDuration",
                schema: "kenuser",
                table: "RegularRequestWFs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoPayHours",
                schema: "kenuser",
                table: "RegularRequestWFs");

            migrationBuilder.DropColumn(
                name: "ShiftTiming",
                schema: "kenuser",
                table: "RegularRequestWFs");

            migrationBuilder.DropColumn(
                name: "WorkDuration",
                schema: "kenuser",
                table: "RegularRequestWFs");
        }
    }
}
