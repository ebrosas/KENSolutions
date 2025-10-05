BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[EmergencyContact] DROP CONSTRAINT [FK_EmergencyContact_Employee_EmployeeNo];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmergencyContact]') AND [c].[name] = N'CityCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmergencyContact] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[EmergencyContact] DROP COLUMN [CityCode];
GO

ALTER TABLE [kenuser].[EmergencyContact] ADD [City] varchar(100) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-09T12:32:50.169' FOR [CreatedAt];
GO

ALTER TABLE [kenuser].[Employee] ADD CONSTRAINT [AK_Employee_EmployeeNo] UNIQUE ([EmployeeNo]);
GO

ALTER TABLE [kenuser].[EmergencyContact] ADD CONSTRAINT [FK_EmergencyContact_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeNo]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250909123250_UpdateEmergencyContactEntity', N'8.0.17');
GO

COMMIT;
GO

