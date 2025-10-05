BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'JobTitle');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[Employee] DROP COLUMN [JobTitle];
GO

EXEC sp_rename N'[kenuser].[Employee].[BankBranchCode]', N'CompanyBranchCode', N'COLUMN';
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'PayGrade');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [PayGrade] varchar(20) NOT NULL;
ALTER TABLE [kenuser].[Employee] ADD DEFAULT '0' FOR [PayGrade];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'Company');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var2 + '];');
UPDATE [kenuser].[Employee] SET [Company] = '' WHERE [Company] IS NULL;
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [Company] varchar(150) NOT NULL;
ALTER TABLE [kenuser].[Employee] ADD DEFAULT '' FOR [Company];
GO

ALTER TABLE [kenuser].[Employee] ADD [BankBranchName] varchar(150) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [JobTitleCode] varchar(20) NOT NULL DEFAULT '';
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-04T11:54:01.584' FOR [CreatedAt];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250904115403_UpdateFieldsEmployeeModel', N'8.0.17');
GO

COMMIT;
GO

