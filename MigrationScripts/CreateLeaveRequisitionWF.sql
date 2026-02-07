BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-02-07T13:17:45.504' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-02-07T13:17:45.504' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-02-07T13:17:45.506' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-02-07T13:17:45.508' FOR [CreatedDate];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-02-07T10:17:45.503' FOR [CreatedAt];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceTimesheet]') AND [c].[name] = N'CreatedDate');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[AttendanceTimesheet] ADD DEFAULT '2026-02-07T13:17:45.509' FOR [CreatedDate];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-02-07T13:17:45.509' FOR [SwipeLogDate];
GO

CREATE TABLE [kenuser].[LeaveRequisitionWF] (
    [LeaveRequestId] bigint NOT NULL IDENTITY,
    [LeaveInstanceID] varchar(50) NULL,
    [LeaveType] varchar(20) NOT NULL,
    [LeaveEmpNo] int NOT NULL,
    [LeaveEmpName] varchar(150) NULL,
    [LeaveEmpEmail] varchar(50) NULL,
    [LeaveStartDate] datetime NOT NULL,
    [LeaveEndDate] datetime NOT NULL,
    [LeaveResumeDate] datetime NOT NULL,
    [LeaveEmpCostCenter] varchar(20) NULL,
    [LeaveRemarks] varchar(500) NULL,
    [LeaveConstraints] bit NULL,
    [LeaveStatusCode] varchar(20) NOT NULL,
    [LeaveApprovalFlag] nvarchar(1) NULL,
    [LeaveVisaRequired] bit NULL,
    [LeavePayAdv] bit NULL,
    [LeaveIsFTMember] bit NULL,
    [LeaveBalance] float NULL,
    [LeaveDuration] float NULL,
    [NoOfHolidays] int NULL,
    [NoOfWeekends] int NULL,
    [PlannedLeave] nvarchar(1) NULL,
    [LeavePlannedNo] int NULL,
    [HalfDayLeaveFlag] nvarchar(1) NULL,
    [LeaveCreatedDate] datetime NULL DEFAULT '2026-02-07T13:17:45.509',
    [LeaveCreatedBy] int NULL,
    [LeaveCreatedUserID] varchar(50) NULL,
    [LeaveCreatedEmail] varchar(50) NULL,
    [LeaveUpdatedDate] datetime NULL,
    [LeaveUpdatedBy] int NULL,
    [LeaveUpdatedUserID] varchar(50) NULL,
    [LeaveUpdatedEmail] varchar(50) NULL,
    CONSTRAINT [PK_LeaveRequisitionWF_LeaveRequestId] PRIMARY KEY ([LeaveRequestId])
);
GO

CREATE UNIQUE INDEX [IX_LeaveRequisitionWF_CompoKeys] ON [kenuser].[LeaveRequisitionWF] ([LeaveEmpNo], [LeaveType], [LeaveStartDate], [LeaveEndDate], [LeaveStatusCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260207101746_CreateLeaveRequisitionWF', N'8.0.17');
GO

COMMIT;
GO

