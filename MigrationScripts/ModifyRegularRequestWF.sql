BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[RegularRequestWFs] ADD [StatusCode] varchar(20) NOT NULL DEFAULT '';
GO

ALTER TABLE [kenuser].[RegularRequestWFs] ADD [StatusHandlingCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[RegularRequestWFs] ADD [StatusID] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260524121219_ModifyRegularRequestWF', N'8.0.17');
GO

COMMIT;
GO

