BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmployeeCertification]') AND [c].[name] = N'CityCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmployeeCertification] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[EmployeeCertification] DROP COLUMN [CityCode];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmployeeCertification]') AND [c].[name] = N'StateCode');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmployeeCertification] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[EmployeeCertification] DROP COLUMN [StateCode];
GO

ALTER TABLE [kenuser].[EmployeeCertification] ADD [CityTownName] varchar(100) NULL;
GO

ALTER TABLE [kenuser].[EmployeeCertification] ADD [State] varchar(100) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-12T20:28:31.545' FOR [CreatedAt];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250912202833_ChangeEmployeeCertificationStateCity', N'8.0.17');
GO

COMMIT;
GO

