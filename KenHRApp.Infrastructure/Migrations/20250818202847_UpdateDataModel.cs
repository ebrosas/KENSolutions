using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KenHRApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PrimaryKey_EmpNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                schema: "kenuser",
                table: "Employee",
                type: "int",
                nullable: false,
                comment: "Primary key for Employee entity",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "The primary key of the Employee entity")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "AccountHolderName",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountTypeCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankBranchCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankNameCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfSuperannuation",
                schema: "kenuser",
                table: "Employee",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeClassCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeNo",
                schema: "kenuser",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Alternate key used for reference navigation");

            migrationBuilder.AddColumn<string>(
                name: "FacebookAccount",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBANNumber",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramAccount",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInAccount",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldEmployeeNo",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PayGrade",
                schema: "kenuser",
                table: "Employee",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAreaCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentCityCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentContactNo",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentCountryCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentMobileNo",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentAddress",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentAreaCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentCityCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentContactNo",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentCountryCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentMobileNo",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reemployed",
                schema: "kenuser",
                table: "Employee",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterAccount",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(40)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee_EmployeeId",
                schema: "kenuser",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.CreateTable(
                name: "EmergencyContact",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactPerson = table.Column<string>(type: "varchar(200)", nullable: false),
                    RelationCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    LandlineNo = table.Column<string>(type: "varchar(20)", nullable: true),
                    Address = table.Column<string>(type: "varchar(300)", nullable: true),
                    CountryCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    CityCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContact_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_EmergencyContact_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCertification",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QualificationCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of unique composite key index"),
                    Stream = table.Column<string>(type: "varchar(100)", nullable: true),
                    Specialization = table.Column<string>(type: "varchar(150)", nullable: false),
                    University = table.Column<string>(type: "varchar(150)", nullable: false),
                    Institute = table.Column<string>(type: "varchar(100)", nullable: true),
                    CountryCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    StateCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    CityCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    FromMonthCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of unique composite key index"),
                    FromYear = table.Column<int>(type: "int", nullable: false, comment: "Part of unique composite key index"),
                    ToMonthCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of unique composite key index"),
                    ToYear = table.Column<int>(type: "int", nullable: false, comment: "Part of unique composite key index"),
                    PassMonthCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    PassYear = table.Column<int>(type: "int", nullable: true),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references Employee.EmployeeNo alternate key")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCertification_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_EmployeeCertification_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkill",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LevelCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    LastUsedMonthCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    LastUsedYear = table.Column<int>(type: "int", nullable: true),
                    FromMonthCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    FromYear = table.Column<int>(type: "int", nullable: true),
                    ToMonthCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    ToYear = table.Column<int>(type: "int", nullable: true),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkill_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_EmployeeSkill_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTransaction",
                schema: "kenuser",
                columns: table => new
                {
                    ActionCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    SectionCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    TransactionNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references employee transaction. Part of composite key."),
                    AutoId = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    LastUpdateOn = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Part of composite unique key index"),
                    CurrentlyAssignedEmpNo = table.Column<int>(type: "int", nullable: true),
                    CurrentlyAssignedEmpName = table.Column<string>(type: "varchar(150)", nullable: true),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTransaction_CompKey", x => new { x.ActionCode, x.SectionCode, x.TransactionNo });
                    table.ForeignKey(
                        name: "FK_EmployeeTransaction_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentHistory",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "varchar(150)", nullable: false, comment: "Part of composite unique key index"),
                    CompanyAddress = table.Column<string>(type: "varchar(300)", nullable: true),
                    Designation = table.Column<string>(type: "varchar(100)", nullable: false, comment: "Part of composite unique key index"),
                    Role = table.Column<string>(type: "varchar(100)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Part of composite unique key index"),
                    ToDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Part of composite unique key index"),
                    LastDrawnSalary = table.Column<string>(type: "nvarchar(max)", precision: 14, scale: 3, nullable: true),
                    SalaryTypeCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    SalaryCurrencyCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    ReasonOfChange = table.Column<string>(type: "varchar(200)", nullable: true),
                    ReportingManager = table.Column<string>(type: "varchar(150)", nullable: true),
                    CompanyWebsite = table.Column<string>(type: "varchar(100)", nullable: true),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentHistory_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_EmploymentHistory_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMember",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false, comment: "Part of composite unique key index"),
                    MiddleName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false, comment: "Part of composite unique key index"),
                    RelationCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    DOB = table.Column<DateTime>(type: "datetime", nullable: true),
                    QualificationCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    StreamCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    SpecializationCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    Occupation = table.Column<string>(type: "varchar(120)", nullable: true),
                    ContactNo = table.Column<string>(type: "varchar(20)", nullable: true),
                    CountryCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    StateCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    CityCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    District = table.Column<string>(type: "varchar(100)", nullable: true),
                    IsDependent = table.Column<bool>(type: "bit", nullable: true),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMember_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_FamilyMember_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyVisa",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    VisaTypeCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    Profession = table.Column<string>(type: "varchar(150)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Part of composite unique key index"),
                    ExpiryDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Part of composite unique key index"),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyVisa_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_FamilyVisa_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityProof",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassportNumber = table.Column<string>(type: "varchar(20)", nullable: true),
                    DateOfIssue = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateOfExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    PlaceOfIssue = table.Column<string>(type: "varchar(100)", nullable: true),
                    DrivingLicenseNo = table.Column<string>(type: "varchar(20)", nullable: true),
                    DLDateOfIssue = table.Column<DateTime>(type: "datetime", nullable: true),
                    DLDateOfExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    DLPlaceOfIssue = table.Column<string>(type: "varchar(50)", nullable: true),
                    TypeOfDocument = table.Column<string>(type: "varchar(50)", nullable: true),
                    OtherDocNumber = table.Column<string>(type: "varchar(30)", nullable: true),
                    OtherDocDateOfIssue = table.Column<DateTime>(type: "datetime", nullable: true),
                    OtherDocDateOfExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    NationalIDNumber = table.Column<string>(type: "varchar(40)", nullable: true),
                    NationalIDTypeCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    NatIDPlaceOfIssue = table.Column<string>(type: "varchar(50)", nullable: true),
                    NatIDDateOfIssue = table.Column<DateTime>(type: "datetime", nullable: true),
                    NatIDDateOfExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    ContractNumber = table.Column<string>(type: "varchar(30)", nullable: true),
                    ContractPlaceOfIssue = table.Column<string>(type: "varchar(100)", nullable: true),
                    ContractDateOfIssue = table.Column<DateTime>(type: "datetime", nullable: true),
                    ContractDateOfExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    VisaNumber = table.Column<string>(type: "varchar(30)", nullable: true),
                    VisaTypeCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    VisaCountryCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    Profession = table.Column<string>(type: "varchar(100)", nullable: true),
                    Sponsor = table.Column<string>(type: "varchar(100)", nullable: true),
                    VisaDateOfIssue = table.Column<DateTime>(type: "datetime", nullable: true),
                    VisaDateOfExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo"),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityProof_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_IdentityProof_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageSkill",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    CanWrite = table.Column<bool>(type: "bit", nullable: true),
                    CanSpeak = table.Column<bool>(type: "bit", nullable: true),
                    CanRead = table.Column<bool>(type: "bit", nullable: true),
                    MotherTongue = table.Column<bool>(type: "bit", nullable: true),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSkill_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_LanguageSkill_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherDocument",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "varchar(150)", nullable: false, comment: "Part of composite unique key index"),
                    DocumentTypeCode = table.Column<string>(type: "varchar(20)", nullable: false, comment: "Part of composite unique key index"),
                    Description = table.Column<string>(type: "varchar(300)", nullable: true),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval"),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherDocument_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_OtherDocument_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Qualification",
                schema: "kenuser",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QualificationCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    StreamCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    SpecializationCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    UniversityName = table.Column<string>(type: "varchar(200)", nullable: false),
                    Institute = table.Column<string>(type: "varchar(100)", nullable: true),
                    QualificationMode = table.Column<string>(type: "varchar(20)", nullable: false),
                    CountryCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    StateCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    CityCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    FromMonthCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    FromYear = table.Column<int>(type: "int", nullable: false),
                    ToMonthCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    ToYear = table.Column<int>(type: "int", nullable: false),
                    PassMonthCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    PassYear = table.Column<int>(type: "int", nullable: false),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false, comment: "Foreign key that references alternate key: Employee.EmployeeNo"),
                    TransactionNo = table.Column<int>(type: "int", nullable: true, comment: "Unique ID number that is generated when a request requires approval")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualification_AutoId", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_Qualification_Employee_EmployeeNo",
                        column: x => x.EmployeeNo,
                        principalSchema: "kenuser",
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Attribute",
                schema: "kenuser",
                table: "Employee",
                columns: new[] { "EmployeeNo", "NationalityCode", "ReligionCode", "MaritalStatusCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Date",
                schema: "kenuser",
                table: "Employee",
                columns: new[] { "EmployeeNo", "HireDate", "TerminationDate", "DateOfConfirmation", "DOB" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmpName",
                schema: "kenuser",
                table: "Employee",
                columns: new[] { "EmployeeNo", "FirstName", "MiddleName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContact_CompoKeys",
                schema: "kenuser",
                table: "EmergencyContact",
                columns: new[] { "EmployeeNo", "ContactPerson", "RelationCode", "MobileNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertification_CompoKeys",
                schema: "kenuser",
                table: "EmployeeCertification",
                columns: new[] { "EmployeeNo", "QualificationCode", "FromMonthCode", "FromYear", "ToMonthCode", "ToYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkill_CompoKeys",
                schema: "kenuser",
                table: "EmployeeSkill",
                columns: new[] { "EmployeeNo", "SkillName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTransaction_CompoKeys",
                schema: "kenuser",
                table: "EmployeeTransaction",
                columns: new[] { "EmployeeNo", "ActionCode", "SectionCode", "StatusCode", "LastUpdateOn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentHistory_CompoKeys",
                schema: "kenuser",
                table: "EmploymentHistory",
                columns: new[] { "EmployeeNo", "CompanyName", "Designation", "FromDate", "ToDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMember_CompoKeys",
                schema: "kenuser",
                table: "FamilyMember",
                columns: new[] { "EmployeeNo", "FirstName", "LastName", "RelationCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyVisa_CompoKeys",
                schema: "kenuser",
                table: "FamilyVisa",
                columns: new[] { "EmployeeNo", "VisaTypeCode", "CountryCode", "IssueDate", "ExpiryDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityProof_ContractDetail",
                schema: "kenuser",
                table: "IdentityProof",
                columns: new[] { "EmployeeNo", "ContractNumber", "ContractDateOfIssue", "ContractDateOfExpiry" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityProof_EmployeeNo",
                schema: "kenuser",
                table: "IdentityProof",
                column: "EmployeeNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityProof_NatlIDDetail",
                schema: "kenuser",
                table: "IdentityProof",
                columns: new[] { "EmployeeNo", "NationalIDNumber", "NationalIDTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityProof_PassportInfo",
                schema: "kenuser",
                table: "IdentityProof",
                columns: new[] { "EmployeeNo", "PassportNumber", "DateOfIssue", "DateOfExpiry" },
                unique: true,
                filter: "[PassportNumber] IS NOT NULL AND [DateOfIssue] IS NOT NULL AND [DateOfExpiry] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityProof_VisaDetail",
                schema: "kenuser",
                table: "IdentityProof",
                columns: new[] { "EmployeeNo", "VisaCountryCode", "VisaNumber", "VisaTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSkill_CompoKeys",
                schema: "kenuser",
                table: "LanguageSkill",
                columns: new[] { "EmployeeNo", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherDocument_CompoKeys",
                schema: "kenuser",
                table: "OtherDocument",
                columns: new[] { "EmployeeNo", "DocumentName", "DocumentTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_CompoKeys",
                schema: "kenuser",
                table: "Qualification",
                columns: new[] { "EmployeeNo", "QualificationCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmergencyContact",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "EmployeeCertification",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "EmployeeSkill",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "EmployeeTransaction",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "EmploymentHistory",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "FamilyMember",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "FamilyVisa",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "IdentityProof",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "LanguageSkill",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "OtherDocument",
                schema: "kenuser");

            migrationBuilder.DropTable(
                name: "Qualification",
                schema: "kenuser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee_EmployeeId",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_Attribute",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_Date",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EmpName",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "AccountHolderName",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "AccountTypeCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BankBranchCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BankNameCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Company",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DateOfSuperannuation",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EducationCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeClassCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "FacebookAccount",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IBANNumber",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "InstagramAccount",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LinkedInAccount",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "OldEmployeeNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PayGrade",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PermanentAreaCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PermanentCityCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PermanentContactNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PermanentCountryCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PermanentMobileNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentAddress",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentAreaCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentCityCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentContactNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentCountryCode",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentMobileNo",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Reemployed",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TwitterAccount",
                schema: "kenuser",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                schema: "kenuser",
                table: "Employee",
                type: "int",
                nullable: false,
                comment: "The primary key of the Employee entity",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary key for Employee entity")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                schema: "kenuser",
                table: "Employee",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                comment: "The unique ID of the employee");

            migrationBuilder.AddPrimaryKey(
                name: "PrimaryKey_EmpNo",
                schema: "kenuser",
                table: "Employee",
                column: "EmployeeId");
        }
    }
}
