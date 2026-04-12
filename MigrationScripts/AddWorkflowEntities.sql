BEGIN TRANSACTION;
GO

CREATE TABLE [kenuser].[WorkflowApprovalRoles] (
    [ApprovalRoleId] int NOT NULL IDENTITY,
    [ApprovalGroupCode] varchar(50) NOT NULL,
    [ApprovalGroupDesc] varchar(200) NOT NULL,
    [Remarks] varchar(300) NULL,
    [AssigneeEmpNo] int NOT NULL,
    [AssigneEmpName] varchar(100) NULL,
    [AssigneEmail] varchar(50) NULL,
    [SubstituteEmpNo] int NULL,
    [SubstituteEmpName] varchar(100) NULL,
    [SubstituteEmail] varchar(50) NULL,
    [CreatedDate] datetime NULL DEFAULT '2026-04-12T15:30:48.386',
    [CreatedBy] int NULL,
    [CreatedUserID] varchar(50) NULL,
    [LastUpdatedDate] datetime NULL,
    [LastUpdatedBy] int NULL,
    [LastUpdatedUserID] varchar(50) NULL,
    CONSTRAINT [PK_WorkflowApprovalRole_ApprovalRoleId] PRIMARY KEY ([ApprovalRoleId])
);
GO

CREATE TABLE [kenuser].[WorkflowDefinitions] (
    [WorkflowDefinitionId] int NOT NULL IDENTITY,
    [Name] varchar(150) NOT NULL,
    [EntityName] varchar(100) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_WorkflowDefinition_WorkflowDefinitionId] PRIMARY KEY ([WorkflowDefinitionId])
);
GO

CREATE TABLE [kenuser].[WorkflowInstances] (
    [WorkflowInstanceId] int NOT NULL IDENTITY,
    [WorkflowDefinitionId] int NOT NULL,
    [EntityId] bigint NOT NULL,
    [Status] varchar(50) NOT NULL,
    CONSTRAINT [PK_WorkflowInstance_WorkflowInstanceId] PRIMARY KEY ([WorkflowInstanceId]),
    CONSTRAINT [FK_WorkflowInstances_WorkflowDefinitions_WorkflowDefinitionId] FOREIGN KEY ([WorkflowDefinitionId]) REFERENCES [kenuser].[WorkflowDefinitions] ([WorkflowDefinitionId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [kenuser].[WorkflowStepDefinitions] (
    [StepDefinitionId] int NOT NULL IDENTITY,
    [WorkflowDefinitionId] int NOT NULL,
    [StepName] varchar(200) NOT NULL,
    [StepOrder] int NOT NULL,
    [ApprovalRole] varchar(50) NOT NULL,
    [IsParallelGroup] bit NOT NULL,
    [ParallelGroupId] uniqueidentifier NULL,
    [RequiresAllParallel] bit NOT NULL,
    CONSTRAINT [PK_WorkflowStepDefinition_StepDefinitionId] PRIMARY KEY ([StepDefinitionId]),
    CONSTRAINT [FK_WorkflowStepDefinitions_WorkflowDefinitions_WorkflowDefinitionId] FOREIGN KEY ([WorkflowDefinitionId]) REFERENCES [kenuser].[WorkflowDefinitions] ([WorkflowDefinitionId]) ON DELETE CASCADE
);
GO

CREATE TABLE [kenuser].[WorkflowConditions] (
    [ConditionId] int NOT NULL IDENTITY,
    [StepDefinitionId] int NOT NULL,
    [FieldName] varchar(100) NOT NULL,
    [Operator] varchar(20) NOT NULL,
    [CompareValue] varchar(50) NOT NULL,
    [NextStepDefinitionId] int NOT NULL,
    CONSTRAINT [PK_WorkflowCondition_ConditionId] PRIMARY KEY ([ConditionId]),
    CONSTRAINT [FK_WorkflowConditions_WorkflowStepDefinitions_NextStepDefinitionId] FOREIGN KEY ([NextStepDefinitionId]) REFERENCES [kenuser].[WorkflowStepDefinitions] ([StepDefinitionId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_WorkflowConditions_WorkflowStepDefinitions_StepDefinitionId] FOREIGN KEY ([StepDefinitionId]) REFERENCES [kenuser].[WorkflowStepDefinitions] ([StepDefinitionId]) ON DELETE CASCADE
);
GO

CREATE TABLE [kenuser].[WorkflowStepInstances] (
    [StepInstanceId] int NOT NULL IDENTITY,
    [WorkflowInstanceId] int NOT NULL,
    [StepDefinitionId] int NOT NULL,
    [ApproverEmpNo] int NOT NULL,
    [ApproverUserID] nvarchar(max) NULL,
    [ApproverRole] varchar(100) NOT NULL,
    [Status] varchar(50) NOT NULL,
    [ActionDate] datetime2 NULL DEFAULT (GETDATE()),
    [Comments] varchar(300) NULL,
    CONSTRAINT [PK_WorkflowStepInstance_StepInstanceId] PRIMARY KEY ([StepInstanceId]),
    CONSTRAINT [FK_WorkflowStepInstances_WorkflowInstances_WorkflowInstanceId] FOREIGN KEY ([WorkflowInstanceId]) REFERENCES [kenuser].[WorkflowInstances] ([WorkflowInstanceId]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkflowStepInstances_WorkflowStepDefinitions_StepDefinitionId] FOREIGN KEY ([StepDefinitionId]) REFERENCES [kenuser].[WorkflowStepDefinitions] ([StepDefinitionId]) ON DELETE NO ACTION
);
GO

CREATE UNIQUE INDEX [IX_WorkflowApprovalRole_CompoKeys] ON [kenuser].[WorkflowApprovalRoles] ([ApprovalGroupCode]);
GO

CREATE INDEX [IX_WorkflowCondition_CompoKeys] ON [kenuser].[WorkflowConditions] ([StepDefinitionId], [FieldName]);
GO

CREATE INDEX [IX_WorkflowConditions_NextStepDefinitionId] ON [kenuser].[WorkflowConditions] ([NextStepDefinitionId]);
GO

CREATE INDEX [IX_WorkflowDefinition_UniqueKey] ON [kenuser].[WorkflowDefinitions] ([EntityName]);
GO

CREATE UNIQUE INDEX [IX_WorkflowInstance_CompoKeys] ON [kenuser].[WorkflowInstances] ([WorkflowDefinitionId], [EntityId], [Status]);
GO

CREATE UNIQUE INDEX [IX_WorkflowStepDefinition_CompoKeys] ON [kenuser].[WorkflowStepDefinitions] ([WorkflowDefinitionId], [StepOrder], [ApprovalRole]);
GO

CREATE UNIQUE INDEX [IX_WorkflowStepInstance_CompoKeys] ON [kenuser].[WorkflowStepInstances] ([WorkflowInstanceId], [StepDefinitionId], [ApproverEmpNo], [ApproverRole]);
GO

CREATE INDEX [IX_WorkflowStepInstances_StepDefinitionId] ON [kenuser].[WorkflowStepInstances] ([StepDefinitionId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260412123050_AddWorkflowEntities', N'8.0.17');
GO

COMMIT;
GO

