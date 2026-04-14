BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[WorkflowConditions] ADD [IsTerminal] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260414113845_AddIsTerminal', N'8.0.17');
GO

COMMIT;
GO

