BEGIN TRANSACTION;
GO

--DECLARE @var0 sysname;
--SELECT @var0 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[FamilyMember]') AND [c].[name] = N'StateCode');
--IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[FamilyMember] DROP CONSTRAINT [' + @var0 + '];');
--ALTER TABLE [kenuser].[FamilyMember] DROP COLUMN [StateCode];
--GO

--ALTER TABLE [kenuser].[FamilyMember] ADD [StateName] varchar(100) NULL;
--GO

--INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
--VALUES (N'20260728195845_AddFamilyMemberStateName', N'8.0.17');
--GO

--COMMIT;
--GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_FamilyVisa_CompoKeys] ON [kenuser].[FamilyVisa];
GO

CREATE UNIQUE INDEX [IX_FamilyVisa_CompoKeys] ON [kenuser].[FamilyVisa] ([EmployeeNo], [FamilyId], [VisaTypeCode], [IssueDate], [ExpiryDate]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260730210347_ModifyFamilyVisaKey', N'8.0.17');
GO

COMMIT;
GO

