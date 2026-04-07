BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[SupportTickets]') AND [c].[name] = N'Subject');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[SupportTickets] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[SupportTickets] ALTER COLUMN [Subject] nvarchar(200) NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[SupportTickets]') AND [c].[name] = N'Requester');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[SupportTickets] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[SupportTickets] ALTER COLUMN [Requester] nvarchar(150) NOT NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[SupportTickets]') AND [c].[name] = N'Description');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[SupportTickets] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[SupportTickets] ALTER COLUMN [Description] nvarchar(2000) NOT NULL;
GO

CREATE TABLE [kenuser].[RequestApprovals] (
    [ApprovalId] bigint NOT NULL IDENTITY,
    [RequestTypeCode] varchar(20) NOT NULL,
    [RequisitionNo] bigint NOT NULL,
    [RoutineSequence] int NULL,
    [AssignedEmpNo] int NOT NULL,
    [AssignedEmpName] varchar(150) NULL,
    [ApprovalRole] varchar(150) NULL,
    [ActionRole] int NOT NULL,
    [IsApproved] bit NULL,
    [IsHold] bit NULL,
    [Remarks] varchar(500) NULL,
    [CreatedDate] datetime NULL DEFAULT '2026-04-07T12:14:58.550',
    [CreatedBy] int NULL,
    [CreatedUserID] varchar(50) NULL,
    [LastUpdatedDate] datetime NULL,
    [LastUpdatedBy] int NULL,
    [LastUpdatedUserID] varchar(50) NULL,
    CONSTRAINT [PK_RequestApproval_ApprovalID] PRIMARY KEY ([ApprovalId])
);
GO

CREATE INDEX [IX_RequestApproval_CompoKeys] ON [kenuser].[RequestApprovals] ([RequestTypeCode], [RequisitionNo], [AssignedEmpNo], [ActionRole]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260407091459_AddRequestApproval', N'8.0.17');
GO

COMMIT;
GO

