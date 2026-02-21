using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 787, DateTimeKind.Local).AddTicks(4694),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 280, DateTimeKind.Local).AddTicks(3079));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 786, DateTimeKind.Local).AddTicks(4491),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 277, DateTimeKind.Local).AddTicks(9487));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 794, DateTimeKind.Local).AddTicks(7848),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 297, DateTimeKind.Local).AddTicks(1042));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 790, DateTimeKind.Local).AddTicks(8212),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 285, DateTimeKind.Local).AddTicks(7589));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 794, DateTimeKind.Local).AddTicks(2027),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 295, DateTimeKind.Local).AddTicks(3599));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 794, DateTimeKind.Local).AddTicks(5149),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 296, DateTimeKind.Local).AddTicks(2583));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 793, DateTimeKind.Local).AddTicks(2999),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 292, DateTimeKind.Local).AddTicks(7281));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 21, 12, 55, 6, 786, DateTimeKind.Utc).AddTicks(1280),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 19, 12, 39, 52, 277, DateTimeKind.Utc).AddTicks(2507));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 793, DateTimeKind.Local).AddTicks(9879),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 294, DateTimeKind.Local).AddTicks(8363));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 793, DateTimeKind.Local).AddTicks(6618),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 293, DateTimeKind.Local).AddTicks(8310));

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                schema: "kenuser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportTicketAttachments",
                schema: "kenuser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "varchar(100)", nullable: false),
                    StoredFileName = table.Column<string>(type: "varchar(250)", nullable: false),
                    ContentType = table.Column<string>(type: "varchar(50)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTicketAttachments_SupportTickets_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalSchema: "kenuser",
                        principalTable: "SupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketAttachments_SupportTicketId",
                schema: "kenuser",
                table: "SupportTicketAttachments",
                column: "SupportTicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTicketAttachments",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "SupportTickets",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 280, DateTimeKind.Local).AddTicks(3079),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 787, DateTimeKind.Local).AddTicks(4694));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 277, DateTimeKind.Local).AddTicks(9487),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 786, DateTimeKind.Local).AddTicks(4491));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 297, DateTimeKind.Local).AddTicks(1042),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 794, DateTimeKind.Local).AddTicks(7848));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 285, DateTimeKind.Local).AddTicks(7589),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 790, DateTimeKind.Local).AddTicks(8212));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 295, DateTimeKind.Local).AddTicks(3599),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 794, DateTimeKind.Local).AddTicks(2027));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 296, DateTimeKind.Local).AddTicks(2583),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 794, DateTimeKind.Local).AddTicks(5149));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 292, DateTimeKind.Local).AddTicks(7281),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 793, DateTimeKind.Local).AddTicks(2999));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 19, 12, 39, 52, 277, DateTimeKind.Utc).AddTicks(2507),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 21, 12, 55, 6, 786, DateTimeKind.Utc).AddTicks(1280));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 294, DateTimeKind.Local).AddTicks(8363),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 793, DateTimeKind.Local).AddTicks(9879));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 19, 15, 39, 52, 293, DateTimeKind.Local).AddTicks(8310),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 21, 15, 55, 6, 793, DateTimeKind.Local).AddTicks(6618));
        }
    }
}
