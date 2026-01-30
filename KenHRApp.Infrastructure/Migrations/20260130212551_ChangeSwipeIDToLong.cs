using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSwipeIDToLong : Migration
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
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 933, DateTimeKind.Local).AddTicks(1384),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 385, DateTimeKind.Local).AddTicks(5180));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 929, DateTimeKind.Local).AddTicks(9744),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 378, DateTimeKind.Local).AddTicks(602));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 941, DateTimeKind.Local).AddTicks(346),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 393, DateTimeKind.Local).AddTicks(8558));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 948, DateTimeKind.Local).AddTicks(9757),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 400, DateTimeKind.Local).AddTicks(6934));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 30, 21, 25, 47, 928, DateTimeKind.Utc).AddTicks(9368),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 30, 20, 52, 42, 377, DateTimeKind.Utc).AddTicks(2311));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 950, DateTimeKind.Local).AddTicks(8463),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 402, DateTimeKind.Local).AddTicks(4645));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 949, DateTimeKind.Local).AddTicks(9514),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 401, DateTimeKind.Local).AddTicks(5868));

            //migrationBuilder.AlterColumn<long>(
            //    name: "SwipeID",
            //    schema: "kenuser",
            //    table: "AttendanceSwipeLog",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(double),
            //    oldType: "float")
            //    .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.Sql(@"
                ALTER TABLE kenuser.AttendanceSwipeLog DROP CONSTRAINT PK_AttendanceSwipeLog_SwipeID;

                ALTER TABLE kenuser.AttendanceSwipeLog DROP COLUMN SwipeID;

                ALTER TABLE kenuser.AttendanceSwipeLog ADD SwipeID BIGINT IDENTITY(1,1) NOT NULL;

                ALTER TABLE kenuser.AttendanceSwipeLog
                ADD CONSTRAINT PK_AttendanceSwipeLog_SwipeID PRIMARY KEY (SwipeID);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 385, DateTimeKind.Local).AddTicks(5180),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 933, DateTimeKind.Local).AddTicks(1384));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 378, DateTimeKind.Local).AddTicks(602),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 929, DateTimeKind.Local).AddTicks(9744));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "MasterShiftPatternTitle",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 393, DateTimeKind.Local).AddTicks(8558),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 941, DateTimeKind.Local).AddTicks(346));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "Holiday",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 400, DateTimeKind.Local).AddTicks(6934),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 948, DateTimeKind.Local).AddTicks(9757));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 30, 20, 52, 42, 377, DateTimeKind.Utc).AddTicks(2311),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2026, 1, 30, 21, 25, 47, 928, DateTimeKind.Utc).AddTicks(9368));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "AttendanceTimesheet",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 402, DateTimeKind.Local).AddTicks(4645),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 950, DateTimeKind.Local).AddTicks(8463));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SwipeLogDate",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2026, 1, 30, 23, 52, 42, 401, DateTimeKind.Local).AddTicks(5868),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2026, 1, 31, 0, 25, 47, 949, DateTimeKind.Local).AddTicks(9514));

            migrationBuilder.AlterColumn<double>(
                name: "SwipeID",
                schema: "kenuser",
                table: "AttendanceSwipeLog",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
