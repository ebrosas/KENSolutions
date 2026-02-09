BEGIN TRANSACTION;
GO

--DECLARE @var0 sysname;
--SELECT @var0 = [d].[name]
--FROM [sys].[default_constraints] [d]
--INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
--WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AttendanceTimesheet]') AND [c].[name] = N'TimesheetId');
--IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AttendanceTimesheet] DROP CONSTRAINT [' + @var0 + '];');

--ALTER TABLE [kenuser].[AttendanceTimesheet] DROP CONSTRAINT [PK_AttendanceTimesheet_TimesheetId];
--GO 

ALTER TABLE [kenuser].[AttendanceTimesheet] DROP COLUMN [TimesheetId];
GO

ALTER TABLE [kenuser].[AttendanceTimesheet] ADD [TimesheetId] bigint NOT NULL IDENTITY;
GO

ALTER TABLE [kenuser].[AttendanceTimesheet] 
ADD CONSTRAINT PK_AttendanceTimesheet_TimesheetId PRIMARY KEY ([TimesheetId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260209132827_ModifyAttendanceTimesheet', N'8.0.17');
GO

COMMIT;
GO

