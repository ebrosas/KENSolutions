BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[Qualification] ADD [StateName] varchar(100) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260724133341_AddQualificationStateName', N'8.0.17');
GO

COMMIT;
GO

