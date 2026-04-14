BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[WorkflowConditions]') AND [c].[name] = N'NextStepDefinitionId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[WorkflowConditions] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[WorkflowConditions] ALTER COLUMN [NextStepDefinitionId] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260414114934_SetNullableNextStepDefinitionId', N'8.0.17');
GO

COMMIT;
GO

