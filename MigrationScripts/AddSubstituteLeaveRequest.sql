BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD [SubstituteName] varchar(100) NULL;
GO

ALTER TABLE [kenuser].[LeaveRequisitionWF] ADD [SubstituteNo] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260709132254_AddSubstituteLeaveRequest', N'8.0.17');
GO

COMMIT;
GO

