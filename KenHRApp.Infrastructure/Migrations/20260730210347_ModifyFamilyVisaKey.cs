using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyFamilyVisaKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FamilyVisa_CompoKeys",
                schema: "kenuser",
                table: "FamilyVisa");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyVisa_CompoKeys",
                schema: "kenuser",
                table: "FamilyVisa",
                columns: new[] { "EmployeeNo", "FamilyId", "VisaTypeCode", "IssueDate", "ExpiryDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FamilyVisa_CompoKeys",
                schema: "kenuser",
                table: "FamilyVisa");
            
            migrationBuilder.CreateIndex(
                name: "IX_FamilyVisa_CompoKeys",
                schema: "kenuser",
                table: "FamilyVisa",
                columns: new[] { "EmployeeNo", "VisaTypeCode", "CountryCode", "IssueDate", "ExpiryDate" },
                unique: true);
        }
    }
}
