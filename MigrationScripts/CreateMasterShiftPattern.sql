BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-01-07T22:42:23.426' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-01-07T22:42:23.422' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-01-07T22:42:23.436' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-01-07T19:42:23.421' FOR [CreatedAt];
GO

CREATE TABLE [kenuser].[ShiftPatternChange] (
    [AutoId] int NOT NULL IDENTITY,
    [EmpNo] int NOT NULL,
    [ShiftPatternCode] varchar(20) NOT NULL,
    [ShiftPointer] int NOT NULL,
    [ChangeType] varchar(20) NOT NULL,
    [EffectiveDate] datetime NOT NULL,
    [EndingDate] datetime NULL,
    [CreatedByEmpNo] int NULL,
    [CreatedByName] varchar(50) NULL,
    [CreatedByUserID] varchar(50) NULL,
    [CreatedDate] datetime NULL,
    [LastUpdateDate] datetime NULL,
    [LastUpdateEmpNo] int NULL,
    [LastUpdateUserID] varchar(50) NULL,
    [LastUpdatedByName] varchar(50) NULL,
    CONSTRAINT [PK_ShiftPatternChange_AutoId] PRIMARY KEY ([AutoId])
);
GO

CREATE UNIQUE INDEX [IX_ShiftPatternChange_CompoKeys] ON [kenuser].[ShiftPatternChange] ([EmpNo], [ShiftPatternCode], [ShiftPointer], [EffectiveDate]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260107194225_CreateMasterShiftPattern', N'8.0.17');
GO

COMMIT;
GO

