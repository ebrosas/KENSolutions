using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataTypeLeaveMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
               name: "StartDayMode",
               schema: "kenuser",
               table: "LeaveRequisitionWF",
               type: "varchar(20)",
               nullable: true,
               oldClrType: typeof(byte),
               oldType: "tinyint",
               oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "EndDayMode",
                schema: "kenuser",
                table: "LeaveRequisitionWF",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "StartDayMode",
              schema: "kenuser",
              table: "LeaveRequisitionWF",
              type: "tinyint",
              nullable: true,
              oldClrType: typeof(string),
              oldType: "varchar(20)",
              oldNullable: true);

            migrationBuilder.AlterColumn<string>(
              name: "EndDayMode",
              schema: "kenuser",
              table: "LeaveRequisitionWF",
              type: "tinyint",
              nullable: true,
              oldClrType: typeof(string),
              oldType: "varchar(20)",
              oldNullable: true);
        }
    }
}
