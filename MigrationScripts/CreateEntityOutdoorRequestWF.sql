BEGIN TRANSACTION;
GO

CREATE TABLE [kenuser].[OutdoorRequestWF] (
    [OutdoorId] bigint NOT NULL IDENTITY,
    [AttachmentId] uniqueidentifier NOT NULL,
    [WorkflowId] uniqueidentifier NOT NULL,
    [EmpNo] int NOT NULL,
    [EmpName] varchar(100) NOT NULL,
    [StartDate] datetime NOT NULL,
    [EndDate] datetime NOT NULL,
    [ROACode] varchar(20) NOT NULL,
    [DOWCode] nvarchar(max) NULL,
    [Description] varchar(500) NOT NULL,
    [ActionCode] varchar(20) NOT NULL,
    [StartTime] time NULL,
    [EndTime] time NULL,
    [StatusID] int NULL,
    [StatusCode] varchar(20) NOT NULL,
    [StatusHandlingCode] varchar(20) NULL,
    [CostCenter] varchar(20) NULL,
    [CreatedDate] datetime NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [CreatedUserID] varchar(50) NULL,
    [CreatedEmail] varchar(50) NULL,
    [LastUpdatedDate] datetime NULL,
    [LastUpdatedBy] int NULL,
    [LastUpdatedUserID] varchar(50) NULL,
    [LastUpdatedEmail] varchar(50) NULL,
    CONSTRAINT [PK_OutdoorRequestWF_Id] PRIMARY KEY ([OutdoorId]),
    CONSTRAINT [AK_OutdoorRequestWF_AttachmentId] UNIQUE ([AttachmentId])
);
GO

CREATE UNIQUE INDEX [IX_OutdoorRequestWF_CompoKeys] ON [kenuser].[OutdoorRequestWF] ([EmpNo], [StartDate], [EndDate], [ROACode], [ActionCode]);
GO

ALTER TABLE [kenuser].[FileAttachments] ADD CONSTRAINT [FK_FileAttachments_OutdoorRequestWF_AttachmentId] FOREIGN KEY ([AttachmentId]) REFERENCES [kenuser].[OutdoorRequestWF] ([AttachmentId]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260628201627_CreateEntityOutdoorRequestWF', N'8.0.17');
GO

COMMIT;
GO

