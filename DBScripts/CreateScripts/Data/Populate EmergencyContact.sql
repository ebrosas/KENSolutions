
	BEGIN TRAN T1

	INSERT INTO [kenuser].[EmergencyContact]
    (
		[ContactPerson]
        ,[RelationCode]
        ,[MobileNo]
        ,[LandlineNo]
        ,[Address]
        ,[CountryCode]
        ,[TransactionNo]
        ,[EmployeeNo]
        ,[City]
	)
     SELECT	'Anne Kirsten Ramirez Brosas' AS ContactPerson,
			'RELDAUGHTER' AS RelationCode,
			'35468966' AS MobileNo,
			'5191945' LandlineNo,
			'788 Burgos Street' AS [Address],
			'PH' AS CountryCode,			
			NULL AS TransactionNo,
			10003632 EmployeeNo,
			'Liliw' AS City

	UNION
    
	SELECT	'Katriane Ramirez Brosas' AS ContactPerson,
			'RELDAUGHTER' AS RelationCode,
			'38695426' AS MobileNo,
			'5489966' LandlineNo,
			'Brgy. Bagong Anyo' AS [Address],
			'PH' AS CountryCode,			
			NULL AS TransactionNo,
			10003632 EmployeeNo,
			'Liliw' AS City

	--ROLLBACK TRAN T1
	COMMIT TRAN T1


