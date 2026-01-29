BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-01-29T16:23:31.269' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-01-29T16:23:31.268' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-01-29T16:23:31.272' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-01-29T16:23:31.276' FOR [CreatedDate];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-01-29T13:23:31.267' FOR [CreatedAt];
GO

CREATE TABLE [kenuser].[AttendanceSwipeLog] (
    [SwipeID] float NOT NULL,
    [EmpNo] int NOT NULL,
    [SwipeDate] datetime NOT NULL,
    [SwipeTime] datetime NULL,
    [SwipeCode] varchar(20) NULL,
    [LocationCode] varchar(20) NULL,
    [ReaderCode] varchar(20) NULL,
    [StatusCode] varchar(20) NULL,
    [SwipeLogDate] datetime NULL DEFAULT '2026-01-29T16:23:31.276',
    CONSTRAINT [PK_AttendanceSwipeLog_SwipeID] PRIMARY KEY ([SwipeID])
);
GO

CREATE UNIQUE INDEX [IX_AttendanceSwipeLog_CompoKeys] ON [kenuser].[AttendanceSwipeLog] ([EmpNo], [SwipeDate], [SwipeTime]) WHERE [SwipeTime] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260129132332_CreateAttendanceSwipeLog', N'8.0.17');
GO

COMMIT;
GO

