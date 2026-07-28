using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFamilyMemberStateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateCode",
                schema: "kenuser",
                table: "FamilyMember");

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                schema: "kenuser",
                table: "FamilyMember",
                type: "varchar(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateName",
                schema: "kenuser",
                table: "FamilyMember");

            migrationBuilder.AddColumn<string>(
                name: "StateCode",
                schema: "kenuser",
                table: "FamilyMember",
                type: "varchar(20)",
                nullable: true);
        }
    }
}
