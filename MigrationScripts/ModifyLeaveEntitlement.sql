BEGIN TRANSACTION;
GO

DROP INDEX [IX_LeaveEntitlement_CompoKeys] ON [kenuser].[LeaveEntitlement];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'SickLeaveEntitlemnt');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[LeaveEntitlement] DROP COLUMN [SickLeaveEntitlemnt];
GO

EXEC sp_rename N'[kenuser].[LeaveEntitlement].[LeaveEntitlemnt]', N'ALEntitlementCount', N'COLUMN';
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'LeaveUOM');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[LeaveEntitlement] ALTER COLUMN [LeaveUOM] varchar(20) NOT NULL;
DECLARE @description AS sql_variant;
EXEC sp_dropextendedproperty 'MS_Description', 'SCHEMA', N'kenuser', 'TABLE', N'LeaveEntitlement', 'COLUMN', N'LeaveUOM';
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[LeaveEntitlement]') AND [c].[name] = N'CreatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[LeaveEntitlement] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[LeaveEntitlement] ADD DEFAULT '2026-04-01T16:00:49.728' FOR [CreatedDate];
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [ALRenewalType] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [EffectiveDate] datetime NOT NULL DEFAULT GETDATE();
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'LeaveEntitlement', 'COLUMN', N'EffectiveDate';
GO

ALTER TABLE [kenuser].[LeaveEntitlement] DROP COLUMN LeaveEntitlemnt
ALTER TABLE [kenuser].[LeaveEntitlement] DROP COLUMN SickLeaveEntitlemnt

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [SLEntitlementCount] float NULL;
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [SLRenewalType] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[LeaveEntitlement] ADD [SickLeaveUOM] varchar(20) NULL;
GO

CREATE UNIQUE INDEX [IX_LeaveEntitlement_CompoKeys] ON [kenuser].[LeaveEntitlement] ([EmployeeNo], [EffectiveDate]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260401130051_ModifyLeaveEntitlement', N'8.0.17');
GO

COMMIT;
GO

