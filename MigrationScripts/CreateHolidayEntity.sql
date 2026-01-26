BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-01-26T14:37:54.055' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-01-26T14:37:54.054' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-01-26T14:37:54.057' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-01-26T11:37:54.053' FOR [CreatedAt];
GO

CREATE TABLE [kenuser].[Holiday] (
    [HolidayId] int NOT NULL IDENTITY,
    [HolidayDesc] varchar(200) NOT NULL,
    [HolidayDate] datetime NOT NULL,
    [HolidayType] tinyint NOT NULL,
    [CreatedByEmpNo] int NULL,
    [CreatedByName] varchar(50) NULL,
    [CreatedByUserID] varchar(50) NULL,
    [CreatedDate] datetime NULL DEFAULT '2026-01-26T14:37:54.059',
    [LastUpdateDate] datetime NULL,
    [LastUpdateEmpNo] int NULL,
    [LastUpdateUserID] varchar(50) NULL,
    [LastUpdatedByName] varchar(50) NULL,
    CONSTRAINT [PK_Holiday_HolidayId] PRIMARY KEY ([HolidayId])
);
GO

CREATE UNIQUE INDEX [IX_Holiday_CompoKeys] ON [kenuser].[Holiday] ([HolidayDesc], [HolidayDate], [HolidayType]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260126113755_CreateHolidayEntity', N'8.0.17');
GO

COMMIT;
GO

