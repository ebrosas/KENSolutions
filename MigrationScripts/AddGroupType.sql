BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[WorkflowApprovalRoles] ADD [GroupType] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);
DECLARE @description AS sql_variant;
SET @description = N'0 or NULL = Based on Assigned Employee No; 1 = Based on Employee Master; 2 = Based on Department Master';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'WorkflowApprovalRoles', 'COLUMN', N'GroupType';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260416194935_AddGroupType', N'8.0.17');
GO

COMMIT;
GO

