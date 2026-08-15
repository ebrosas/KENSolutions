BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[Employee] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [CreatedDate] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.0000000+00:00';
GO

ALTER TABLE [kenuser].[Employee] ADD [DeletedBy] nvarchar(max) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [DeletedDate] datetimeoffset NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [FirstNameAr] varchar(50) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [Id] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
GO

ALTER TABLE [kenuser].[Employee] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [kenuser].[Employee] ADD [LastNameAr] varchar(50) NOT NULL DEFAULT '';
GO

ALTER TABLE [kenuser].[Employee] ADD [LocationCode] nvarchar(max) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [MiddleNameAr] varchar(50) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [ModifiedBy] nvarchar(max) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [ModifiedDate] datetimeoffset NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [RowVersion] varbinary(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260815125403_ModifyEmployeeEntity', N'8.0.17');
GO

COMMIT;
GO

