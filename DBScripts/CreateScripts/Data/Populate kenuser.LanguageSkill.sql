
    BEGIN TRAN T1

    INSERT INTO [kenuser].[LanguageSkill]
           ([LanguageCode]
           ,[CanWrite]
           ,[CanSpeak]
           ,[CanRead]
           ,[MotherTongue]
           ,[TransactionNo]
           ,[EmployeeNo])
     SELECT 'TG' AS [LanguageCode]
          ,1 AS [CanWrite]
          ,1 AS [CanSpeak]
          ,1 AS [CanRead]
          ,1 AS [MotherTongue]
          ,NULL AS [TransactionNo]
          ,10003632 AS [EmployeeNo]

    UNION

    SELECT 'EN' AS [LanguageCode]
          ,1 AS [CanWrite]
          ,1 AS [CanSpeak]
          ,1 AS [CanRead]
          ,0 AS [MotherTongue]
          ,NULL AS [TransactionNo]
          ,10003632 AS [EmployeeNo]

    UNION

    SELECT 'AR' AS [LanguageCode]
          ,0 AS [CanWrite]
          ,1 AS [CanSpeak]
          ,0 AS [CanRead]
          ,0 AS [MotherTongue]
          ,NULL AS [TransactionNo]
          ,10003632 AS [EmployeeNo]

    ROLLBACK TRAN T1
    --COMMIT TRAN T1


