

    BEGIN TRAN T1

    INSERT INTO [kenuser].[EmployeeSkill]
           ([SkillName]
           ,[LevelCode]
           ,[LastUsedMonthCode]
           ,[LastUsedYear]
           ,[FromMonthCode]
           ,[FromYear]
           ,[ToMonthCode]
           ,[ToYear]
           ,[TransactionNo]
           ,[EmployeeNo])
    SELECT 'C#.Net' AS [SkillName]
          ,'EXPERT' AS [LevelCode]
          ,'SEP' AS [LastUsedMonthCode]
          ,2025 AS [LastUsedYear]
          ,'JAN' AS [FromMonthCode]
          ,2011 AS [FromYear]
          ,'AUG' AS [ToMonthCode]
          ,2025 AS [ToYear]
          ,NULL AS [TransactionNo]
          ,10003632 [EmployeeNo]

    UNION

    SELECT 'Entity Framework Core' AS [SkillName]
          ,'ADVANCED' AS [LevelCode]
          ,'MAY' AS [LastUsedMonthCode]
          ,2025 AS [LastUsedYear]
          ,'MAR' AS [FromMonthCode]
          ,2020 AS [FromYear]
          ,'APR' AS [ToMonthCode]
          ,2025 AS [ToYear]
          ,NULL AS [TransactionNo]
          ,10003632 [EmployeeNo]

    UNION

    SELECT 'Blazor WebApp' AS [SkillName]
          ,'INTERMEDIATE' AS [LevelCode]
          ,'SEP' AS [LastUsedMonthCode]
          ,2025 AS [LastUsedYear]
          ,'JUN' AS [FromMonthCode]
          ,2024 AS [FromYear]
          ,'AUG' AS [ToMonthCode]
          ,2025 AS [ToYear]
          ,NULL AS [TransactionNo]
          ,10003632 [EmployeeNo]


    ROLLBACK TRAN T1
    --COMMIT TRAN T1


