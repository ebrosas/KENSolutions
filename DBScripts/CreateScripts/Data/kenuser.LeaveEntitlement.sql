
	BEGIN TRAN T1

	INSERT INTO [kenuser].[LeaveEntitlement]
           ([LeaveEntitlemnt]
           ,[SickLeaveEntitlemnt]
           ,[LeaveUOM]
           ,[CreatedDate]
           ,[LeaveCreatedBy]
           ,[CreatedUserID]
           ,[EmployeeNo])
     SELECT 28 AS LeaveEntitlemnt,
			30 AS SickLeaveEntitlemnt, 
			'D' AS LeaveUOM, 
			GETDATE() AS CreatedDate,
			10003632 AS LeaveCreatedBy,
			'ervin' AS CreatedUserID, 
			10003632 AS EmployeeNo

	UNION
    
	SELECT	22 AS LeaveEntitlemnt,
			15 AS SickLeaveEntitlemnt, 
			'D' AS LeaveUOM, 
			GETDATE() AS CreatedDate,
			10003632 AS LeaveCreatedBy,
			'ervin' AS CreatedUserID, 
			10003633 AS EmployeeNo

	ROLLBACK TRAN T1
	--COMMIT TRAN T1


