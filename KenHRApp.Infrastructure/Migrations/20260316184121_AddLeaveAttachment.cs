using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveAttachment : Migration
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
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 694, DateTimeKind.Local).AddTicks(9216),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 253, DateTimeKind.Local).AddTicks(3902));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 692, DateTimeKind.Local).AddTicks(9933),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 251, DateTimeKind.Local).AddTicks(9861));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 706, DateTimeKind.Local).AddTicks(1311),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 262, DateTimeKind.Local).AddTicks(5533));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 698, DateTimeKind.Local).AddTicks(6761),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 256, DateTimeKind.Local).AddTicks(6989));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 704, DateTimeKind.Local).AddTicks(7477),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 261, DateTimeKind.Local).AddTicks(7202));

            migrationBuilder.AddColumn<byte>(
                name: "EndDayMode",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaveStatusID",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "StartDayMode",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusHandlingCode",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowId",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 705, DateTimeKind.Local).AddTicks(5958),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 262, DateTimeKind.Local).AddTicks(1542));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 702, DateTimeKind.Local).AddTicks(5117),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 260, DateTimeKind.Local).AddTicks(5238));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 16, 18, 41, 19, 692, DateTimeKind.Utc).AddTicks(5475),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 2, 27, 12, 26, 28, 251, DateTimeKind.Utc).AddTicks(5693));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(5257),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 261, DateTimeKind.Local).AddTicks(4237));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(369),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 260, DateTimeKind.Local).AddTicks(9771));

            migrationBuilder.CreateTable(
                name: "LeaveAttachments",
                schema: "kenuser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StoredFileName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ContentType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LeaveRequestId = table.Column<long>(type: "bigint", nullable: false, comment: "Foreign key that references primary key: LeaveRequisitionWF.LeaveRequestId")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveAttachment_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveAttachments_LeaveRequisitionWF_LeaveRequestId",
                        column: x => x.LeaveRequestId,
                        principalSchema: "kenuser",
                        principalTable: "LeaveRequisitionWF",
                        principalColumn: "LeaveRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAttachment_CompoKeys",
                schema: "kenuser",
                table: "LeaveAttachments",
                columns: new[] { "LeaveRequestId", "FileName", "ContentType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveAttachments",
                schema: "kenuser");

            migrationBuilder.DropColumn(
                name: "EndDayMode",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.DropColumn(
                name: "LeaveStatusID",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.DropColumn(
                name: "StartDayMode",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.DropColumn(
                name: "StatusHandlingCode",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 253, DateTimeKind.Local).AddTicks(3902),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 694, DateTimeKind.Local).AddTicks(9216));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 251, DateTimeKind.Local).AddTicks(9861),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 692, DateTimeKind.Local).AddTicks(9933));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "PayrollPeriod",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 262, DateTimeKind.Local).AddTicks(5533),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 706, DateTimeKind.Local).AddTicks(1311));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 256, DateTimeKind.Local).AddTicks(6989),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 698, DateTimeKind.Local).AddTicks(6761));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 261, DateTimeKind.Local).AddTicks(7202),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 704, DateTimeKind.Local).AddTicks(7477));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "LeaveEntitlement",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 262, DateTimeKind.Local).AddTicks(1542),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 705, DateTimeKind.Local).AddTicks(5958));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 260, DateTimeKind.Local).AddTicks(5238),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 702, DateTimeKind.Local).AddTicks(5117));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 27, 12, 26, 28, 251, DateTimeKind.Utc).AddTicks(5693),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 3, 16, 18, 41, 19, 692, DateTimeKind.Utc).AddTicks(5475));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 261, DateTimeKind.Local).AddTicks(4237),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(5257));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 2, 27, 15, 26, 28, 260, DateTimeKind.Local).AddTicks(9771),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 16, 21, 41, 19, 703, DateTimeKind.Local).AddTicks(369));
        }
    }
}
