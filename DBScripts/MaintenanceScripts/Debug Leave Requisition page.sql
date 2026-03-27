DECLARE @leaveNo	BIGINT = 9

	SELECT a.LeaveApprovalFlag, * 
	FROM kenuser.LeaveRequisitionWF a
	WHERE a.LeaveRequestId = @leaveNo

	SELECT * FROM [kenuser].[LeaveAttachments] a
	WHERE a.LeaveAttachmentId = 
	(
		SELECT x.LeaveAttachmentId
		FROM kenuser.LeaveRequisitionWF x
		WHERE x.LeaveRequestId = @leaveNo
	)

	SELECT a.LeaveApprovalFlag, a.LeaveDuration, a.HalfDayLeaveFlag, * 
	FROM kenuser.LeaveRequisitionWF a
	WHERE a.LeaveEmpNo = 10003632
	ORDER BY a.LeaveVisaRequired

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
