using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDefinedCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDefinedCodeGroup",
                schema: "kenuser",
                columns: table => new
                {
                    UDCGroupId = table.Column<int>(type: "int", nullable: false, comment: "Primary key of UserDefinedCodeGroup entity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UDCGCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    UDCGDesc1 = table.Column<string>(type: "varchar(150)", nullable: false),
                    UDCGDesc2 = table.Column<string>(type: "varchar(150)", nullable: true),
                    UDCGSpecialHandlingCode = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDefinedCodeGroup_UDCGroupId", x => x.UDCGroupId);
                });

            migrationBuilder.CreateTable(
                name: "UserDefinedCode",
                schema: "kenuser",
                columns: table => new
                {
                    UDCId = table.Column<int>(type: "int", nullable: false, comment: "Primary key of UserDefinedCode entity")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UDCCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    UDCDesc1 = table.Column<string>(type: "varchar(150)", nullable: false),
                    UDCDesc2 = table.Column<string>(type: "varchar(150)", nullable: true),
                    UDCSpecialHandlingCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    SequenceNo = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Amount = table.Column<decimal>(type: "decimal(13,3)", precision: 13, scale: 3, nullable: true),
                    GroupID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDefinedCode_UDCId", x => x.UDCId);
                    table.ForeignKey(
                        name: "FK_UserDefinedCode_UserDefinedCodeGroup_GroupID",
                        column: x => x.GroupID,
                        principalSchema: "kenuser",
                        principalTable: "UserDefinedCodeGroup",
                        principalColumn: "UDCGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDefinedCode_CompoKeys",
                schema: "kenuser",
                table: "UserDefinedCode",
                columns: new[] { "UDCCode", "UDCDesc1", "UDCSpecialHandlingCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDefinedCode_GroupID",
                schema: "kenuser",
                table: "UserDefinedCode",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_UserDefinedCodeGroup_CompoKeys",
                schema: "kenuser",
                table: "UserDefinedCodeGroup",
                columns: new[] { "UDCGCode", "UDCGDesc1", "UDCGSpecialHandlingCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDefinedCode",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "UserDefinedCodeGroup",
                schema: "kenuser");
        }
    }
}
