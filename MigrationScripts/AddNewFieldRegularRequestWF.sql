BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[RegularRequestWFs] ADD [NoPayHours] int NULL;
GO

ALTER TABLE [kenuser].[RegularRequestWFs] ADD [ShiftTiming] varchar(100) NULL;
GO

ALTER TABLE [kenuser].[RegularRequestWFs] ADD [WorkDuration] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260528182555_AddNewFieldRegularRequestWF', N'8.0.17');
GO

COMMIT;
GO

