
	BEGIN TRAN T1

	INSERT INTO [kenuser].[EmploymentHistory]
           ([CompanyName]
           ,[CompanyAddress]
           ,[Designation]
           ,[Role]
           ,[FromDate]
           ,[ToDate]
           ,[LastDrawnSalary]
           ,[SalaryTypeCode]
           ,[SalaryCurrencyCode]
           ,[ReasonOfChange]
           ,[ReportingManager]
           ,[CompanyWebsite]
           ,[TransactionNo]
           ,[EmployeeNo])
	 SELECT	'Comprehensive Systems Company' AS [CompanyName]
			,'Riyadh, Saudi Arabia' AS [CompanyAddress]
			,'.Net Developer - Team Lead' AS [Designation]
			,'Senior Level' AS [Role]
			,'03/01/2004' AS [FromDate]
			,'05/31/2010' AS [ToDate]
			,4500 AS [LastDrawnSalary]
			,'STBASIC' AS [SalaryTypeCode]
			,'SAR' AS [SalaryCurrencyCode]
			,'Explore better job opportunities' AS [ReasonOfChange]
			,'Surayie Al Dousary' AS [ReportingManager]
			,'www.csc.sa' AS [CompanyWebsite]
			,NULL AS [TransactionNo]
			,10003632 AS [EmployeeNo]

	UNION
    
	SELECT	'Rendition Digital Inc.' AS [CompanyName]
			,'Ayala Ave. Makati City, Philippines' AS [CompanyAddress]
			,'Senior .Net Developer' AS [Designation]
			,'Senior Level' AS [Role]
			,'06/01/2010' AS [FromDate]
			,'10/08/2011' AS [ToDate]
			,95000 AS [LastDrawnSalary]
			,'STBASIC' AS [SalaryTypeCode]
			,'PHP' AS [SalaryCurrencyCode]
			,'Seek opportunity to work abroad' AS [ReasonOfChange]
			,'Anna Marie Magtanggol' AS [ReportingManager]
			,'www.renditiondigital.com' AS [CompanyWebsite]
			,NULL AS [TransactionNo]
			,10003632 AS [EmployeeNo]

	--ROLLBACK TRAN T1
	COMMIT TRAN T1


