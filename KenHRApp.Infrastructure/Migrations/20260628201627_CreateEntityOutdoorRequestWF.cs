using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntityOutdoorRequestWF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutdoorRequestWF",
                schema: "kenuser",
                columns: table => new
                {
                    OutdoorId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpNo = table.Column<int>(type: "int", nullable: false),
                    EmpName = table.Column<string>(type: "varchar(100)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ROACode = table.Column<string>(type: "varchar(20)", nullable: false),
                    DOWCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", nullable: false),
                    ActionCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    StatusHandlingCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    CostCenter = table.Column<string>(type: "varchar(20)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedEmail = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutdoorRequestWF_Id", x => x.OutdoorId);
                    table.UniqueConstraint("AK_OutdoorRequestWF_AttachmentId", x => x.AttachmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutdoorRequestWF_CompoKeys",
                schema: "kenuser",
                table: "OutdoorRequestWF",
                columns: new[] { "EmpNo", "StartDate", "EndDate", "ROACode", "ActionCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_OutdoorRequestWF_AttachmentId",
                schema: "kenuser",
                table: "FileAttachments",
                column: "AttachmentId",
                principalSchema: "kenuser",
                principalTable: "OutdoorRequestWF",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_OutdoorRequestWF_AttachmentId",
                schema: "kenuser",
                table: "FileAttachments");

            migrationBuilder.DropTable(
                name: "OutdoorRequestWF",
                schema: "kenuser");
        }
    }
}
