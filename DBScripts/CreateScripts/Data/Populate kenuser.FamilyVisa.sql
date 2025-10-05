

    BEGIN TRAN T1

    INSERT INTO [kenuser].[FamilyVisa]
           ([CountryCode]
           ,[VisaTypeCode]
           ,[Profession]
           ,[IssueDate]
           ,[ExpiryDate]
           ,[TransactionNo]
           ,[EmployeeNo]
		   ,FamilyId)
     SELECT 'PH' AS [CountryCode]
          ,'VTWORK' AS [VisaTypeCode]
          ,'Software Engineer' AS [Profession]
          ,'10/17/2023' AS [IssueDate]
          ,'10/16/2025' AS [ExpiryDate]
          ,NULL AS [TransactionNo]
          ,10003632 AS [EmployeeNo]
		  ,a.AutoId
	FROM kenuser.FamilyMember a 
	WHERE a.EmployeeNo = 10003632
		AND RTRIM(a.FirstName) = 'Antonina'


    --ROLLBACK TRAN T1
    COMMIT TRAN T1

/*

	TRUNCATE TABLE [kenuser].[FamilyVisa]

*/


