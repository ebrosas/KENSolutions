
	BEGIN TRAN T1

	INSERT INTO [kenuser].[Qualification]
           ([QualificationCode]
           ,[StreamCode]
           ,[SpecializationCode]
           ,[UniversityName]
           ,[Institute]
           ,[QualificationMode]
           ,[CountryCode]
           ,[StateCode]
           ,[FromMonthCode]
           ,[FromYear]
           ,[ToMonthCode]
           ,[ToYear]
           ,[PassMonthCode]
           ,[PassYear]
           ,[EmployeeNo]
           ,[TransactionNo]
           ,[CityTownName])
	SELECT	'QTBACHELOR' AS [QualificationCode],
			'STMICT' AS [StreamCode],
			'SPECENGING' AS [SpecializationCode],
			'Polytechnic University of the Philippines' AS [UniversityName],
			'PUP' AS [Institute],
			'QMFULLTIME' AS [QualificationMode],
			'PH' AS [CountryCode],
			NULL AS [StateCode],
			'SEP' AS [FromMonthCode],
			1995 AS [FromYear],
			'MAY' AS [ToMonthCode],
			2000 AS [ToYear],
			'MAY' AS [PassMonthCode],
			2000 AS [PassYear],
			10003632 AS [EmployeeNo],
			NULL AS [TransactionNo],
			'Manila' AS [CityTownName]

	--ROLLBACK TRAN T1
	COMMIT TRAN T1
