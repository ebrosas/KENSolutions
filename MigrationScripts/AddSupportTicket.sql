BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentRequest]') AND [c].[name] = N'CreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentRequest] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[RecruitmentRequest] ADD DEFAULT '2026-02-21T15:55:06.787' FOR [CreatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[RecruitmentBudget]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[RecruitmentBudget] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[RecruitmentBudget] ADD DEFAULT '2026-02-21T15:55:06.786' FOR [CreatedDate];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[PayrollPeriod]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[PayrollPeriod] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[PayrollPeriod] ADD DEFAULT '2026-02-21T15:55:06.794' FOR [CreatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[MasterShiftPatternTitle]') AND [c].[name] = N'CreatedDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[MasterShiftPatternTitle] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[MasterShiftPatternTitle] ADD DEFAULT '2026-02-21T15:55:06.790' FOR [CreatedDate];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveCreatedDate');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD DEFAULT '2026-02-21T15:55:06.794' FOR [LeaveCreatedDate];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'CreatedDate');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[LeaveEntitlement] ADD DEFAULT '2026-02-21T15:55:06.794' FOR [CreatedDate];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Holiday]') AND [c].[name] = N'CreatedDate');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Holiday] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [kenuser].[Holiday] ADD DEFAULT '2026-02-21T15:55:06.793' FOR [CreatedDate];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2026-02-21T12:55:06.786' FOR [CreatedAt];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceTimesheet]') AND [c].[name] = N'CreatedDate');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [kenuser].[AttendanceTimesheet] ADD DEFAULT '2026-02-21T15:55:06.793' FOR [CreatedDate];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[AttendanceSwipeLog]') AND [c].[name] = N'SwipeLogDate');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[AttendanceSwipeLog] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [kenuser].[AttendanceSwipeLog] ADD DEFAULT '2026-02-21T15:55:06.793' FOR [SwipeLogDate];
GO

CREATE TABLE [kenuser].[SupportTickets] (
    [Id] uniqueidentifier NOT NULL,
    [Subject] nvarchar(max) NOT NULL,
    [Requester] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CreatedAtUtc] datetime NOT NULL,
    CONSTRAINT [PK_SupportTickets] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [kenuser].[SupportTicketAttachments] (
    [Id] uniqueidentifier NOT NULL,
    [SupportTicketId] uniqueidentifier NOT NULL,
    [FileName] varchar(100) NOT NULL,
    [StoredFileName] varchar(250) NOT NULL,
    [ContentType] varchar(50) NOT NULL,
    [FileSize] bigint NOT NULL,
    CONSTRAINT [PK_SupportTicketAttachments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SupportTicketAttachments_SupportTickets_SupportTicketId] FOREIGN KEY ([SupportTicketId]) REFERENCES [kenuser].[SupportTickets] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_SupportTicketAttachments_SupportTicketId] ON [kenuser].[SupportTicketAttachments] ([SupportTicketId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260221125507_AddSupportTicket', N'8.0.17');
GO

COMMIT;
GO

