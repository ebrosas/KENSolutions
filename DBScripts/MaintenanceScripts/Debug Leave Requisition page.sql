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

	SELECT a.LeaveApprovalFlag, * 
	FROM kenuser.LeaveRequisitionWF a
	WHERE a.LeaveEmpNo = 10003632
	ORDER BY a.LeaveVisaRequired

	
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
