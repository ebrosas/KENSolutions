BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'CompanyBranchCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[Employee] DROP COLUMN [CompanyBranchCode];
GO

ALTER TABLE [kenuser].[Employee] ADD [CompanyBranch] varchar(150) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-29T20:04:24.423' FOR [CreatedAt];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250929200426_RenameCompanyBranch', N'8.0.17');
GO

COMMIT;
GO

