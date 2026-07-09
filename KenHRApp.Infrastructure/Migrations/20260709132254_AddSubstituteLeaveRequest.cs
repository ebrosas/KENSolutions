using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubstituteLeaveRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubstituteName",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubstituteNo",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubstituteName",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.DropColumn(
                name: "SubstituteNo",
                schema: "kenuser",
                table: "LeaveRequisitionWF");
        }
    }
}
