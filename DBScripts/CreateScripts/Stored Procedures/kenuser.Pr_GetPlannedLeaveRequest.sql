/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetPlannedLeaveRequest
*	Description: Get the planned leave details based on the specified search criteria
*
*	Date			Author		Rev. #		Comments:
*	05/07/2026		Ervin		1.0			Created
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetPlannedLeaveRequest
(   	
	@leaveNo		BIGINT = NULL,	
	@empNo			INT = NULL,
	@costCenter		VARCHAR(20) = NULL,	
	@status			VARCHAR(20) = NULL,
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL,
	@usedLeave		BIT = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@leaveNo, 0) = 0
		SET @leaveNo = NULL

	IF ISNULL(@usedLeave, 0) = 0
		SET @usedLeave = NULL

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

	SELECT	a.PlannedLeaveId,
			lv.LeaveNo,
			a.EmpNo,
			a.EmpName,
			a.LeaveStartDate,
			a.LeaveEndDate,
			a.LeaveResumeDate,
			a.StartDayMode,
			sdm.StartDayModeDesc,
			a.EndDayMode,
			edm.EndDayModeDesc,	
			a.CostCenter,
			dep.DepartmentName AS CostCenterName,
			a.Remarks,			
			a.LeaveDuration,
			a.NoOfHolidays,
			a.NoOfWeekends,
			a.HalfDayLeaveFlag,			
			a.StatusID,
			a.StatusCode,
			b.StatusDesc,
			a.StatusHandlingCode,
			a.CreatedDate,
			a.CreatedBy,
			a.CreatedByName,
			a.CreatedUserID,
			a.CreatedEmail,
			a.LastUpdatedDate,
			a.LastUpdatedBy,
			a.LastUpdatedName,
			a.LastUpdatedUserID,
			a.LastUpdatedEmail			
	FROM kenuser.PlannedLeaveRequest a WITH (NOLOCK)
		INNER JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.CostCenter) = RTRIM(dep.DepartmentCode)
		OUTER APPLY
		(
			SELECT RTRIM(x.UDCCode) as StatusCode, RTRIM(x.UDCDesc1) AS StatusDesc
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'STATUS')
				AND RTRIM(x.UDCCode) = RTRIM(a.StatusCode)
		) b
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
		OUTER APPLY
		(
			SELECT x.LeaveRequestId AS LeaveNo
			FROM kenuser.LeaveRequisitionWF x WITH (NOLOCK)
			WHERE x.LeavePlannedNo = a.PlannedLeaveId
		) lv
	WHERE 
		(a.PlannedLeaveId = @leaveNo OR @leaveNo IS NULL)
		AND (a.EmpNo = @empNo OR @empNo IS NULL) 
		AND (RTRIM(a.CostCenter) = @costCenter OR @costCenter IS NULL)
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
		AND 
		(
			(lv.LeaveNo IS NOT NULL AND @usedLeave = 1)
			OR @usedLeave IS NULL 
		)
END

/*	Debug:

PARAMETERS:
	@leaveNo		BIGINT = NULL,	
	@empNo			INT = NULL,
	@costCenter		VARCHAR(20) = NULL,	
	@status			VARCHAR(20) = NULL,
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL,
	@usedLeave		BIT = NULL

	EXEC kenuser.Pr_GetPlannedLeaveRequest
	EXEC kenuser.Pr_GetPlannedLeaveRequest 1
	EXEC kenuser.Pr_GetPlannedLeaveRequest 0, 10003632 
	EXEC kenuser.Pr_GetPlannedLeaveRequest 0, 0, '7600'
	EXEC kenuser.Pr_GetPlannedLeaveRequest 0, 0, '', 'Closed'
	EXEC kenuser.Pr_GetPlannedLeaveRequest 0, 10003632, '', '', '08/01/2026', '08/31/2026'
	EXEC kenuser.Pr_GetPlannedLeaveRequest 0, 0, '', '', NULL, NULL, 0

*/

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.PlannedLeaveRequest 
	SET StatusHandlingCode = 'Cancelled'
	WHERE LeaveRequestId = 9

	UPDATE kenuser.PlannedLeaveRequest 
	SET LeaveApprovalFlag = 'W'
	WHERE LeaveRequestId = 7

	COMMIT TRAN T1

*/