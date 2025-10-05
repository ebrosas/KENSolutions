BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[EmploymentHistory] DROP CONSTRAINT [FK_EmploymentHistory_Employee_EmployeeNo];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmploymentHistory]') AND [c].[name] = N'ReportingManager');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmploymentHistory] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[EmploymentHistory] ALTER COLUMN [ReportingManager] int NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmploymentHistory]') AND [c].[name] = N'LastDrawnSalary');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmploymentHistory] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[EmploymentHistory] ALTER COLUMN [LastDrawnSalary] decimal(14,3) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-14T11:39:25.046' FOR [CreatedAt];
GO

ALTER TABLE [kenuser].[EmploymentHistory] ADD CONSTRAINT [FK_EmploymentHistory_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeNo]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250914113925_UpdateEmploymentHistory', N'8.0.17');
GO

COMMIT;
GO

