using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovalStageDesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalStageDesc",
                schema: "kenuser",
                table: "WorkflowStepDefinitions",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "GroupType",
                schema: "kenuser",
                table: "WorkflowApprovalRoles",
                type: "tinyint",
                nullable: false,
                comment: "0 = Assignee Employee; 1 = Supervisor; 2 = Superintendent; 3 = Cost Center Manager",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "0 or NULL = Based on Assigned Employee No; 1 = Based on Employee Master; 2 = Based on Department Master");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStageDesc",
                schema: "kenuser",
                table: "WorkflowStepDefinitions");

            migrationBuilder.AlterColumn<byte>(
                name: "GroupType",
                schema: "kenuser",
                table: "WorkflowApprovalRoles",
                type: "tinyint",
                nullable: false,
                comment: "0 or NULL = Based on Assigned Employee No; 1 = Based on Employee Master; 2 = Based on Department Master",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "0 = Assignee Employee; 1 = Supervisor; 2 = Superintendent; 3 = Cost Center Manager");
        }
    }
}
