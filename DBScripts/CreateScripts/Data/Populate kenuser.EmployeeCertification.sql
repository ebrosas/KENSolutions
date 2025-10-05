

    BEGIN TRAN T1

    INSERT INTO [kenuser].[EmployeeCertification]
           ([QualificationCode]
           ,[Specialization]
           ,[University]
           ,[Institute]
           ,[CountryCode]
           ,[FromMonthCode]
           ,[FromYear]
           ,[ToMonthCode]
           ,[ToYear]
           ,[PassMonthCode]
           ,[PassYear]
           ,[TransactionNo]
           ,[EmployeeNo]
           ,[StreamCode]
           ,[CityTownName]
           ,[State])
    SELECT  'QTBACHELOR' AS [QualificationCode]
            ,'Computer Engineering' AS [Specialization]
            ,'Polytechnic University of the Philippines' AS [University]
            ,'State University' AS [Institute]
            ,'PH' AS [CountryCode]            
            ,'SEP' AS [FromMonthCode]
            ,1995 AS [FromYear]
            ,'MAY' AS [ToMonthCode]
            ,2000 AS [ToYear]
            ,'JAN' AS [PassMonthCode]
            ,2025 AS [PassYear]
            ,NULL AS [TransactionNo]
            ,10003632 [EmployeeNo]
            ,'STMICT' AS [StreamCode]
            ,'Manila' AS [CityTownName]
            ,'National Capital Region' AS [State]
            

    ROLLBACK TRAN T1
    --COMMIT TRAN T1


