BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmployeeMasters]') AND [c].[name] = N'ReportingManagerCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmployeeMasters] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[EmployeeMasters] ALTER COLUMN [ReportingManagerCode] nvarchar(max) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmployeeMasters]') AND [c].[name] = N'EmployeeNo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmployeeMasters] DROP CONSTRAINT [' + @var1 + '];');
UPDATE [kenuser].[EmployeeMasters] SET [EmployeeNo] = 0 WHERE [EmployeeNo] IS NULL;
ALTER TABLE [kenuser].[EmployeeMasters] ALTER COLUMN [EmployeeNo] int NOT NULL;
ALTER TABLE [kenuser].[EmployeeMasters] ADD DEFAULT 0 FOR [EmployeeNo];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmployeeMasters]') AND [c].[name] = N'DepartmentName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmployeeMasters] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[EmployeeMasters] ALTER COLUMN [DepartmentName] nvarchar(max) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[EmployeeMasters]') AND [c].[name] = N'DepartmentCode');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[EmployeeMasters] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[EmployeeMasters] ALTER COLUMN [DepartmentCode] nvarchar(max) NULL;
GO

ALTER TABLE [kenuser].[EmployeeMasters] ADD [EmployeeId] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [kenuser].[DepartmentMaster] (
    [DepartmentId] int NOT NULL IDENTITY,
    [DepartmentCode] varchar(20) NOT NULL,
    [DepartmentName] varchar(120) NOT NULL,
    [GroupCode] varchar(20) NULL,
    [Description] varchar(150) NULL,
    [ParentDepartmentId] int NULL,
    [SuperintendentEmpNo] int NULL,
    [ManagerEmpNo] int NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime NOT NULL,
    [UpdatedAt] datetime NULL,
    CONSTRAINT [PK_DepartmentMaster_DeptId] PRIMARY KEY ([DepartmentId])
);
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'DepartmentMaster', 'COLUMN', N'DepartmentCode';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'DepartmentMaster', 'COLUMN', N'DepartmentName';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'DepartmentMaster', 'COLUMN', N'GroupCode';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'DepartmentMaster', 'COLUMN', N'SuperintendentEmpNo';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'DepartmentMaster', 'COLUMN', N'ManagerEmpNo';
GO

CREATE UNIQUE INDEX [IX_DepartmentMaster_CompoKeys] ON [kenuser].[DepartmentMaster] ([DepartmentCode], [DepartmentName], [GroupCode], [SuperintendentEmpNo], [ManagerEmpNo]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250827131822_AddDepartmentMaster', N'8.0.17');
GO

COMMIT;
GO

