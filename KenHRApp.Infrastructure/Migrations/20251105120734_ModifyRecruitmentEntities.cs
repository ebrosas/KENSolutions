using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRecruitmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecruitmentRequisition_CompoKeys",
                schema: "kenuser",
                table: "RecruitmentRequest");

            migrationBuilder.DropColumn(
                name: "EmployeeClass",
                schema: "kenuser",
                table: "RecruitmentRequest");

            migrationBuilder.DropColumn(
                name: "Qualification",
                schema: "kenuser",
                table: "JobQualification");

            migrationBuilder.DropColumn(
                name: "Specialization",
                schema: "kenuser",
                table: "JobQualification");

            migrationBuilder.DropColumn(
                name: "Stream",
                schema: "kenuser",
                table: "JobQualification");

            migrationBuilder.AlterColumn<string>(
                name: "VideoDescriptionURL",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SalaryRangeType",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequiredGender",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequiredAsset",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 11, 5, 15, 7, 33, 568, DateTimeKind.Local).AddTicks(9171),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 11, 3, 16, 53, 52, 329, DateTimeKind.Local).AddTicks(9171));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 11, 5, 15, 7, 33, 473, DateTimeKind.Local).AddTicks(4387),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 11, 3, 16, 53, 52, 328, DateTimeKind.Local).AddTicks(6625));

            migrationBuilder.AlterColumn<string>(
                name: "StreamCode",
                schema: "kenuser",
                table: "JobQualification",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SpecializationCode",
                schema: "kenuser",
                table: "JobQualification",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualificationCode",
                schema: "kenuser",
                table: "JobQualification",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 5, 12, 7, 33, 452, DateTimeKind.Utc).AddTicks(4258),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 11, 3, 13, 53, 52, 328, DateTimeKind.Utc).AddTicks(2908));

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentRequisition_CompoKeys",
                schema: "kenuser",
                table: "RecruitmentRequest",
                columns: new[] { "EmploymentTypeCode", "PositionTypeCode", "CompanyCode", "DepartmentCode", "EmployeeClassCode", "JobTitleCode", "PayGradeCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecruitmentRequisition_CompoKeys",
                schema: "kenuser",
                table: "RecruitmentRequest");

            migrationBuilder.AlterColumn<string>(
                name: "VideoDescriptionURL",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SalaryRangeType",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequiredGender",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequiredAsset",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 11, 3, 16, 53, 52, 329, DateTimeKind.Local).AddTicks(9171),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 11, 5, 15, 7, 33, 568, DateTimeKind.Local).AddTicks(9171));

            migrationBuilder.AddColumn<string>(
                name: "EmployeeClass",
                schema: "kenuser",
                table: "RecruitmentRequest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 11, 3, 16, 53, 52, 328, DateTimeKind.Local).AddTicks(6625),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 11, 5, 15, 7, 33, 473, DateTimeKind.Local).AddTicks(4387));

            migrationBuilder.AlterColumn<string>(
                name: "StreamCode",
                schema: "kenuser",
                table: "JobQualification",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "SpecializationCode",
                schema: "kenuser",
                table: "JobQualification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualificationCode",
                schema: "kenuser",
                table: "JobQualification",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                schema: "kenuser",
                table: "JobQualification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                schema: "kenuser",
                table: "JobQualification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stream",
                schema: "kenuser",
                table: "JobQualification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 3, 13, 53, 52, 328, DateTimeKind.Utc).AddTicks(2908),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 11, 5, 12, 7, 33, 452, DateTimeKind.Utc).AddTicks(4258));

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentRequisition_CompoKeys",
                schema: "kenuser",
                table: "RecruitmentRequest",
                columns: new[] { "EmploymentTypeCode", "PositionTypeCode", "CompanyCode", "DepartmentCode", "EmployeeClass", "JobTitleCode", "PayGradeCode" },
                unique: true);
        }
    }
}
