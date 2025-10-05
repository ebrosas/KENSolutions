IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'kenuser') IS NULL EXEC(N'CREATE SCHEMA [kenuser];');
GO

CREATE TABLE [kenuser].[Employee] (
    [EmployeeID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [MiddleName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Position] nvarchar(max) NOT NULL,
    [DOB] datetime2 NULL,
    [NationalityCode] nvarchar(max) NOT NULL,
    [ReligionCode] nvarchar(max) NOT NULL,
    [Gender] nvarchar(max) NOT NULL,
    [MaritalStatus] nvarchar(max) NOT NULL,
    [Salutation] nvarchar(max) NULL,
    [OfficialEmail] nvarchar(max) NOT NULL,
    [PersonalEmail] nvarchar(max) NULL,
    [AlternateEmail] nvarchar(max) NULL,
    [OfficeLandlineNo] nvarchar(max) NULL,
    [ResidenceLandlineNo] nvarchar(max) NULL,
    [OfficeExtNo] nvarchar(max) NULL,
    [MobileNo] nvarchar(max) NULL,
    [AlternateMobileNo] nvarchar(max) NULL,
    [EmployeeCode] nvarchar(max) NOT NULL,
    [EmployeeStatusID] tinyint NULL,
    [ReportingManagerCode] int NULL,
    [WorkPermitID] nvarchar(max) NULL,
    [WorkPermitExpiryDate] datetime2 NULL,
    [DateOfJoining] datetime2 NULL,
    [DateOfConfirmation] datetime2 NULL,
    [TerminationDate] datetime2 NULL,
    CONSTRAINT [PrimaryKey_EmpNo] PRIMARY KEY ([EmployeeID])
);
DECLARE @description AS sql_variant;
SET @description = N'The unique ID of the employee';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'Employee', 'COLUMN', N'EmployeeCode';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250718211332_InitialCreate', N'8.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[kenuser].[Employee].[MaritalStatus]', N'MaritalStatusCode', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250719193637_RenameMaritalStatusToMaritalStatusCode', N'8.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[kenuser].[Employee].[DateOfJoining]', N'HireDate', N'COLUMN';
GO

EXEC sp_rename N'[kenuser].[Employee].[Gender]', N'GenderCode', N'COLUMN';
GO

EXEC sp_rename N'[kenuser].[Employee].[EmployeeID]', N'EmployeeId', N'COLUMN';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'WorkPermitID');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [WorkPermitID] varchar(30) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'WorkPermitExpiryDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [WorkPermitExpiryDate] datetime NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'TerminationDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [TerminationDate] datetime NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'Salutation');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [Salutation] varchar(10) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'ResidenceLandlineNo');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [ResidenceLandlineNo] varchar(20) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'ReportingManagerCode');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [ReportingManagerCode] varchar(20) NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'ReligionCode');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [ReligionCode] varchar(20) NOT NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'Position');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [Position] varchar(100) NOT NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'PersonalEmail');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [PersonalEmail] varchar(50) NULL;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'OfficialEmail');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [OfficialEmail] varchar(50) NOT NULL;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'OfficeLandlineNo');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [OfficeLandlineNo] varchar(20) NULL;
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'OfficeExtNo');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [OfficeExtNo] varchar(10) NULL;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'NationalityCode');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [NationalityCode] varchar(20) NOT NULL;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'MobileNo');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [MobileNo] varchar(20) NULL;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'MiddleName');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [MiddleName] varchar(50) NULL;
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'MaritalStatusCode');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [MaritalStatusCode] varchar(20) NOT NULL;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'LastName');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [LastName] varchar(50) NOT NULL;
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'FirstName');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [FirstName] varchar(50) NOT NULL;
GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'EmployeeCode');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [EmployeeCode] varchar(20) NOT NULL;
GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'DateOfConfirmation');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [DateOfConfirmation] datetime NULL;
GO

DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'DOB');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [DOB] datetime NULL;
GO

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'AlternateMobileNo');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [AlternateMobileNo] varchar(20) NULL;
GO

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'AlternateEmail');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [kenuser].[Employee] ALTER COLUMN [AlternateEmail] varchar(50) NULL;
GO

