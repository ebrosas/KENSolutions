DECLARE @leaveNo BIGINT = 1

	SELECT a.StatusCode, b.UDCDesc1 as StatusDesc, 
		a.StatusID, a.StatusHandlingCode, a.* 
	FROM kenuser.PlannedLeaveRequest a
		CROSS APPLY
		(
			SELECT y.* 
			FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
				INNER JOIN kenuser.UserDefinedCode y WITH (NOLOCK) ON x.UDCGroupId = y.GroupID
			where x.UDCGCode = 'STATUS'
				AND y.UDCCode = a.StatusCode
		) b

	WHERE a.PlannedLeaveId = @leaveNo

/*	Analyse data

	SELECT * FROM kenuser.PlannedLeaveRequest a
	WHERE a.EmpNo = 10003632

*/	

	
/*	Remove records:

	TRUNCATE TABLE [kenuser].[PlannedLeaveRequest] 

	DELETE FROM [kenuser].[PlannedLeaveRequest] 

*/

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.PlannedLeaveRequest
	SET LeaveApprovalFlag = 'A'
	WHERE LeaveRequestId = 9

	COMMIT TRAN T1

*/
