BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[WorkflowStepDefinitions] ADD [ApprovalStageDesc] varchar(300) NULL;
GO

DECLARE @description AS sql_variant;
EXEC sp_dropextendedproperty 'MS_Description', 'SCHEMA', N'kenuser', 'TABLE', N'WorkflowApprovalRoles', 'COLUMN', N'GroupType';
SET @description = N'0 = Assignee Employee; 1 = Supervisor; 2 = Superintendent; 3 = Cost Center Manager';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'WorkflowApprovalRoles', 'COLUMN', N'GroupType';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260420124530_AddApprovalStageDesc', N'8.0.17');
GO

COMMIT;
GO

