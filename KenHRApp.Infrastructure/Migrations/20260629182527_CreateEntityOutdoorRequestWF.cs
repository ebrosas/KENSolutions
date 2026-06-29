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

            migrationBuilder.CreateTable(
                name: "OutdoorAttachment",
                schema: "kenuser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    FileName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StoredFileName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ContentType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutdoorAttachment_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutdoorAttachment_OutdoorRequestWF_AttachmentId",
                        column: x => x.AttachmentId,
                        principalSchema: "kenuser",
                        principalTable: "OutdoorRequestWF",
                        principalColumn: "AttachmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutdoorAttachment_AttachmentId",
                schema: "kenuser",
                table: "OutdoorAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutdoorRequestWF_CompoKeys",
                schema: "kenuser",
                table: "OutdoorRequestWF",
                columns: new[] { "EmpNo", "StartDate", "EndDate", "ROACode", "ActionCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutdoorAttachment",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "OutdoorRequestWF",
                schema: "kenuser");
        }
    }
}
