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
	@leaveNo		BIGINT = NULL,
	@empNo			INT = NULL,
	@costCenter		VARCHAR(20) = NULL,	
	@leaveType		VARCHAR(20) = NULL,	
	@status			VARCHAR(20) = NULL,
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@leaveNo, 0) = 0
		SET @leaveNo = NULL

	IF ISNULL(@leaveType, 0) = ''
		SET @leaveType = NULL

	IF ISNULL(@costCenter, 0) = ''
		SET @costCenter = NULL

	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	IF ISNULL(@status, 0) = ''
		SET @status = NULL

	IF	@startDate IS NOT NULL AND CAST(@startDate AS DATETIME) = CAST('' AS DATETIME)
		SET @startDate = NULL

	IF	@endDate IS NOT NULL AND CAST(@endDate AS DATETIME) = CAST('' AS DATETIME)
		SET @endDate = NULL

	SELECT	a.LeaveRequestId,
			a.LeaveAttachmentId,
			a.WorkflowId,
			a.LeaveInstanceID,
			a.LeaveType,
			lt.LeaveTypeDesc,
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
			ISNULL(a.LeaveBalance, 0) AS LeaveBalance,
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
			sdm.StartDayModeDesc,
			a.EndDayMode,
			edm.EndDayModeDesc,
			RTRIM(b.UDCDesc1) as StatusDesc,
			RTRIM(c.UDCDesc1) as ApprovalFlagDesc,
			RTRIM(d.FirstName) + ' ' + RTRIM(d.MiddleName) + ' ' + RTRIM(d.LastName) AS CreatedByName,
			emp.DepartmentCode,
			dep.DepartmentName
	FROM kenuser.LeaveRequisitionWF a WITH (NOLOCK)
		INNER JOIN kenuser.Employee emp WITH (NOLOCK) ON a.LeaveEmpNo = emp.EmployeeNo
		INNER JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(emp.DepartmentCode) = RTRIM(dep.DepartmentCode)
		LEFT JOIN kenuser.UserDefinedCode b WITH (NOLOCK) ON RTRIM(a.LeaveStatusCode) = RTRIM(b.UDCCOde)
		LEFT JOIN kenuser.UserDefinedCode c WITH (NOLOCK) ON RTRIM(a.LeaveApprovalFlag) = RTRIM(c.UDCCOde)
		OUTER APPLY
		(
			SELECT RTRIM(x.UDCCode) as LeaveTypeCode, RTRIM(x.UDCDesc1) AS LeaveTypeDesc
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVETYPES')
				AND RTRIM(x.UDCCode) = RTRIM(a.LeaveType)
		) lt
		OUTER APPLY
		(
			SELECT RTRIM(x.UDCCode) as StartDayMode, RTRIM(x.UDCDesc1) AS StartDayModeDesc
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVEAPORTION')
				AND RTRIM(x.UDCCode) = RTRIM(a.StartDayMode)
		) sdm
		OUTER APPLY
		(
			SELECT RTRIM(x.UDCCode) as EndDayMode, RTRIM(x.UDCDesc1) AS EndDayModeDesc
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVEAPORTION')
				AND RTRIM(x.UDCCode) = RTRIM(a.EndDayMode)
		) edm
		LEFT JOIN kenuser.Employee d WITH (NOLOCK) ON a.LeaveCreatedBy = d.EmployeeNo
	WHERE 
		(a.LeaveRequestId = @leaveNo OR @leaveNo IS NULL)
		AND (a.LeaveEmpNo = @empNo OR @empNo IS NULL) 
		AND (RTRIM(a.LeaveType) = @leaveType OR @leaveType IS NULL)
		AND (RTRIM(emp.DepartmentCode) = @costCenter OR @costCenter IS NULL)
		AND (RTRIM(a.StatusHandlingCode) = @status OR @status IS NULL)
		AND 
		(
			(
				a.LeaveStartDate BETWEEN @startDate AND @endDate
				AND a.LeaveResumeDate BETWEEN @startDate AND @endDate
				AND (@startDate IS NOT NULL AND @endDate IS NOT NULL)
			)
			OR (@startDate IS NULL AND @endDate IS NULL) 
		)
END

/*	Debug:

PARAMETERS:
	@leaveNo		BIGINT = 0,
	@empNo			INT = 0,
	@costCenter		VARCHAR(20) = '',	
	@leaveType		VARCHAR(20) = '',	
	@status			VARCHAR(20) = '',
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL

	EXEC kenuser.Pr_GetLeaveRequestDetail
	EXEC kenuser.Pr_GetLeaveRequestDetail 11
	EXEC kenuser.Pr_GetLeaveRequestDetail 0, 10003632 
	EXEC kenuser.Pr_GetLeaveRequestDetail 0, 0, '7600'
	EXEC kenuser.Pr_GetLeaveRequestDetail 0, 0, '', 'AL'
	EXEC kenuser.Pr_GetLeaveRequestDetail 0, 0, '', '', 'Open'
	EXEC kenuser.Pr_GetLeaveRequestDetail 0, 10003632, '', '', '', '03/16/2026', '04/15/2026'

*/

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.LeaveRequisitionWF 
	SET StatusHandlingCode = 'Cancelled'
	WHERE LeaveRequestId = 9

	UPDATE kenuser.LeaveRequisitionWF 
	SET LeaveApprovalFlag = 'W'
	WHERE LeaveRequestId = 7

	COMMIT TRAN T1

*/