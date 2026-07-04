using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntityPlannedLeaveRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlannedLeaveRequest",
                schema: "kenuser",
                columns: table => new
                {
                    PlannedLeaveId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpNo = table.Column<int>(type: "int", nullable: false),
                    EmpName = table.Column<string>(type: "varchar(150)", nullable: true),
                    LeaveStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LeaveEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LeaveResumeDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CostCenter = table.Column<string>(type: "varchar(20)", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(500)", nullable: true),
                    StartDayMode = table.Column<string>(type: "varchar(20)", nullable: true),
                    EndDayMode = table.Column<string>(type: "varchar(20)", nullable: true),
                    LeaveDuration = table.Column<double>(type: "float", nullable: true),
                    NoOfHolidays = table.Column<int>(type: "int", nullable: true),
                    NoOfWeekends = table.Column<int>(type: "int", nullable: true),
                    HalfDayLeaveFlag = table.Column<string>(type: "char(1)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedByName = table.Column<string>(type: "varchar(100)", nullable: true),
                    CreatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedName = table.Column<string>(type: "varchar(100)", nullable: true),
                    LastUpdatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedEmail = table.Column<string>(type: "varchar(50)", nullable: true),
                    StatusCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    StatusHandlingCode = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedLeaveRequest_Id", x => x.PlannedLeaveId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlannedLeaveRequest_CompoKeys",
                schema: "kenuser",
                table: "PlannedLeaveRequest",
                columns: new[] { "EmpNo", "LeaveStartDate", "LeaveResumeDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedLeaveRequest",
                schema: "kenuser");
        }
    }
}