DECLARE @description AS sql_variant;
SET @description = N'The primary key of the Employee entity';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'Employee', 'COLUMN', N'EmployeeId';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250719203747_RefactorEmployeeEntitySchema', N'8.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [PrimaryKey_EmpNo];
GO

DECLARE @var23 sysname;
SELECT @var23 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[Employee]') AND [c].[name] = N'EmployeeCode');
IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[Employee] DROP CONSTRAINT [' + @var23 + '];');
ALTER TABLE [kenuser].[Employee] DROP COLUMN [EmployeeCode];
GO

DECLARE @description AS sql_variant;
EXEC sp_dropextendedproperty 'MS_Description', 'SCHEMA', N'kenuser', 'TABLE', N'Employee', 'COLUMN', N'EmployeeId';
SET @description = N'Primary key for Employee entity';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'Employee', 'COLUMN', N'EmployeeId';
GO

ALTER TABLE [kenuser].[Employee] ADD [AccountHolderName] varchar(50) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [AccountNumber] varchar(30) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [AccountTypeCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [BankBranchCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [BankNameCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [Company] varchar(40) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [DateOfSuperannuation] datetime NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [EducationCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [EmployeeClassCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [EmployeeNo] int NOT NULL DEFAULT 0;
DECLARE @description AS sql_variant;
SET @description = N'Alternate key used for reference navigation';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'Employee', 'COLUMN', N'EmployeeNo';
GO

ALTER TABLE [kenuser].[Employee] ADD [FacebookAccount] varchar(40) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [IBANNumber] varchar(30) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [InstagramAccount] varchar(40) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [JobTitle] varchar(40) NOT NULL DEFAULT '';
GO

ALTER TABLE [kenuser].[Employee] ADD [LinkedInAccount] varchar(40) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [OldEmployeeNo] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PayGrade] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);
GO

ALTER TABLE [kenuser].[Employee] ADD [PermanentAddress] varchar(300) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PermanentAreaCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PermanentCityCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PermanentContactNo] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PermanentCountryCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PermanentMobileNo] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PresentAddress] varchar(300) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PresentAreaCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PresentCityCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PresentContactNo] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PresentCountryCode] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [PresentMobileNo] varchar(20) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [Reemployed] bit NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [TaxNumber] varchar(40) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD [TwitterAccount] varchar(40) NULL;
GO

ALTER TABLE [kenuser].[Employee] ADD CONSTRAINT [PK_Employee_EmployeeId] PRIMARY KEY ([EmployeeId]);
GO

