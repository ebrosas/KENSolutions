
	BEGIN TRAN T1

	INSERT INTO [kenuser].[Holiday]
    (
		[HolidayDesc]
        ,[HolidayDate]
        ,[HolidayType]
        ,[CreatedByEmpNo]
        ,[CreatedByName]
        ,[CreatedByUserID]
        ,[CreatedDate]
	)
     SELECT 'New Year Day' AS HolidayDesc,
			'01/01/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Fitr' AS HolidayDesc,
			'03/18/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Fitr' AS HolidayDesc,
			'03/19/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Labour Day' AS HolidayDesc,
			'05/01/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Adha' AS HolidayDesc,
			'05/26/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Adha' AS HolidayDesc,
			'05/27/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Adha' AS HolidayDesc,
			'05/28/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Ashura' AS HolidayDesc,
			'06/24/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'National Day' AS HolidayDesc,
			'12/16/2026'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	ROLLBACK TRAN T1
	--COMMIT TRAN T1

	SELECT * FROM [kenuser].[Holiday] a


