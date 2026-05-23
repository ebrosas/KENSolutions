BEGIN TRANSACTION;
GO

CREATE TABLE [kenuser].[RegularRequestWFs] (
    [RegularizationId] bigint NOT NULL IDENTITY,
    [AttachmentId] uniqueidentifier NOT NULL,
    [WorkflowId] uniqueidentifier NOT NULL,
    [EmployeeNo] int NOT NULL,
    [EmployeeName] varchar(100) NOT NULL,
    [AttendanceDate] datetime NOT NULL,
    [ROACode] varchar(20) NOT NULL,
    [ActionCode] varchar(20) NOT NULL,
    [RegularizedTimeIn] time NOT NULL,
    [RegularizedTimeOut] time NOT NULL,
    [ShiftPattern] varchar(20) NULL,
    [RegularizedDescription] varchar(500) NOT NULL,
    [CreatedDate] datetime NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [CreatedUserID] varchar(50) NULL,
    [CreatedEmail] varchar(50) NULL,
    [LastUpdatedDate] datetime NULL,
    [LastUpdatedBy] int NULL,
    [LastUpdatedUserID] varchar(50) NULL,
    [LastUpdatedEmail] varchar(50) NULL,
    CONSTRAINT [PK_RegularRequestWF_Id] PRIMARY KEY ([RegularizationId]),
    CONSTRAINT [AK_RegularRequestWFs_AttachmentId] UNIQUE ([AttachmentId])
);
GO

CREATE TABLE [kenuser].[FileAttachments] (
    [Id] uniqueidentifier NOT NULL,
    [AttachmentId] uniqueidentifier NOT NULL,
    [RequestType] varchar(20) NOT NULL,
    [FileName] varchar(100) NOT NULL,
    [StoredFileName] varchar(250) NOT NULL,
    [ContentType] varchar(50) NOT NULL,
    [FileSize] bigint NOT NULL,
    [FileData] varbinary(max) NULL,
    CONSTRAINT [PK_FileAttachment_Id] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FileAttachments_RegularRequestWFs_AttachmentId] FOREIGN KEY ([AttachmentId]) REFERENCES [kenuser].[RegularRequestWFs] ([AttachmentId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_FileAttachments_AttachmentId] ON [kenuser].[FileAttachments] ([AttachmentId]);
GO

CREATE UNIQUE INDEX [IX_RegularRequestWF_CompoKeys] ON [kenuser].[RegularRequestWFs] ([EmployeeNo], [AttendanceDate], [ROACode], [ActionCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260523202403_AddRegularRequestWF', N'8.0.17');
GO

COMMIT;
GO

