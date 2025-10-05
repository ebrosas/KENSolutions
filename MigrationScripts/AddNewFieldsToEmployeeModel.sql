BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'PayGrade');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[Employee] ADD DEFAULT CAST(0 AS tinyint) FOR [PayGrade];
GO

ALTER TABLE [kenuser].[Employee] ADD [EmploymentTypeCode] varchar(20) NOT NULL DEFAULT '';
GO

ALTER TABLE [kenuser].[Employee] ADD [FirstAttendanceModeCode] varchar(20) NOT NULL DEFAULT '';
GO

ALTER TABLE [kenuser].[Employee] ADD [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit);
GO

ALTER TABLE [kenuser].[Employee] ADD [RoleCode] varchar(20) NOT NULL DEFAULT '';
GO

ALTER TABLE [kenuser].[Employee] ADD [SecondAttendanceModeCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [SecondReportingManagerCode] int NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [ThirdAttendanceModeCode] varchar(20) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'IsActive');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT CAST(1 AS bit) FOR [IsActive];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-08-28T13:09:44.455' FOR [CreatedAt];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250828130945_AddNewFieldsToEmployeeModel', N'8.0.17');
GO

COMMIT;
GO

