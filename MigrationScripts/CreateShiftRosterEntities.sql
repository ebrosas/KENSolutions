BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2025-12-08T16:50:34.003' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2025-12-08T16:50:34.002' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-12-08T13:50:34.002' FOR [CreatedAt];
GO

CREATE TABLE [kenuser].[MasterShiftPatternTitle] (
    [ShiftPatternId] int NOT NULL IDENTITY,
    [ShiftPatternCode] varchar(20) NOT NULL,
    [ShiftPatternDescription] varchar(300) NULL,
    [IsActive] bit NOT NULL,
    [IsDayShift] bit NULL,
    [IsFlexiTime] bit NULL,
    [CreatedByEmpNo] int NULL,
    [CreatedByName] varchar(50) NULL,
    [CreatedByUserID] varchar(50) NULL,
    [CreatedDate] datetime NULL DEFAULT '2025-12-08T16:50:34.004',
    [LastUpdateDate] datetime NULL,
    [LastUpdateEmpNo] int NULL,
    [LastUpdateUserID] varchar(50) NULL,
    [LastUpdatedByName] varchar(50) NULL,
    CONSTRAINT [PK_MasterShiftPatternTitle_ShiftPatternId] PRIMARY KEY ([ShiftPatternId]),
    CONSTRAINT [AK_MasterShiftPatternTitle_ShiftPatternCode] UNIQUE ([ShiftPatternCode])
);
GO

CREATE TABLE [kenuser].[MasterShiftPattern] (
    [ShiftPointerId] int NOT NULL IDENTITY,
    [ShiftPointer] int NOT NULL,
    [ShiftDescription] varchar(200) NULL,
    [ShiftCode] varchar(10) NOT NULL,
    [ShiftPatternCode] varchar(20) NOT NULL,
    CONSTRAINT [PK_MasterShiftPattern_ShiftPointerId] PRIMARY KEY ([ShiftPointerId]),
    CONSTRAINT [FK_MasterShiftPattern_MasterShiftPatternTitle_ShiftPatternCode] FOREIGN KEY ([ShiftPatternCode]) REFERENCES [kenuser].[MasterShiftPatternTitle] ([ShiftPatternCode]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Foreign key that references alternate key: MasterShiftPatternTitle.ShiftPatternCode';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'MasterShiftPattern', 'COLUMN', N'ShiftPatternCode';
GO

CREATE TABLE [kenuser].[MasterShiftTime] (
    [ShiftTimingId] int NOT NULL IDENTITY,
    [ShiftCode] varchar(10) NOT NULL,
    [ShiftDescription] varchar(200) NOT NULL,
    [ArrivalFrom] time NULL,
    [ArrivalTo] time NOT NULL,
    [DepartFrom] time NOT NULL,
    [DepartTo] time NULL,
    [DurationNormal] int NOT NULL,
    [RArrivalFrom] time NULL,
    [RArrivalTo] time NULL,
    [RDepartFrom] time NULL,
    [RDepartTo] time NULL,
    [DurationRamadan] int NULL,
    [CreatedByEmpNo] int NULL,
    [CreatedByName] varchar(50) NULL,
    [CreatedByUserID] varchar(50) NULL,
    [CreatedDate] datetime NULL,
    [LastUpdateDate] datetime NULL,
    [LastUpdateEmpNo] int NULL,
    [LastUpdateUserID] varchar(50) NULL,
    [LastUpdatedByName] varchar(50) NULL,
    [ShiftPatternCode] varchar(20) NOT NULL,
    CONSTRAINT [PK_MasterShiftTime_ShiftTimingId] PRIMARY KEY ([ShiftTimingId]),
    CONSTRAINT [FK_MasterShiftTime_MasterShiftPatternTitle_ShiftPatternCode] FOREIGN KEY ([ShiftPatternCode]) REFERENCES [kenuser].[MasterShiftPatternTitle] ([ShiftPatternCode]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Foreign key that references alternate key: MasterShiftPatternTitle.ShiftPatternCode';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'MasterShiftTime', 'COLUMN', N'ShiftPatternCode';
GO

CREATE UNIQUE INDEX [IX_MasterShiftPattern_CompoKeys] ON [kenuser].[MasterShiftPattern] ([ShiftPatternCode], [ShiftCode]);
GO

CREATE UNIQUE INDEX [IX_MasterShiftPatternTitle_UniqueKey] ON [kenuser].[MasterShiftPatternTitle] ([ShiftPatternCode]);
GO

CREATE UNIQUE INDEX [IX_MasterShiftTime_CompoKeys] ON [kenuser].[MasterShiftTime] ([ShiftPatternCode], [ShiftCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251208135035_CreateShiftRosterEntities', N'8.0.17');
GO

COMMIT;
GO

