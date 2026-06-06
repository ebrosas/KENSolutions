using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntityOTRequestWF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OTRequestWF",
                schema: "kenuser",
                columns: table => new
                {
                    ExtratimeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS_AutoId = table.Column<long>(type: "bigint", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false),
                    EmployeeName = table.Column<string>(type: "varchar(100)", nullable: false),
                    CostCenter = table.Column<string>(type: "varchar(20)", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    OTReasonCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    ActionCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    OTStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OTEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ShiftPattern = table.Column<string>(type: "varchar(20)", nullable: true),
                    ShiftTiming = table.Column<string>(type: "varchar(100)", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(500)", nullable: true),
                    StatusCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    StatusHandlingCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    WorkDuration = table.Column<int>(type: "int", nullable: true),
                    OTDuration = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_OTRequestWF_Id", x => x.ExtratimeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OTRequestWF_CompoKeys",
                schema: "kenuser",
                table: "OTRequestWF",
                columns: new[] { "TS_AutoId", "EmployeeNo", "AttendanceDate", "OTReasonCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTRequestWF",
                schema: "kenuser");
        }
    }
}
