DECLARE @leaveNo BIGINT = 18

	SELECT a.LeaveApprovalFlag, a.LeaveStatusCode, b.UDCDesc1 as StatusDesc, 
		a.LeaveStatusID, a.StatusHandlingCode, a.* 
	FROM kenuser.LeaveRequisitionWF a
		CROSS APPLY
		(
			SELECT y.* 
			FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
				INNER JOIN kenuser.UserDefinedCode y WITH (NOLOCK) ON x.UDCGroupId = y.GroupID
			where x.UDCGCode = 'STATUS'
				AND y.UDCCode = a.LeaveStatusCode
		) b

	WHERE a.LeaveRequestId = @leaveNo

	SELECT * FROM [kenuser].[LeaveAttachments] a
	WHERE a.LeaveAttachmentId = 
	(
		SELECT x.LeaveAttachmentId
		FROM kenuser.LeaveRequisitionWF x
		WHERE x.LeaveRequestId = @leaveNo
	)

	SELECT a.LeaveApprovalFlag, a.LeaveStatusCode, a.LeaveStatusID, a.StatusHandlingCode, a.LeaveDuration, a.HalfDayLeaveFlag, * 
	FROM kenuser.LeaveRequisitionWF a
	WHERE a.LeaveEmpNo = 10003632
	ORDER BY a.LeaveVisaRequired

	-- SELECT a.* 
	-- FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	-- WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'STATUS')
	-- ORDER BY a.UDCCode

	SELECT b.* 
	FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		INNER JOIN kenuser.UserDefinedCode b WITH (NOLOCK) ON a.UDCGroupId = b.GroupID
	where a.UDCGCode = 'STATUS'
	ORDER BY b.UDCCode

	SELECT SUM(x.LeaveDuration) AS TotalLeave 
	FROM kenuser.LeaveRequisitionWF x WITH (NOLOCK)
	WHERE RTRIM(x.LeaveApprovalFlag) NOT IN ('C', 'R', 'D')
		AND x.LeaveEmpNo = 10003632
		AND (x.LeaveStartDate >= '04/16/2026' AND x.LeaveStartDate <= '05/15/2026')

	SELECT COUNT(x.LeaveRequestId) AS TotalHalfDay
	FROM kenuser.LeaveRequisitionWF x WITH (NOLOCK)
	WHERE RTRIM(x.LeaveApprovalFlag) NOT IN ('C', 'R', 'D')
		AND 
		(
			RTRIM(x.EndDayMode) IN ('LEAVEFH', 'LEAVESH')
			OR RTRIM(x.StartDayMode) IN ('LEAVEFH', 'LEAVESH')
		)
		AND x.LeaveEmpNo = 10003632
		AND (x.LeaveStartDate >= '04/16/2026' AND x.LeaveStartDate <= '05/15/2026')

	
/*	Remove records:

	TRUNCATE TABLE [kenuser].[LeaveRequisitionWF] 

	DELETE FROM [kenuser].[LeaveRequisitionWF] 

*/

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.LeaveRequisitionWF
	SET LeaveApprovalFlag = 'A'
	WHERE LeaveRequestId = 9

	COMMIT TRAN T1

*/
