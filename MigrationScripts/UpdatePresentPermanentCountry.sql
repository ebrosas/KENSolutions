BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'PermanentCityCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[Employee] DROP COLUMN [PermanentCityCode];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'PresentCityCode');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[Employee] DROP COLUMN [PresentCityCode];
GO

ALTER TABLE [kenuser].[Employee] ADD [PermanentCity] varchar(100) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PresentCity] varchar(100) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-04T20:38:47.531' FOR [CreatedAt];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250904203848_UpdatePresentPermanentCountry', N'8.0.17');
GO

COMMIT;
GO

