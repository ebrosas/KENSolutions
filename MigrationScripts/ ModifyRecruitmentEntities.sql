BEGIN TRANSACTION;
GO

DROP INDEX [IX_RecruitmentRequisition_CompoKeys] ON [kenuser].[RecruitmentRequest];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'EmployeeClass');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] DROP COLUMN [EmployeeClass];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[JobQualification]') AND [c].[name] = N'Qualification');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[JobQualification] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[JobQualification] DROP COLUMN [Qualification];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[JobQualification]') AND [c].[name] = N'Specialization');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[JobQualification] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[JobQualification] DROP COLUMN [Specialization];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[JobQualification]') AND [c].[name] = N'Stream');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[JobQualification] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[JobQualification] DROP COLUMN [Stream];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'VideoDescriptionURL');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ALTER COLUMN [VideoDescriptionURL] varchar(200) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'SalaryRangeType');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ALTER COLUMN [SalaryRangeType] varchar(50) NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'RequiredGender');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ALTER COLUMN [RequiredGender] varchar(50) NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'RequiredAsset');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ALTER COLUMN [RequiredAsset] varchar(200) NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2025-11-05T15:07:33.568' FOR [CreatedDate];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2025-11-05T15:07:33.473' FOR [CreatedDate];
GO

DROP INDEX [IX_JobQualification_CompoKeys] ON [kenuser].[JobQualification];
DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[JobQualification]') AND [c].[name] = N'StreamCode');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[JobQualification] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [kenuser].[JobQualification] ALTER COLUMN [StreamCode] varchar(20) NOT NULL;
CREATE UNIQUE INDEX [IX_JobQualification_CompoKeys] ON [kenuser].[JobQualification] ([RequisitionId], [QualificationCode], [StreamCode]);
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[JobQualification]') AND [c].[name] = N'SpecializationCode');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[JobQualification] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [kenuser].[JobQualification] ALTER COLUMN [SpecializationCode] varchar(20) NULL;
GO

DROP INDEX [IX_JobQualification_CompoKeys] ON [kenuser].[JobQualification];
DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[JobQualification]') AND [c].[name] = N'QualificationCode');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[JobQualification] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [kenuser].[JobQualification] ALTER COLUMN [QualificationCode] varchar(20) NOT NULL;
CREATE UNIQUE INDEX [IX_JobQualification_CompoKeys] ON [kenuser].[JobQualification] ([RequisitionId], [QualificationCode], [StreamCode]);
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-11-05T12:07:33.452' FOR [CreatedAt];
GO

CREATE UNIQUE INDEX [IX_RecruitmentRequisition_CompoKeys] ON [kenuser].[RecruitmentRequest] ([EmploymentTypeCode], [PositionTypeCode], [CompanyCode], [DepartmentCode], [EmployeeClassCode], [JobTitleCode], [PayGradeCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251105120734_ModifyRecruitmentEntities', N'8.0.17');
GO

COMMIT;
GO

