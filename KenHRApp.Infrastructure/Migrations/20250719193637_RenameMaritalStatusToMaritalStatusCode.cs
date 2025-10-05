using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameMaritalStatusToMaritalStatusCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaritalStatus",
                schema: "kenuser",
                table: "Employee",
                newName: "MaritalStatusCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaritalStatusCode",
                schema: "kenuser",
                table: "Employee",
                newName: "MaritalStatus");
        }
    }
}
