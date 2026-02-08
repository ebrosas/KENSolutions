
	BEGIN TRAN T1

	INSERT INTO [kenuser].[LeaveEntitlement]
           ([LeaveEntitlemnt]
           ,[SickLeaveEntitlemnt]
           ,[LeaveUOM]
           ,[CreatedDate]
           ,[LeaveCreatedBy]
           ,[CreatedUserID]
           ,[EmployeeNo]
		   ,DILBalance
		   ,LeaveBalance
		   ,SLBalance)
     SELECT 28 AS LeaveEntitlemnt,
			30 AS SickLeaveEntitlemnt, 
			'D' AS LeaveUOM, 
			GETDATE() AS CreatedDate,
			10003632 AS LeaveCreatedBy,
			'ervin' AS CreatedUserID, 
			10003632 AS EmployeeNo,
			5 AS DILBalance,
			56 AS LeaveBalance,
			35 AS SLBalance

	UNION
    
	SELECT	22 AS LeaveEntitlemnt,
			15 AS SickLeaveEntitlemnt, 
			'D' AS LeaveUOM, 
			GETDATE() AS CreatedDate,
			10003632 AS LeaveCreatedBy,
			'ervin' AS CreatedUserID, 
			10003633 AS EmployeeNo,
			0 AS DILBalance,
			35 AS LeaveBalance,
			23 AS SLBalance

	--ROLLBACK TRAN T1
	COMMIT TRAN T1

/*

	TRUNCATE TABLE kenuser.LeaveEntitlement

*/


