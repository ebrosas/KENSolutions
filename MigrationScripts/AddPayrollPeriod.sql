BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-02-12T14:10:44.347' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-02-12T14:10:44.346' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-02-12T14:10:44.351' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveCreatedDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD DEFAULT '2026-02-12T14:10:44.356' FOR [LeaveCreatedDate];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'CreatedDate');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[LeaveEntitlement] ADD DEFAULT '2026-02-12T14:10:44.357' FOR [CreatedDate];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-02-12T14:10:44.355' FOR [CreatedDate];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-02-12T11:10:44.345' FOR [CreatedAt];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceTimesheet]') AND [c].[name] = N'CreatedDate');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [kenuser].[AttendanceTimesheet] ADD DEFAULT '2026-02-12T14:10:44.356' FOR [CreatedDate];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-02-12T14:10:44.356' FOR [SwipeLogDate];
GO

CREATE TABLE [kenuser].[PayrollPeriod] (
    [PayrollPeriodId] int NOT NULL IDENTITY,
    [FiscalYear] int NOT NULL,
    [PayrollStartDate] datetime NOT NULL,
    [PayrollEndDate] datetime NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedByEmpNo] int NULL,
    [CreatedByUserID] varchar(50) NULL,
    [CreatedDate] datetime NULL DEFAULT '2026-02-12T14:10:44.357',
    [LastUpdateEmpNo] int NULL,
    [LastUpdateUserID] varchar(50) NULL,
    [LastUpdateDate] datetime NULL,
    CONSTRAINT [PK_PayrollPeriod_PayrollPeriodId] PRIMARY KEY ([PayrollPeriodId])
);
GO

CREATE UNIQUE INDEX [IX_PayrollPeriod_CompoKeys] ON [kenuser].[PayrollPeriod] ([FiscalYear], [PayrollStartDate], [PayrollEndDate]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260212111045_AddPayrollPeriod', N'8.0.17');
GO

COMMIT;
GO

