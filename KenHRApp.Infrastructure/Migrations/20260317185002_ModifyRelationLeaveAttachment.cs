using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRelationLeaveAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveAttachments_LeaveRequisitionWF_LeaveRequestId",
                schema: "kenuser",
                table: "LeaveAttachments");

            migrationBuilder.DropIndex(
                name: "IX_LeaveAttachment_CompoKeys",
                schema: "kenuser",
                table: "LeaveAttachments");

            migrationBuilder.DropColumn(
                name: "LeaveRequestId",
                schema: "kenuser",
                table: "LeaveAttachments");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 190, DateTimeKind.Local).AddTicks(6655),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 824, DateTimeKind.Local).AddTicks(9069));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 187, DateTimeKind.Local).AddTicks(2641),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 823, DateTimeKind.Local).AddTicks(665));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 216, DateTimeKind.Local).AddTicks(3481),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 839, DateTimeKind.Local).AddTicks(1330));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 196, DateTimeKind.Local).AddTicks(8647),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 831, DateTimeKind.Local).AddTicks(3261));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 212, DateTimeKind.Local).AddTicks(5591),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 837, DateTimeKind.Local).AddTicks(7337));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 215, DateTimeKind.Local).AddTicks(7034),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 838, DateTimeKind.Local).AddTicks(6831));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 209, DateTimeKind.Local).AddTicks(1661),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(3014));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 3, 17, 18, 49, 59, 186, DateTimeKind.Utc).AddTicks(2617),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 3, 17, 18, 14, 2, 822, DateTimeKind.Utc).AddTicks(3902));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 210, DateTimeKind.Local).AddTicks(5704),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 836, DateTimeKind.Local).AddTicks(3511));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 209, DateTimeKind.Local).AddTicks(8579),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(8427));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_LeaveRequisitionWF_LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                column: "LeaveAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAttachments_LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveAttachments",
                column: "LeaveAttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveAttachments_LeaveRequisitionWF_LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveAttachments",
                column: "LeaveAttachmentId",
                principalSchema: "kenuser",
                principalTable: "LeaveRequisitionWF",
                principalColumn: "LeaveAttachmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveAttachments_LeaveRequisitionWF_LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveAttachments");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_LeaveRequisitionWF_LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveRequisitionWF");

            migrationBuilder.DropIndex(
                name: "IX_LeaveAttachments_LeaveAttachmentId",
                schema: "kenuser",
                table: "LeaveAttachments");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentRequest",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 824, DateTimeKind.Local).AddTicks(9069),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 190, DateTimeKind.Local).AddTicks(6655));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "RecruitmentBudget",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 823, DateTimeKind.Local).AddTicks(665),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 187, DateTimeKind.Local).AddTicks(2641));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "PayrollPeriod",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 839, DateTimeKind.Local).AddTicks(1330),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 216, DateTimeKind.Local).AddTicks(3481));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "MasterShiftPatternTitle",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 831, DateTimeKind.Local).AddTicks(3261),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 196, DateTimeKind.Local).AddTicks(8647));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveCreatedDate",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 837, DateTimeKind.Local).AddTicks(7337),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 212, DateTimeKind.Local).AddTicks(5591));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "LeaveEntitlement",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 838, DateTimeKind.Local).AddTicks(6831),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 215, DateTimeKind.Local).AddTicks(7034));

            migrationBuilder.AddColumn<long>(
                name: "LeaveRequestId",
                schema: "kenuser",
                table: "LeaveAttachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Foreign key that references primary key: LeaveRequisitionWF.LeaveRequestId");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "Holiday",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(3014),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 209, DateTimeKind.Local).AddTicks(1661));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    schema: "kenuser",
            //    table: "DepartmentMaster",
            //    type: "datetime",
            //    nullable: false,
            //    defaultValue: new DateTime(2026, 3, 17, 18, 14, 2, 822, DateTimeKind.Utc).AddTicks(3902),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldDefaultValue: new DateTime(2026, 3, 17, 18, 49, 59, 186, DateTimeKind.Utc).AddTicks(2617));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedDate",
            //    schema: "kenuser",
            //    table: "AttendanceTimesheet",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 836, DateTimeKind.Local).AddTicks(3511),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 210, DateTimeKind.Local).AddTicks(5704));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "SwipeLogDate",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "datetime",
            //    nullable: true,
            //    defaultValue: new DateTime(2026, 3, 17, 21, 14, 2, 835, DateTimeKind.Local).AddTicks(8427),
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime",
            //    oldNullable: true,
            //    oldDefaultValue: new DateTime(2026, 3, 17, 21, 49, 59, 209, DateTimeKind.Local).AddTicks(8579));

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAttachment_CompoKeys",
                schema: "kenuser",
                table: "LeaveAttachments",
                columns: new[] { "LeaveRequestId", "FileName", "ContentType" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveAttachments_LeaveRequisitionWF_LeaveRequestId",
                schema: "kenuser",
                table: "LeaveAttachments",
                column: "LeaveRequestId",
                principalSchema: "kenuser",
                principalTable: "LeaveRequisitionWF",
                principalColumn: "LeaveRequestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
