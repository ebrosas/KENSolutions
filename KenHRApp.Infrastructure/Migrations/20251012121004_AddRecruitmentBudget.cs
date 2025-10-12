using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruitmentBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 12, 12, 10, 2, 832, DateTimeKind.Utc).AddTicks(5074),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 9, 29, 20, 4, 24, 423, DateTimeKind.Utc).AddTicks(9460));

            migrationBuilder.CreateTable(
                name: "RecruitmentBudget",
                schema: "kenuser",
                columns: table => new
                {
                    BudgetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    BudgetHeadCount = table.Column<int>(type: "int", nullable: false),
                    ActiveCount = table.Column<int>(type: "int", nullable: false),
                    ExitCount = table.Column<int>(type: "int", nullable: false),
                    RequisitionCount = table.Column<int>(type: "int", nullable: false),
                    NetGapCount = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(300)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2025, 10, 12, 15, 10, 2, 833, DateTimeKind.Local).AddTicks(4636)),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentBudget_BudgetId", x => x.BudgetId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentBudget_CompoKeys",
                schema: "kenuser",
                table: "RecruitmentBudget",
                columns: new[] { "DepartmentCode", "BudgetHeadCount" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecruitmentBudget",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 9, 29, 20, 4, 24, 423, DateTimeKind.Utc).AddTicks(9460),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 10, 12, 12, 10, 2, 832, DateTimeKind.Utc).AddTicks(5074));
        }
    }
}
