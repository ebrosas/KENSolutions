using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "GroupType",
                schema: "kenuser",
                table: "WorkflowApprovalRoles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "0 or NULL = Based on Assigned Employee No; 1 = Based on Employee Master; 2 = Based on Department Master");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupType",
                schema: "kenuser",
                table: "WorkflowApprovalRoles");
        }
    }
}
