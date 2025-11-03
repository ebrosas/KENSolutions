using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruitmentRequisition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2025, 10, 13, 12, 4, 16, 963, DateTimeKind.Local).AddTicks(507));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 11, 3, 13, 53, 52, 328, DateTimeKind.Utc).AddTicks(2908),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 10, 13, 9, 4, 16, 962, DateTimeKind.Utc).AddTicks(7736));

            migrationBuilder.CreateTable(
                name: "RecruitmentRequest",
                schema: "kenuser",
                columns: table => new
                {
                    RequisitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmploymentTypeCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    QualificationModeCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    PositionTypeCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    InterviewProcessCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    IsPreAssessment = table.Column<bool>(type: "bit", nullable: true),
                    CompanyCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    DepartmentCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    CountryCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    EducationCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployeeClassCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    EmployeeClass = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EthnicityCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    JobTitleCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    PayGradeCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    PositionDescription = table.Column<string>(type: "varchar(1000)", nullable: false),
                    TotalWorkExperience = table.Column<int>(type: "int", nullable: false),
                    MinWorkExperience = table.Column<int>(type: "int", nullable: true),
                    MaxWorkExperience = table.Column<int>(type: "int", nullable: true),
                    TotalRelevantExperience = table.Column<int>(type: "int", nullable: false),
                    MinRelevantExperience = table.Column<int>(type: "int", nullable: true),
                    MaxRelevantExperience = table.Column<int>(type: "int", nullable: true),
                    AgeRange = table.Column<int>(type: "int", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    RequiredGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredAsset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoDescriptionURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryRangeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearlySalaryRange = table.Column<int>(type: "int", nullable: true),
                    YearlySalaryRangeMin = table.Column<int>(type: "int", nullable: true),
                    YearlySalaryRangeMax = table.Column<int>(type: "int", nullable: true),
                    YearlySalaryRangeCurrency = table.Column<string>(type: "varchar(20)", nullable: true),
                    MonthlySalaryRange = table.Column<int>(type: "int", nullable: true),
                    MonthlySalaryRangeMin = table.Column<int>(type: "int", nullable: true),
                    MonthlySalaryRangeMax = table.Column<int>(type: "int", nullable: true),
                    MonthlySalaryRangeCurrency = table.Column<string>(type: "varchar(20)", nullable: true),
                    DailySalaryRange = table.Column<int>(type: "int", nullable: true),
                    DailySalaryRangeMin = table.Column<int>(type: "int", nullable: true),
                    DailySalaryRangeMax = table.Column<int>(type: "int", nullable: true),
                    DailySalaryRangeCurrency = table.Column<string>(type: "varchar(20)", nullable: true),
                    HourlySalaryRange = table.Column<int>(type: "int", nullable: true),
                    HourlySalaryRangeMin = table.Column<int>(type: "int", nullable: true),
                    HourlySalaryRangeMax = table.Column<int>(type: "int", nullable: true),
                    HourlySalaryRangeCurrency = table.Column<string>(type: "varchar(20)", nullable: true),
                    Responsibilities = table.Column<string>(type: "varchar(5000)", nullable: false),
                    Competencies = table.Column<string>(type: "varchar(5000)", nullable: false),
                    GeneralRemarks = table.Column<string>(type: "varchar(5000)", nullable: true),
                    CreatedByNo = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedByName = table.Column<string>(type: "varchar(100)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValue: new DateTime(2025, 11, 3, 16, 53, 52, 329, DateTimeKind.Local).AddTicks(9171)),
                    LastUpdatedByNo = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedUserID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastUpdatedName = table.Column<string>(type: "varchar(100)", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentRequest_RequisitionId", x => x.RequisitionId);
                });

            migrationBuilder.CreateTable(
                name: "JobQualification",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QualificationCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreamCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Stream = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecializationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(500)", nullable: true),
                    RequisitionId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references primary key: RecruitmentRequest.RequisitionId")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQualification_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_JobQualification_RecruitmentRequest_RequisitionId",
                        column: x => x.RequisitionId,
                        principalSchema: "kenuser",
                        principalTable: "RecruitmentRequest",
                        principalColumn: "RequisitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobQualification_CompoKeys",
                schema: "kenuser",
                table: "JobQualification",
                columns: new[] { "RequisitionId", "QualificationCode", "StreamCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentRequisition_CompoKeys",
                schema: "kenuser",
                table: "RecruitmentRequest",
                columns: new[] { "EmploymentTypeCode", "PositionTypeCode", "CompanyCode", "DepartmentCode", "EmployeeClass", "JobTitleCode", "PayGradeCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobQualification",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "RecruitmentRequest",
                schema: "kenuser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "kenuser",
                table: "RecruitmentBudget",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(2025, 10, 13, 12, 4, 16, 963, DateTimeKind.Local).AddTicks(507),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 11, 3, 16, 53, 52, 328, DateTimeKind.Local).AddTicks(6625));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "kenuser",
                table: "DepartmentMaster",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 13, 9, 4, 16, 962, DateTimeKind.Utc).AddTicks(7736),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2025, 11, 3, 13, 53, 52, 328, DateTimeKind.Utc).AddTicks(2908));
        }
    }
}
