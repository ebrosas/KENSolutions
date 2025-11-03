BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2025-11-03T16:53:52.328' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-11-03T13:53:52.328' FOR [CreatedAt];
GO

CREATE TABLE [kenuser].[RecruitmentRequest] (
    [RequisitionId] int NOT NULL IDENTITY,
    [EmploymentTypeCode] varchar(20) NOT NULL,
    [QualificationModeCode] varchar(20) NOT NULL,
    [PositionTypeCode] varchar(20) NOT NULL,
    [InterviewProcessCode] varchar(20) NOT NULL,
    [IsPreAssessment] bit NULL,
    [CompanyCode] varchar(20) NOT NULL,
    [DepartmentCode] varchar(20) NOT NULL,
    [CountryCode] varchar(20) NULL,
    [EducationCode] varchar(20) NULL,
    [EmployeeClassCode] varchar(20) NOT NULL,
    [EmployeeClass] nvarchar(450) NOT NULL,
    [EthnicityCode] varchar(20) NULL,
    [JobTitleCode] varchar(20) NOT NULL,
    [PayGradeCode] varchar(20) NOT NULL,
    [PositionDescription] varchar(1000) NOT NULL,
    [TotalWorkExperience] int NOT NULL,
    [MinWorkExperience] int NULL,
    [MaxWorkExperience] int NULL,
    [TotalRelevantExperience] int NOT NULL,
    [MinRelevantExperience] int NULL,
    [MaxRelevantExperience] int NULL,
    [AgeRange] int NOT NULL,
    [MinAge] int NULL,
    [MaxAge] int NULL,
    [RequiredGender] nvarchar(max) NULL,
    [RequiredAsset] nvarchar(max) NULL,
    [VideoDescriptionURL] nvarchar(max) NULL,
    [SalaryRangeType] nvarchar(max) NULL,
    [YearlySalaryRange] int NULL,
    [YearlySalaryRangeMin] int NULL,
    [YearlySalaryRangeMax] int NULL,
    [YearlySalaryRangeCurrency] varchar(20) NULL,
    [MonthlySalaryRange] int NULL,
    [MonthlySalaryRangeMin] int NULL,
    [MonthlySalaryRangeMax] int NULL,
    [MonthlySalaryRangeCurrency] varchar(20) NULL,
    [DailySalaryRange] int NULL,
    [DailySalaryRangeMin] int NULL,
    [DailySalaryRangeMax] int NULL,
    [DailySalaryRangeCurrency] varchar(20) NULL,
    [HourlySalaryRange] int NULL,
    [HourlySalaryRangeMin] int NULL,
    [HourlySalaryRangeMax] int NULL,
    [HourlySalaryRangeCurrency] varchar(20) NULL,
    [Responsibilities] varchar(5000) NOT NULL,
    [Competencies] varchar(5000) NOT NULL,
    [GeneralRemarks] varchar(5000) NULL,
    [CreatedByNo] int NULL,
    [CreatedByUserID] varchar(50) NULL,
    [CreatedByName] varchar(100) NULL,
    [CreatedDate] datetime NULL DEFAULT '2025-11-03T16:53:52.329',
    [LastUpdatedByNo] int NULL,
    [LastUpdatedUserID] varchar(50) NULL,
    [LastUpdatedName] varchar(100) NULL,
    [LastUpdateDate] datetime NULL,
    CONSTRAINT [PK_RecruitmentRequest_RequisitionId] PRIMARY KEY ([RequisitionId])
);
GO

CREATE TABLE [kenuser].[JobQualification] (
    [AutoId] int NOT NULL IDENTITY,
    [QualificationCode] nvarchar(450) NOT NULL,
    [Qualification] nvarchar(max) NOT NULL,
    [StreamCode] nvarchar(450) NOT NULL,
    [Stream] nvarchar(max) NOT NULL,
    [SpecializationCode] nvarchar(max) NULL,
    [Specialization] nvarchar(max) NULL,
    [Remarks] varchar(500) NULL,
    [RequisitionId] int NOT NULL,
    CONSTRAINT [PK_JobQualification_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_JobQualification_RecruitmentRequest_RequisitionId] FOREIGN KEY ([RequisitionId]) REFERENCES [kenuser].[RecruitmentRequest] ([RequisitionId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Foreign key that references primary key: RecruitmentRequest.RequisitionId';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'JobQualification', 'COLUMN', N'RequisitionId';
GO

CREATE UNIQUE INDEX [IX_JobQualification_CompoKeys] ON [kenuser].[JobQualification] ([RequisitionId], [QualificationCode], [StreamCode]);
GO

CREATE UNIQUE INDEX [IX_RecruitmentRequisition_CompoKeys] ON [kenuser].[RecruitmentRequest] ([EmploymentTypeCode], [PositionTypeCode], [CompanyCode], [DepartmentCode], [EmployeeClass], [JobTitleCode], [PayGradeCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251103135353_AddRecruitmentRequisition', N'8.0.17');
GO

COMMIT;
GO

