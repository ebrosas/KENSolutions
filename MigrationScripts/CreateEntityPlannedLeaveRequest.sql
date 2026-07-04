BEGIN TRANSACTION;
GO

CREATE TABLE [kenuser].[PlannedLeaveRequest] (
    [PlannedLeaveId] bigint NOT NULL IDENTITY,
    [EmpNo] int NOT NULL,
    [EmpName] varchar(150) NULL,
    [LeaveStartDate] datetime NOT NULL,
    [LeaveEndDate] datetime NOT NULL,
    [LeaveResumeDate] datetime NOT NULL,
    [CostCenter] varchar(20) NULL,
    [Remarks] varchar(500) NULL,
    [StartDayMode] varchar(20) NULL,
    [EndDayMode] varchar(20) NULL,
    [LeaveDuration] float NULL,
    [NoOfHolidays] int NULL,
    [NoOfWeekends] int NULL,
    [HalfDayLeaveFlag] char(1) NULL,
    [CreatedDate] datetime NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [CreatedByName] varchar(100) NULL,
    [CreatedUserID] varchar(50) NULL,
    [CreatedEmail] varchar(50) NULL,
    [LastUpdatedDate] datetime NULL,
    [LastUpdatedBy] int NULL,
    [LastUpdatedName] varchar(100) NULL,
    [LastUpdatedUserID] varchar(50) NULL,
    [LastUpdatedEmail] varchar(50) NULL,
    [StatusCode] varchar(20) NOT NULL,
    [StatusID] int NULL,
    [StatusHandlingCode] varchar(20) NULL,
    CONSTRAINT [PK_PlannedLeaveRequest_Id] PRIMARY KEY ([PlannedLeaveId])
);
GO

CREATE UNIQUE INDEX [IX_PlannedLeaveRequest_CompoKeys] ON [kenuser].[PlannedLeaveRequest] ([EmpNo], [LeaveStartDate], [LeaveResumeDate]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260704192308_CreateEntityPlannedLeaveRequest', N'8.0.17');
GO

COMMIT;
GO