CREATE TABLE [kenuser].[EmergencyContact] (
    [AutoId] int NOT NULL IDENTITY,
    [ContactPerson] varchar(200) NOT NULL,
    [RelationCode] varchar(20) NOT NULL,
    [MobileNo] varchar(20) NOT NULL,
    [LandlineNo] varchar(20) NULL,
    [Address] varchar(300) NULL,
    [CountryCode] varchar(20) NULL,
    [CityCode] varchar(20) NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_EmergencyContact_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_EmergencyContact_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmergencyContact', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmergencyContact', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[EmployeeCertification] (
    [AutoId] int NOT NULL IDENTITY,
    [QualificationCode] varchar(20) NOT NULL,
    [Stream] varchar(100) NULL,
    [Specialization] varchar(150) NOT NULL,
    [University] varchar(150) NOT NULL,
    [Institute] varchar(100) NULL,
    [CountryCode] varchar(20) NULL,
    [StateCode] varchar(20) NULL,
    [CityCode] varchar(20) NULL,
    [FromMonthCode] varchar(20) NOT NULL,
    [FromYear] int NOT NULL,
    [ToMonthCode] varchar(20) NOT NULL,
    [ToYear] int NOT NULL,
    [PassMonthCode] varchar(20) NOT NULL,
    [PassYear] int NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_EmployeeCertification_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_EmployeeCertification_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Part of unique composite key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeCertification', 'COLUMN', N'QualificationCode';
SET @description = N'Part of unique composite key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeCertification', 'COLUMN', N'FromMonthCode';
SET @description = N'Part of unique composite key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeCertification', 'COLUMN', N'FromYear';
SET @description = N'Part of unique composite key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeCertification', 'COLUMN', N'ToMonthCode';
SET @description = N'Part of unique composite key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeCertification', 'COLUMN', N'ToYear';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeCertification', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references Employee.EmployeeNo alternate key';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeCertification', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[EmployeeSkill] (
    [AutoId] int NOT NULL IDENTITY,
    [SkillName] varchar(50) NOT NULL,
    [LevelCode] varchar(20) NULL,
    [LastUsedMonthCode] varchar(20) NULL,
    [LastUsedYear] int NULL,
    [FromMonthCode] varchar(20) NULL,
    [FromYear] int NULL,
    [ToMonthCode] varchar(20) NULL,
    [ToYear] int NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_EmployeeSkill_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_EmployeeSkill_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeSkill', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeSkill', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[EmployeeTransaction] (
    [ActionCode] varchar(20) NOT NULL,
    [SectionCode] varchar(20) NOT NULL,
    [TransactionNo] int NOT NULL,
    [AutoId] int NOT NULL,
    [StatusCode] varchar(20) NOT NULL,
    [LastUpdateOn] datetime NULL,
    [CurrentlyAssignedEmpNo] int NULL,
    [CurrentlyAssignedEmpName] varchar(150) NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_EmployeeTransaction_CompKey] PRIMARY KEY ([ActionCode], [SectionCode], [TransactionNo]),
    CONSTRAINT [FK_EmployeeTransaction_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeTransaction', 'COLUMN', N'ActionCode';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeTransaction', 'COLUMN', N'SectionCode';
SET @description = N'Foreign key that references employee transaction. Part of composite key.';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeTransaction', 'COLUMN', N'TransactionNo';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeTransaction', 'COLUMN', N'StatusCode';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeTransaction', 'COLUMN', N'LastUpdateOn';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmployeeTransaction', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[EmploymentHistory] (
    [AutoId] int NOT NULL IDENTITY,
    [CompanyName] varchar(150) NOT NULL,
    [CompanyAddress] varchar(300) NULL,
    [Designation] varchar(100) NOT NULL,
    [Role] varchar(100) NULL,
    [FromDate] datetime NULL,
    [ToDate] datetime NULL,
    [LastDrawnSalary] nvarchar(max) NULL,
    [SalaryTypeCode] varchar(20) NULL,
    [SalaryCurrencyCode] varchar(20) NULL,
    [ReasonOfChange] varchar(200) NULL,
    [ReportingManager] varchar(150) NULL,
    [CompanyWebsite] varchar(100) NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_EmploymentHistory_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_EmploymentHistory_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmploymentHistory', 'COLUMN', N'CompanyName';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmploymentHistory', 'COLUMN', N'Designation';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmploymentHistory', 'COLUMN', N'FromDate';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmploymentHistory', 'COLUMN', N'ToDate';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmploymentHistory', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'EmploymentHistory', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[FamilyMember] (
    [AutoId] int NOT NULL IDENTITY,
    [FirstName] varchar(50) NOT NULL,
    [MiddleName] varchar(50) NULL,
    [LastName] varchar(50) NOT NULL,
    [RelationCode] varchar(20) NOT NULL,
    [DOB] datetime NULL,
    [QualificationCode] varchar(20) NULL,
    [StreamCode] varchar(20) NULL,
    [SpecializationCode] varchar(20) NULL,
    [Occupation] varchar(120) NULL,
    [ContactNo] varchar(20) NULL,
    [CountryCode] varchar(20) NULL,
    [StateCode] varchar(20) NULL,
    [CityCode] varchar(20) NULL,
    [District] varchar(100) NULL,
    [IsDependent] bit NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_FamilyMember_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_FamilyMember_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyMember', 'COLUMN', N'FirstName';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyMember', 'COLUMN', N'LastName';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyMember', 'COLUMN', N'RelationCode';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyMember', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyMember', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[FamilyVisa] (
    [AutoId] int NOT NULL IDENTITY,
    [CountryCode] varchar(20) NOT NULL,
    [VisaTypeCode] varchar(20) NOT NULL,
    [Profession] varchar(150) NOT NULL,
    [IssueDate] datetime NULL,
    [ExpiryDate] datetime NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_FamilyVisa_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_FamilyVisa_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyVisa', 'COLUMN', N'CountryCode';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyVisa', 'COLUMN', N'VisaTypeCode';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyVisa', 'COLUMN', N'IssueDate';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyVisa', 'COLUMN', N'ExpiryDate';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyVisa', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'FamilyVisa', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[IdentityProof] (
    [AutoId] int NOT NULL IDENTITY,
    [PassportNumber] varchar(20) NULL,
    [DateOfIssue] datetime NULL,
    [DateOfExpiry] datetime NULL,
    [PlaceOfIssue] varchar(100) NULL,
    [DrivingLicenseNo] varchar(20) NULL,
    [DLDateOfIssue] datetime NULL,
    [DLDateOfExpiry] datetime NULL,
    [DLPlaceOfIssue] varchar(50) NULL,
    [TypeOfDocument] varchar(50) NULL,
    [OtherDocNumber] varchar(30) NULL,
    [OtherDocDateOfIssue] datetime NULL,
    [OtherDocDateOfExpiry] datetime NULL,
    [NationalIDNumber] varchar(40) NULL,
    [NationalIDTypeCode] varchar(20) NULL,
    [NatIDPlaceOfIssue] varchar(50) NULL,
    [NatIDDateOfIssue] datetime NULL,
    [NatIDDateOfExpiry] datetime NULL,
    [ContractNumber] varchar(30) NULL,
    [ContractPlaceOfIssue] varchar(100) NULL,
    [ContractDateOfIssue] datetime NULL,
    [ContractDateOfExpiry] datetime NULL,
    [VisaNumber] varchar(30) NULL,
    [VisaTypeCode] varchar(20) NULL,
    [VisaCountryCode] varchar(20) NULL,
    [Profession] varchar(100) NULL,
    [Sponsor] varchar(100) NULL,
    [VisaDateOfIssue] datetime NULL,
    [VisaDateOfExpiry] datetime NULL,
    [EmployeeNo] int NOT NULL,
    [TransactionNo] int NULL,
    CONSTRAINT [PK_IdentityProof_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_IdentityProof_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'IdentityProof', 'COLUMN', N'EmployeeNo';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'IdentityProof', 'COLUMN', N'TransactionNo';
GO

CREATE TABLE [kenuser].[LanguageSkill] (
    [AutoId] int NOT NULL IDENTITY,
    [LanguageCode] varchar(20) NOT NULL,
    [CanWrite] bit NULL,
    [CanSpeak] bit NULL,
    [CanRead] bit NULL,
    [MotherTongue] bit NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_LanguageSkill_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_LanguageSkill_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'LanguageSkill', 'COLUMN', N'LanguageCode';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'LanguageSkill', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'LanguageSkill', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[OtherDocument] (
    [AutoId] int NOT NULL IDENTITY,
    [DocumentName] varchar(150) NOT NULL,
    [DocumentTypeCode] varchar(20) NOT NULL,
    [Description] varchar(300) NULL,
    [FileData] varbinary(max) NULL,
    [FileExtension] nvarchar(max) NULL,
    [ContentType] nvarchar(max) NULL,
    [UploadDate] datetime NULL,
    [TransactionNo] int NULL,
    [EmployeeNo] int NOT NULL,
    CONSTRAINT [PK_OtherDocument_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_OtherDocument_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'OtherDocument', 'COLUMN', N'DocumentName';
SET @description = N'Part of composite unique key index';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'OtherDocument', 'COLUMN', N'DocumentTypeCode';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'OtherDocument', 'COLUMN', N'TransactionNo';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'OtherDocument', 'COLUMN', N'EmployeeNo';
GO

CREATE TABLE [kenuser].[Qualification] (
    [AutoId] int NOT NULL IDENTITY,
    [QualificationCode] varchar(20) NOT NULL,
    [StreamCode] varchar(20) NULL,
    [SpecializationCode] varchar(20) NULL,
    [UniversityName] varchar(200) NOT NULL,
    [Institute] varchar(100) NULL,
    [QualificationMode] varchar(20) NOT NULL,
    [CountryCode] varchar(20) NULL,
    [StateCode] varchar(20) NULL,
    [CityCode] varchar(20) NULL,
    [FromMonthCode] varchar(20) NOT NULL,
    [FromYear] int NOT NULL,
    [ToMonthCode] varchar(20) NOT NULL,
    [ToYear] int NOT NULL,
    [PassMonthCode] varchar(20) NOT NULL,
    [PassYear] int NOT NULL,
    [EmployeeNo] int NOT NULL,
    [TransactionNo] int NULL,
    CONSTRAINT [PK_Qualification_AutoId] PRIMARY KEY ([AutoId]),
    CONSTRAINT [FK_Qualification_Employee_EmployeeNo] FOREIGN KEY ([EmployeeNo]) REFERENCES [kenuser].[Employee] ([EmployeeId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'Qualification', 'COLUMN', N'EmployeeNo';
SET @description = N'Unique ID number that is generated when a request requires approval';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'Qualification', 'COLUMN', N'TransactionNo';
GO

CREATE INDEX [IX_Employee_Attribute] ON [kenuser].[Employee] ([EmployeeNo], [NationalityCode], [ReligionCode], [MaritalStatusCode]);
GO

CREATE INDEX [IX_Employee_Date] ON [kenuser].[Employee] ([EmployeeNo] DESC, [HireDate] DESC, [TerminationDate] DESC, [DateOfConfirmation] DESC, [DOB] DESC);
GO

CREATE INDEX [IX_Employee_EmpName] ON [kenuser].[Employee] ([EmployeeNo], [FirstName], [MiddleName], [LastName]);
GO

CREATE UNIQUE INDEX [IX_EmergencyContact_CompoKeys] ON [kenuser].[EmergencyContact] ([EmployeeNo], [ContactPerson], [RelationCode], [MobileNo]);
GO

CREATE UNIQUE INDEX [IX_EmployeeCertification_CompoKeys] ON [kenuser].[EmployeeCertification] ([EmployeeNo], [QualificationCode], [FromMonthCode], [FromYear], [ToMonthCode], [ToYear]);
GO

CREATE UNIQUE INDEX [IX_EmployeeSkill_CompoKeys] ON [kenuser].[EmployeeSkill] ([EmployeeNo], [SkillName]);
GO

CREATE UNIQUE INDEX [IX_EmployeeTransaction_CompoKeys] ON [kenuser].[EmployeeTransaction] ([EmployeeNo], [ActionCode], [SectionCode], [StatusCode], [LastUpdateOn]);
GO

CREATE UNIQUE INDEX [IX_EmploymentHistory_CompoKeys] ON [kenuser].[EmploymentHistory] ([EmployeeNo], [CompanyName], [Designation], [FromDate], [ToDate]);
GO

CREATE UNIQUE INDEX [IX_FamilyMember_CompoKeys] ON [kenuser].[FamilyMember] ([EmployeeNo], [FirstName], [LastName], [RelationCode]);
GO

CREATE UNIQUE INDEX [IX_FamilyVisa_CompoKeys] ON [kenuser].[FamilyVisa] ([EmployeeNo], [VisaTypeCode], [CountryCode], [IssueDate], [ExpiryDate]);
GO

CREATE INDEX [IX_IdentityProof_ContractDetail] ON [kenuser].[IdentityProof] ([EmployeeNo], [ContractNumber], [ContractDateOfIssue], [ContractDateOfExpiry]);
GO

CREATE UNIQUE INDEX [IX_IdentityProof_EmployeeNo] ON [kenuser].[IdentityProof] ([EmployeeNo]);
GO

CREATE UNIQUE INDEX [IX_IdentityProof_NatlIDDetail] ON [kenuser].[IdentityProof] ([EmployeeNo], [NationalIDNumber], [NationalIDTypeCode]);
GO

CREATE UNIQUE INDEX [IX_IdentityProof_PassportInfo] ON [kenuser].[IdentityProof] ([EmployeeNo], [PassportNumber], [DateOfIssue], [DateOfExpiry]) WHERE [PassportNumber] IS NOT NULL AND [DateOfIssue] IS NOT NULL AND [DateOfExpiry] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_IdentityProof_VisaDetail] ON [kenuser].[IdentityProof] ([EmployeeNo], [VisaCountryCode], [VisaNumber], [VisaTypeCode]);
GO

CREATE UNIQUE INDEX [IX_LanguageSkill_CompoKeys] ON [kenuser].[LanguageSkill] ([EmployeeNo], [LanguageCode]);
GO

CREATE UNIQUE INDEX [IX_OtherDocument_CompoKeys] ON [kenuser].[OtherDocument] ([EmployeeNo], [DocumentName], [DocumentTypeCode]);
GO

CREATE UNIQUE INDEX [IX_Qualification_CompoKeys] ON [kenuser].[Qualification] ([EmployeeNo], [QualificationCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250818202847_UpdateDataModel', N'8.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [kenuser].[UserDefinedCodeGroup] (
    [UDCGroupId] int NOT NULL IDENTITY,
    [UDCGCode] varchar(20) NOT NULL,
    [UDCGDesc1] varchar(150) NOT NULL,
    [UDCGDesc2] varchar(150) NULL,
    [UDCGSpecialHandlingCode] varchar(20) NULL,
    CONSTRAINT [PK_UserDefinedCodeGroup_UDCGroupId] PRIMARY KEY ([UDCGroupId])
);
DECLARE @description AS sql_variant;
SET @description = N'Primary key of UserDefinedCodeGroup entity';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'UserDefinedCodeGroup', 'COLUMN', N'UDCGroupId';
GO

CREATE TABLE [kenuser].[UserDefinedCode] (
    [UDCId] int NOT NULL IDENTITY,
    [UDCCode] varchar(20) NOT NULL,
    [UDCDesc1] varchar(150) NOT NULL,
    [UDCDesc2] varchar(150) NULL,
    [UDCSpecialHandlingCode] varchar(20) NULL,
    [SequenceNo] int NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [Amount] decimal(13,3) NULL,
    [GroupID] int NOT NULL,
    CONSTRAINT [PK_UserDefinedCode_UDCId] PRIMARY KEY ([UDCId]),
    CONSTRAINT [FK_UserDefinedCode_UserDefinedCodeGroup_GroupID] FOREIGN KEY ([GroupID]) REFERENCES [kenuser].[UserDefinedCodeGroup] ([UDCGroupId]) ON DELETE CASCADE
);
DECLARE @description AS sql_variant;
SET @description = N'Primary key of UserDefinedCode entity';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'UserDefinedCode', 'COLUMN', N'UDCId';
SET @description = N'Foreign key that references alternate key: Employee.EmployeeNo';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'kenuser', 'TABLE', N'UserDefinedCode', 'COLUMN', N'GroupID';
GO

CREATE UNIQUE INDEX [IX_UserDefinedCode_CompoKeys] ON [kenuser].[UserDefinedCode] ([UDCCode], [UDCDesc1], [UDCSpecialHandlingCode]);
GO

CREATE INDEX [IX_UserDefinedCode_GroupID] ON [kenuser].[UserDefinedCode] ([GroupID]);
GO

CREATE UNIQUE INDEX [IX_UserDefinedCodeGroup_CompoKeys] ON [kenuser].[UserDefinedCodeGroup] ([UDCGCode], [UDCGDesc1], [UDCGSpecialHandlingCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250820204820_AddUserDefinedCode', N'8.0.17');
GO

COMMIT;
GO

