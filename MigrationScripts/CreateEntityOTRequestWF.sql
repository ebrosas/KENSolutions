BEGIN TRANSACTION;
GO

CREATE TABLE [kenuser].[OTRequestWF] (
    [ExtratimeId] bigint NOT NULL IDENTITY,
    [TS_AutoId] bigint NOT NULL,
    [WorkflowId] uniqueidentifier NOT NULL,
    [EmployeeNo] int NOT NULL,
    [EmployeeName] varchar(100) NOT NULL,
    [CostCenter] varchar(20) NOT NULL,
    [AttendanceDate] datetime NOT NULL,
    [OTReasonCode] varchar(20) NOT NULL,
    [ActionCode] varchar(20) NOT NULL,
    [OTStartTime] time NOT NULL,
    [OTEndTime] time NOT NULL,
    [ShiftPattern] varchar(20) NULL,
    [ShiftTiming] varchar(100) NULL,
    [Remarks] varchar(500) NULL,
    [StatusCode] varchar(20) NOT NULL,
    [StatusID] int NULL,
    [StatusHandlingCode] varchar(20) NULL,
    [WorkDuration] int NULL,
    [OTDuration] int NULL,
    [CreatedDate] datetime NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [CreatedUserID] varchar(50) NULL,
    [CreatedEmail] varchar(50) NULL,
    [LastUpdatedDate] datetime NULL,
    [LastUpdatedBy] int NULL,
    [LastUpdatedUserID] varchar(50) NULL,
    [LastUpdatedEmail] varchar(50) NULL,
    CONSTRAINT [PK_OTRequestWF_Id] PRIMARY KEY ([ExtratimeId])
);
GO

CREATE UNIQUE INDEX [IX_OTRequestWF_CompoKeys] ON [kenuser].[OTRequestWF] ([TS_AutoId], [EmployeeNo], [AttendanceDate], [OTReasonCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260606200920_CreateEntityOTRequestWF', N'8.0.17');
GO

COMMIT;
GO

