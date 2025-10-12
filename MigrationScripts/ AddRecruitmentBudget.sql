BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[kenuser].[DepartmentMaster]') AND [c].[name] = N'CreatedAt');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [kenuser].[DepartmentMaster] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [kenuser].[DepartmentMaster] ADD DEFAULT '2025-10-12T12:10:02.832' FOR [CreatedAt];
GO

CREATE TABLE [kenuser].[RecruitmentBudget] (
    [BudgetId] int NOT NULL IDENTITY,
    [DepartmentCode] varchar(20) NOT NULL,
    [BudgetHeadCount] int NOT NULL,
    [ActiveCount] int NOT NULL,
    [ExitCount] int NOT NULL,
    [RequisitionCount] int NOT NULL,
    [NetGapCount] int NOT NULL,
    [Remarks] varchar(300) NULL,
    [CreatedDate] datetime NULL DEFAULT '2025-10-12T15:10:02.833',
    [LastUpdateDate] datetime NULL,
    CONSTRAINT [PK_RecruitmentBudget_BudgetId] PRIMARY KEY ([BudgetId])
);
GO

CREATE UNIQUE INDEX [IX_RecruitmentBudget_CompoKeys] ON [kenuser].[RecruitmentBudget] ([DepartmentCode], [BudgetHeadCount]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251012121004_AddRecruitmentBudget', N'8.0.17');
GO

COMMIT;
GO

