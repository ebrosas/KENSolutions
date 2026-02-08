BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-02-07T14:45:08.922' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-02-07T14:45:08.921' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-02-07T14:45:08.924' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'PlannedLeave');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ALTER COLUMN [PlannedLeave] char(1) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveCreatedDate');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD DEFAULT '2026-02-07T14:45:08.931' FOR [LeaveCreatedDate];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveApprovalFlag');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ALTER COLUMN [LeaveApprovalFlag] char(1) NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'HalfDayLeaveFlag');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ALTER COLUMN [HalfDayLeaveFlag] char(1) NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-02-07T14:45:08.930' FOR [CreatedDate];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-02-07T11:45:08.921' FOR [CreatedAt];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceTimesheet]') AND [c].[name] = N'CreatedDate');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [kenuser].[AttendanceTimesheet] ADD DEFAULT '2026-02-07T14:45:08.930' FOR [CreatedDate];
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-02-07T14:45:08.930' FOR [SwipeLogDate];
GO

CREATE TABLE [kenuser].[LeaveEntitlement] (
    [LeaveEntitlementId] int NOT NULL IDENTITY,
    [LeaveEntitlemnt] float NOT NULL,
    [SickLeaveEntitlemnt] float NOT NULL,
    [LeaveUOM] char(1) NOT NULL,
    [CreatedDate] datetime NULL DEFAULT '2026-02-07T14:45:08.931',
    [LeaveCreatedBy] int NULL,
    [CreatedUserID] varchar(50) NULL,
    [LastUpdatedDate] datetime NULL,
    [LastUpdatedBy] INT NULL,
    [LastUpdatedUserID] VARCHAR(50) NULL,
    [EmployeeNo] INT NOT NULL,
    CONSTRAINT [LeaveEntitlementId] PRIMARY KEY ([LeaveEntitlementId]),
    CONSTRAINT [FK_LeaveEntitlement_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeNo]) ON DELETE CASCADE	
);
DECLARE @description AS SQL_VARIANT;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'LeaveEntitlement', 'COLUMN', N'LeaveEntitlemnt';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'LeaveEntitlement', 'COLUMN', N'LeaveUOM';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'LeaveEntitlement', 'COLUMN', N'EmployeeNo';
GO

CREATE UNIQUE INDEX [IX_LeaveEntitlement_CompoKeys] ON [kenuser].[LeaveEntitlement] ([EmployeeNo], [LeaveEntitlemnt]);
GO

CREATE UNIQUE INDEX [IX_LeaveEntitlement_EmployeeNo] ON [kenuser].[LeaveEntitlement] ([EmployeeNo]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260207114509_CreateLeaveEntitlement', N'8.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [FK_LeaveEntitlement_Employee_EmployeeNo];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-02-08T15:58:27.090' FOR [CreatedDate];
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-02-08T15:58:27.088' FOR [CreatedDate];
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-02-08T15:58:27.094' FOR [CreatedDate];
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveCreatedDate');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD DEFAULT '2026-02-08T15:58:27.100' FOR [LeaveCreatedDate];
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'CreatedDate');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [kenuser].[LeaveEntitlement] ADD DEFAULT '2026-02-08T15:58:27.100' FOR [CreatedDate];
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [DILBalance] float NULL;
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [LeaveBalance] float NULL;
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [SLBalance] float NULL;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-02-08T15:58:27.098' FOR [CreatedDate];
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-02-08T12:58:27.088' FOR [CreatedAt];
GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceTimesheet]') AND [c].[name] = N'CreatedDate');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [kenuser].[AttendanceTimesheet] ADD DEFAULT '2026-02-08T15:58:27.099' FOR [CreatedDate];
GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-02-08T15:58:27.099' FOR [SwipeLogDate];
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD CONSTRAINT [FK_LeaveEntitlement_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeNo]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260208125828_AddFieldsToLeaveEntitlement', N'8.0.17');
GO

COMMIT;
GO

