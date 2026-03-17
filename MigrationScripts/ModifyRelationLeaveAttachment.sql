BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[LeaveAttachments] DROP CONSTRAINT [FK_LeaveAttachments_LeaveRequisitionWF_LeaveRequestId];
GO

DROP INDEX [IX_LeaveAttachment_CompoKeys] ON [kenuser].[LeaveAttachments];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveAttachments]') AND [c].[name] = N'LeaveRequestId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveAttachments] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[LeaveAttachments] DROP COLUMN [LeaveRequestId];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'LeaveCreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD DEFAULT '2026-03-17T21:49:59.212' FOR [LeaveCreatedDate];
GO

ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD CONSTRAINT [AK_LeaveRequisitionWF_LeaveAttachmentId] UNIQUE ([LeaveAttachmentId]);
GO

CREATE INDEX [IX_LeaveAttachments_LeaveAttachmentId] ON [kenuser].[LeaveAttachments] ([LeaveAttachmentId]);
GO

ALTER TABLE [kenuser].[LeaveAttachments] ADD CONSTRAINT [FK_LeaveAttachments_LeaveRequisitionWF_LeaveAttachmentId] FOREIGN KEY ([LeaveAttachmentId]) REFERENCES [kenuser].[LeaveRequisitionWF] ([LeaveAttachmentId]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317185002_ModifyRelationLeaveAttachment', N'8.0.17');
GO

COMMIT;
GO

