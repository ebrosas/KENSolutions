
	BEGIN TRAN T1

/*	Populate public holidays for 2026

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

    SELECT * FROM [kenuser].[Holiday] a

*/

--/*	Populate public holidays for 2025

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
			'01/01/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Fitr' AS HolidayDesc,
			'03/30/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Fitr' AS HolidayDesc,
			'03/31/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Fitr' AS HolidayDesc,
			'04/01/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Labour Day' AS HolidayDesc,
			'05/01/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Adha' AS HolidayDesc,
			'06/08/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Adha' AS HolidayDesc,
			'06/09/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Eid Al-Adha' AS HolidayDesc,
			'06/10/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Ashura' AS HolidayDesc,
			'07/05/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'Ashura' AS HolidayDesc,
			'07/06/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

	UNION
    
	SELECT 'National Day' AS HolidayDesc,
			'12/16/2025'  AS HolidayDate, 
            1 AS HolidayType,
            10003632  AS CreatedByEmpNo,
            'ERVIN OLINAS BROSAS' AS CreatedByName, 
            'ervin' AS CreatedByUserID, 
            GETDATE() AS CreatedDate

    SELECT * FROM [kenuser].[Holiday] a

--*/

	ROLLBACK TRAN T1
	--COMMIT TRAN T1

	


