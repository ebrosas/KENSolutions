BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[FamilyMember] DROP CONSTRAINT [FK_FamilyMember_Employee_EmployeeNo];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[FamilyMember]') AND [c].[name] = N'CityCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[FamilyMember] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[FamilyMember] DROP COLUMN [CityCode];
GO

ALTER TABLE [kenuser].[FamilyVisa] ADD [FamilyId] int NOT NULL DEFAULT 0;
DECLARE @description AS sql_variant;
SET @description = N'Foreign key that references primary key: FamilyMember.AutoId';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyVisa', 'COLUMN', N'FamilyId';
GO

ALTER TABLE [kenuser].[FamilyMember] ADD [CityTownName] varchar(100) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-09-13T14:38:25.914' FOR [CreatedAt];
GO

ALTER TABLE [kenuser].[FamilyMember] ADD CONSTRAINT [FK_FamilyMember_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeNo]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250913143826_UpdateFamilyMember', N'8.0.17');
GO

COMMIT;
GO

