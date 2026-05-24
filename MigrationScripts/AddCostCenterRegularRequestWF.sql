BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[RegularRequestWFs] ADD [CostCenter] varchar(20) NOT NULL DEFAULT '';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260524133815_AddCostCenterRegularRequestWF', N'8.0.17');
GO

COMMIT;
GO

