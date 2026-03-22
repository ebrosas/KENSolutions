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

ALTER PROCEDURE kenuser.Pr_GetLeaveRequestDetail
(   		
	@leaveNo	BIGINT
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	SELECT	a.LeaveRequestId,
			a.LeaveAttachmentId,
			a.WorkflowId,
			a.LeaveInstanceID,
			a.LeaveType,
			a.LeaveEmpNo,
			a.LeaveEmpName,
			a.LeaveEmpEmail,
			a.LeaveStartDate,
			a.LeaveEndDate,
			a.LeaveResumeDate,
			a.LeaveEmpCostCenter,
			a.LeaveRemarks,
			a.LeaveConstraints,
			a.LeaveStatusCode,
			a.LeaveApprovalFlag,
			a.LeaveVisaRequired,
			a.LeavePayAdv,
			a.LeaveIsFTMember,
			a.LeaveBalance,
			a.LeaveDuration,
			a.NoOfHolidays,
			a.NoOfWeekends,
			a.PlannedLeave,
			a.LeavePlannedNo,
			a.HalfDayLeaveFlag,
			a.LeaveCreatedDate,
			a.LeaveCreatedBy,
			a.LeaveCreatedUserID,
			a.LeaveCreatedEmail,
			a.LeaveUpdatedDate,
			a.LeaveUpdatedBy,
			a.LeaveUpdatedUserID,
			a.LeaveUpdatedEmail,
			a.LeaveStatusID,
			a.StatusHandlingCode,
			a.StartDayMode,
			a.EndDayMode,
			RTRIM(b.UDCDesc1) as StatusDesc,
			RTRIM(c.UDCDesc1) as ApprovalFlagDesc
	FROM kenuser.LeaveRequisitionWF a WITH (NOLOCK)
		LEFT JOIN kenuser.UserDefinedCode b WITH (NOLOCK) ON RTRIM(a.LeaveStatusCode) = RTRIM(b.UDCCOde)
		LEFT JOIN kenuser.UserDefinedCode c WITH (NOLOCK) ON RTRIM(a.LeaveApprovalFlag) = RTRIM(c.UDCCOde)
	WHERE a.LeaveRequestId = @leaveNo
	
	
END

/*	Debug:

PARAMETERS:
	@leaveNo	BIGINT

	EXEC kenuser.Pr_GetLeaveRequestDetail 9

*/