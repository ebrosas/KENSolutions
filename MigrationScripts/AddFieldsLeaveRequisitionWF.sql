BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveCreatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD DEFAULT '2026-03-17T21:14:02.837' FOR [LeaveCreatedDate];
GO

ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD [LeaveAttachmentId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[LeaveEntitlement] ADD DEFAULT '2026-03-17T21:14:02.838' FOR [CreatedDate];
GO

ALTER TABLE [kenuser].[LeaveAttachments] ADD [LeaveAttachmentId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317181404_AddFieldsLeaveRequisitionWF', N'8.0.17');
GO

COMMIT;
GO

