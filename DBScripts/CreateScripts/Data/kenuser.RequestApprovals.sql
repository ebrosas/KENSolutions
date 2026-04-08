
	BEGIN TRAN T1

	INSERT INTO kenuser.RequestApprovals
    (
		[RequestTypeCode]
        ,[RequisitionNo]
        ,[RoutineSequence]
        ,[AssignedEmpNo]
        ,[AssignedEmpName]
        ,[ApprovalRole]
        ,[ActionRole]
        ,[IsApproved]
        ,[IsHold]
        ,[Remarks]
        ,[CreatedDate]
        ,[CreatedBy]
        ,[CreatedUserID]
	)
    SELECT 'RTYPELEAVE' AS RequestTypeCode, 
           14 AS RequisitionNo, 
           1 AS RoutineSequence, 
           10003632 AS AssignedEmpNo, 
           'ERVIN OLINAS BROSAS' AS AssignedEmpName, 
          'Direct Supervisor' AS ApprovalRole,
           2 AS ActionRole,
           NULL AS IsApproved, 
           NULL AS IsHold, 
           NULL AS Remarks, 
           '2026-03-29 10:43:45.000' AS  CreatedDate,
           10003632 AS CreatedBy, 
           'ervin' AS CreatedUserID

	--ROLLBACK TRAN T1
	COMMIT TRAN T1


