BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'StartDayMode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ALTER COLUMN [StartDayMode] varchar(20) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveRequisitionWF]') AND [c].[name] = N'EndDayMode');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveRequisitionWF] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[LeaveRequisitionWF] ALTER COLUMN [EndDayMode] varchar(20) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260318130923_ChangeDataTypeLeaveMode', N'8.0.17');
GO

COMMIT;
GO

