BEGIN TRANSACTION;
GO

EXEC sp_rename N'[kenuser].[AttendanceSwipeLog].[SwipeCode]', N'SwipeType', N'COLUMN';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-01-30T23:52:42.385' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-01-30T23:52:42.378' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-01-30T23:52:42.393' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-01-30T23:52:42.400' FOR [CreatedDate];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-01-30T20:52:42.377' FOR [CreatedAt];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-01-30T23:52:42.401' FOR [SwipeLogDate];
GO

CREATE TABLE [kenuser].[AttendanceTimesheet] (
    [TimesheetId] float NOT NULL,
    [EmpNo] int NOT NULL,
    [CostCenter] varchar(20) NOT NULL,
    [PayGrade] varchar(20) NULL,
    [AttendanceDate] datetime NOT NULL,
    [TimeIn] datetime NULL,
    [TimeOut] datetime NULL,
    [ShavedIn] datetime NULL,
    [ShavedOut] datetime NULL,
    [OTType] varchar(20) NULL,
    [OTStartTime] datetime NULL,
    [OTEndTime] datetime NULL,
    [ShiftPatCode] varchar(20) NULL,
    [SchedShiftCode] varchar(10) NULL,
    [ActualShiftCode] varchar(10) NULL,
    [NoPayHours] int NULL,
    [RemarkCode] varchar(10) NULL,
    [AbsenceReasonCode] varchar(10) NULL,
    [AbsenceReasonColumn] varchar(10) NULL,
    [LeaveType] varchar(10) NULL,
    [DIL_Entitlement] varchar(10) NULL,
    [CorrectionCode] varchar(10) NULL,
    [Processed] bit NULL,
    [ProcessID] int NULL,
    [UploadID] int NULL,
    [IsPublicHoliday] bit NULL,
    [IsDILHoliday] bit NULL,
    [IsRamadan] bit NULL,
    [IsMuslim] bit NULL,
    [IsDriver] bit NULL,
    [IsDILDayWorker] bit NULL,
    [IsSalaryStaff] bit NULL,
    [IsDayWorkerOrShifter] bit NULL,
    [IsLiasonOfficer] bit NULL,
    [IsLastRow] bit NULL,
    [DurationRequired] int NULL,
    [DurationWorked] int NULL,
    [DurationWorkedCumulative] int NULL,
    [NetMinutes] int NULL,
    [CreatedByEmpNo] int NULL,
    [CreatedByUserID] varchar(50) NULL,
    [CreatedDate] datetime NULL DEFAULT '2026-01-30T23:52:42.402',
    [LastUpdateDate] datetime NULL,
    [LastUpdateEmpNo] int NULL,
    [LastUpdateUserID] varchar(50) NULL,
    CONSTRAINT [PK_AttendanceTimesheet_TimesheetId] PRIMARY KEY ([TimesheetId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260130205245_ModifyAttendanceSwipeLog', N'8.0.17');
GO

COMMIT;
GO

