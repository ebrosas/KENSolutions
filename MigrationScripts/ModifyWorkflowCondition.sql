BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[WorkflowConditions] ADD [Expression] varchar(500) NULL;
GO

--DECLARE @var0 sysname;
--SELECT @var0 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[WorkflowApprovalRoles]') AND [c].[name] = N'CreatedDate');
--IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[WorkflowApprovalRoles] DROP CONSTRAINT [' + @var0 + '];');
--ALTER TABLE [kenuser].[WorkflowApprovalRoles] ADD DEFAULT '2026-04-14T12:42:51.249' FOR [CreatedDate];
--GO

--DECLARE @var1 sysname;
--SELECT @var1 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RequestApprovals]') AND [c].[name] = N'CreatedDate');
--IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RequestApprovals] DROP CONSTRAINT [' + @var1 + '];');
--ALTER TABLE [kenuser].[RequestApprovals] ADD DEFAULT '2026-04-14T12:42:51.245' FOR [CreatedDate];
--GO

--DECLARE @var2 sysname;
--SELECT @var2 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
--IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var2 + '];');
--ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-04-14T12:42:51.287' FOR [CreatedDate];
--GO

--DECLARE @var3 sysname;
--SELECT @var3 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
--IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var3 + '];');
--ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-04-14T12:42:51.286' FOR [CreatedDate];
--GO

--DECLARE @var4 sysname;
--SELECT @var4 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[PayrollPeriod]') AND [c].[name] = N'CreatedDate');
--IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[PayrollPeriod] DROP CONSTRAINT [' + @var4 + '];');
--ALTER TABLE [kenuser].[PayrollPeriod] ADD DEFAULT '2026-04-14T12:42:51.295' FOR [CreatedDate];
--GO

--DECLARE @var5 sysname;
--SELECT @var5 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
--IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var5 + '];');
--ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-04-14T12:42:51.290' FOR [CreatedDate];
--GO

--DECLARE @var6 sysname;
--SELECT @var6 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveCreatedDate');
--IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var6 + '];');
--ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD DEFAULT '2026-04-14T12:42:51.293' FOR [LeaveCreatedDate];
--GO

--DECLARE @var7 sysname;
--SELECT @var7 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'EffectiveDate');
--IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var7 + '];');
--ALTER TABLE [kenuser].[LeaveEntitlement] ADD DEFAULT '2026-04-14T12:42:51.295' FOR [EffectiveDate];
--GO

--DECLARE @var8 sysname;
--SELECT @var8 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'CreatedDate');
--IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var8 + '];');
--ALTER TABLE [kenuser].[LeaveEntitlement] ADD DEFAULT '2026-04-14T12:42:51.295' FOR [CreatedDate];
--GO

--DECLARE @var9 sysname;
--SELECT @var9 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
--IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var9 + '];');
--ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-04-14T12:42:51.292' FOR [CreatedDate];
--GO

--DECLARE @var10 sysname;
--SELECT @var10 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
--IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var10 + '];');
--ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-04-14T09:42:51.285' FOR [CreatedAt];
--GO

--DECLARE @var11 sysname;
--SELECT @var11 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceTimesheet]') AND [c].[name] = N'CreatedDate');
--IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [' + @var11 + '];');
--ALTER TABLE [kenuser].[AttendanceTimesheet] ADD DEFAULT '2026-04-14T12:42:51.293' FOR [CreatedDate];
--GO

--DECLARE @var12 sysname;
--SELECT @var12 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
--IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var12 + '];');
--ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-04-14T12:42:51.292' FOR [SwipeLogDate];
--GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260414094253_ModifyWorkflowCondition', N'8.0.17');
GO

COMMIT;
GO

