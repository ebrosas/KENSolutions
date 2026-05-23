using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegularRequestWF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegularRequestWFs",
                schema: "kenuser",
                columns: table => new
                {
                    RegularizationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false),
                    EmployeeName = table.Column<string>(type: "varchar(100)", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ROACode = table.Column<string>(type: "varchar(20)", nullable: false),
                    ActionCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    RegularizedTimeIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    RegularizedTimeOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    ShiftPattern = table.Column<string>(type: "varchar(20)", nullable: true),
                    RegularizedDescription = table.Column<string>(type: "varchar(500)", nullable: false),
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
                    table.PrimaryKey("PK_RegularRequestWF_Id", x => x.RegularizationId);
                    table.UniqueConstraint("AK_RegularRequestWFs_AttachmentId", x => x.AttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "FileAttachments",
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
                    table.PrimaryKey("PK_FileAttachment_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileAttachments_RegularRequestWFs_AttachmentId",
                        column: x => x.AttachmentId,
                        principalSchema: "kenuser",
                        principalTable: "RegularRequestWFs",
                        principalColumn: "AttachmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_AttachmentId",
                schema: "kenuser",
                table: "FileAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RegularRequestWF_CompoKeys",
                schema: "kenuser",
                table: "RegularRequestWFs",
                columns: new[] { "EmployeeNo", "AttendanceDate", "ROACode", "ActionCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAttachments",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "RegularRequestWFs",
                schema: "kenuser");
        }
    }
}
