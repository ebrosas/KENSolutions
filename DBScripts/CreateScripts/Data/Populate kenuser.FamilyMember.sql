
	BEGIN TRAN T1

	INSERT INTO [kenuser].[FamilyMember]
           ([FirstName]
           ,[MiddleName]
           ,[LastName]
           ,[RelationCode]
           ,[DOB]
           ,[QualificationCode]
           ,[StreamCode]
           ,[SpecializationCode]
           ,[Occupation]
           ,[ContactNo]
           ,[CountryCode]
           ,[StateCode]
           ,[District]
           ,[IsDependent]
           ,[TransactionNo]
           ,[EmployeeNo]
           ,[CityTownName])
     SELECT	'Antonina' AS [FirstName]
		  ,'Ramirez' AS [MiddleName]
		  ,'Brosas' AS [LastName]
		  ,'RELSPOUSE' AS [RelationCode]
		  ,'08/22/1978' AS [DOB]
		  ,'QTBACHELOR' AS [QualificationCode]
		  ,'STMBUSINESS' AS [StreamCode]
		  ,'SPECBUSINESS' AS [SpecializationCode]
		  ,'Office Secretary' AS [Occupation]
		  ,'39875626' AS [ContactNo]
		  ,'PH' AS [CountryCode]
		  ,'LAG' AS [StateCode]
		  ,'Laguna' AS [District]
		  ,1 AS [IsDependent]
		  ,NULL AS [TransactionNo]
		  ,10003632 AS [EmployeeNo]
		  ,'Liliw' AS [CityTownName]

	UNION
    
	 SELECT	'Katriane' AS [FirstName]
		  ,'Ramirez' AS [MiddleName]
		  ,'Brosas' AS [LastName]
		  ,'RELDAUGHTER' AS [RelationCode]
		  ,'10/05/2016' AS [DOB]
		  ,'QTCERTPROG' AS [QualificationCode]
		  ,'STMHEALTH' AS [StreamCode]
		  ,'SPECARTS' AS [SpecializationCode]
		  ,'Accounting Clerk' AS [Occupation]
		  ,'3957845' AS [ContactNo]
		  ,'PH' AS [CountryCode]
		  ,'LAG' AS [StateCode]
		  ,'Makati City' AS [District]
		  ,1 AS [IsDependent]
		  ,NULL AS [TransactionNo]
		  ,10003632 AS [EmployeeNo]
		  ,'Liliw' AS [CityTownName]

	ROLLBACK TRAN T1
	--COMMIT TRAN T1


