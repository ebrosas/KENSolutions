
	BEGIN TRAN T1

	INSERT INTO [kenuser].[PayrollPeriod]
           ([FiscalYear]
		   ,[FiscalMonth]
           ,[PayrollStartDate]
           ,[PayrollEndDate]
           ,[IsActive]
           ,[CreatedByEmpNo]
           ,[CreatedByUserID])
     SELECT
		  2026 AS [FiscalYear]
		  ,1 AS [FiscalMonth]
		  ,'12/16/2025' AS [PayrollStartDate]
		  ,'01/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,2 AS [FiscalMonth]
		  ,'01/16/2026' AS [PayrollStartDate]
		  ,'02/15/2026' AS [PayrollEndDate]
		  ,1 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,3 AS [FiscalMonth]
		  ,'02/16/2026' AS [PayrollStartDate]
		  ,'03/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,4 AS [FiscalMonth]
		  ,'03/16/2026' AS [PayrollStartDate]
		  ,'04/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,5 AS [FiscalMonth]
		  ,'04/16/2026' AS [PayrollStartDate]
		  ,'05/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,6 AS [FiscalMonth]
		  ,'05/16/2026' AS [PayrollStartDate]
		  ,'06/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,7 AS [FiscalMonth]
		  ,'06/16/2026' AS [PayrollStartDate]
		  ,'07/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,8 AS [FiscalMonth]
		  ,'07/16/2026' AS [PayrollStartDate]
		  ,'08/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,9 AS [FiscalMonth]
		  ,'08/16/2026' AS [PayrollStartDate]
		  ,'09/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,10 AS [FiscalMonth]
		  ,'09/16/2026' AS [PayrollStartDate]
		  ,'10/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,11 AS [FiscalMonth]
		  ,'10/16/2026' AS [PayrollStartDate]
		  ,'11/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2026 AS [FiscalYear]
		  ,12 AS [FiscalMonth]
		  ,'11/16/2026' AS [PayrollStartDate]
		  ,'12/15/2026' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION

	SELECT
		  2025 AS [FiscalYear]
		  ,1 AS [FiscalMonth]
		  ,'12/16/2024' AS [PayrollStartDate]
		  ,'01/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,2 AS [FiscalMonth]
		  ,'01/16/2025' AS [PayrollStartDate]
		  ,'02/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,3 AS [FiscalMonth]
		  ,'02/16/2025' AS [PayrollStartDate]
		  ,'03/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,4 AS [FiscalMonth]
		  ,'03/16/2025' AS [PayrollStartDate]
		  ,'04/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,5 AS [FiscalMonth]
		  ,'04/16/2025' AS [PayrollStartDate]
		  ,'05/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,6 AS [FiscalMonth]
		  ,'05/16/2025' AS [PayrollStartDate]
		  ,'06/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,7 AS [FiscalMonth]
		  ,'06/16/2025' AS [PayrollStartDate]
		  ,'07/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,8 AS [FiscalMonth]
		  ,'07/16/2025' AS [PayrollStartDate]
		  ,'08/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,9 AS [FiscalMonth]
		  ,'08/16/2025' AS [PayrollStartDate]
		  ,'09/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,10 AS [FiscalMonth]
		  ,'09/16/2025' AS [PayrollStartDate]
		  ,'10/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,11 AS [FiscalMonth]
		  ,'10/16/2025' AS [PayrollStartDate]
		  ,'11/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	UNION
    
	SELECT
		  2025 AS [FiscalYear]
		  ,12 AS [FiscalMonth]
		  ,'11/16/2025' AS [PayrollStartDate]
		  ,'12/15/2025' AS [PayrollEndDate]
		  ,0 AS [IsActive]
		  ,10003632 AS [CreatedByEmpNo]
		  ,'ervin' AS [CreatedByUserID]

	SELECT @@ROWCOUNT AS RowsInserted

	--ROLLBACK TRAN T1
	COMMIT TRAN T1

/*

	SELECT * FROM [kenuser].[PayrollPeriod] a ORDER BY a.FiscalYear DESC, a.FiscalMonth ASC

	TRUNCATE TABLE [kenuser].[PayrollPeriod]

*/

