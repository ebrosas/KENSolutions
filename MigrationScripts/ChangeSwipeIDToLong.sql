BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-01-31T00:25:47.933' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-01-31T00:25:47.929' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-01-31T00:25:47.941' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-01-31T00:25:47.948' FOR [CreatedDate];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-01-30T21:25:47.928' FOR [CreatedAt];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceTimesheet]') AND [c].[name] = N'CreatedDate');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[AttendanceTimesheet] ADD DEFAULT '2026-01-31T00:25:47.950' FOR [CreatedDate];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-01-31T00:25:47.949' FOR [SwipeLogDate];
GO


                ALTER TABLE kenuser.AttendanceSwipeLog DROP CONSTRAINT PK_AttendanceSwipeLog_SwipeID;

                ALTER TABLE kenuser.AttendanceSwipeLog DROP COLUMN SwipeID;

                ALTER TABLE kenuser.AttendanceSwipeLog ADD SwipeID BIGINT IDENTITY(1,1) NOT NULL;

                ALTER TABLE kenuser.AttendanceSwipeLog
                ADD CONSTRAINT PK_AttendanceSwipeLog_SwipeID PRIMARY KEY (SwipeID);
            
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260130212551_ChangeSwipeIDToLong', N'8.0.17');
GO

COMMIT;
GO

