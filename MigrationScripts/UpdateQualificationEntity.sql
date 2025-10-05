BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[Qualification] DROP CONSTRAINT [FK_Qualification_Employee_EmployeeNo];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Qualification]') AND [c].[name] = N'CityCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Qualification] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[Qualification] DROP COLUMN [CityCode];
GO

ALTER TABLE [kenuser].[Qualification] ADD [CityTownName] varchar(100) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-09T19:56:00.222' FOR [CreatedAt];
GO

ALTER TABLE [kenuser].[Qualification] ADD CONSTRAINT [FK_Qualification_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeNo]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250909195601_UpdateQualificationEntity', N'8.0.17');
GO

COMMIT;
GO

