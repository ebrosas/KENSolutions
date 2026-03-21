/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetLeaveRequestDetail
*	Description: Get the leave requisition details based on the specified Leave No.
*
*	Date			Author		Rev. #		Comments:
*	22/03/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

CREATE PROCEDURE kenuser.Pr_GetLeaveRequestDetail
(   		
	@leaveNo	BIGINT
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	SELECT	[LeaveRequestId]
			,[LeaveType]
			,[LeaveEmpNo]
			,[LeaveEmpName]
			,[LeaveEmpEmail]
			,[LeaveStartDate]
			,[LeaveEndDate]
			,[LeaveResumeDate]
			,[LeaveEmpCostCenter]
			,[LeaveRemarks]
			,[LeaveStatusCode]
			,[LeaveApprovalFlag]
			,[LeaveVisaRequired]
			,[LeavePayAdv]
			,[LeaveBalance]
			,[LeaveDuration]
			,[NoOfHolidays]
			,[NoOfWeekends]
			,[LeaveCreatedDate]
			,[LeaveCreatedBy]
			,[LeaveUpdatedDate]
			,[LeaveUpdatedBy]
			,[EndDayMode]
			,[StartDayMode]
			,[LeaveStatusID]
			,[StatusHandlingCode]
			,[WorkflowId]
			,[LeaveAttachmentId]
	FROM kenuser.LeaveRequisitionWF a WITH (NOLOCK)
	WHERE a.LeaveRequestId = @leaveNo
	
	
END

/*	Debug:

PARAMETERS:
	@empNo				INT,
	@attendanceDate		DATETIME

	EXEC kenuser.Pr_GetLeaveRequestDetail 10003632, '01/31/2026'
	EXEC kenuser.Pr_GetLeaveRequestDetail 10003632, '02/01/2026'
	EXEC kenuser.Pr_GetLeaveRequestDetail 10003632, '02/07/2026'

*/