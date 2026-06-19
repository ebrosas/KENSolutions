/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetOvertimeDetail
*	Description: Get the leave requisition details based on the specified Leave No.
*
*	Date			Author		Rev. #		Comments:
*	22/03/2026		Ervin		1.0			Created
*	19/06/2026		Ervin		1.1			Returned the current approver emp. no and name
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetOvertimeDetail
(   	
	@requestNo		BIGINT = NULL,
	@empNo			INT = NULL,
	@costCenter		VARCHAR(20) = NULL,	
	@otReasonCode	VARCHAR(20) = NULL,	
	@status			VARCHAR(20) = NULL,
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@requestNo, 0) = 0
		SET @requestNo = NULL

	IF ISNULL(@otReasonCode, 0) = ''
		SET @otReasonCode = NULL

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

	SELECT	a.ExtratimeId,
			a.TS_AutoId,
			a.WorkflowId,
			a.EmployeeNo,
			a.EmployeeName,
			a.CostCenter,
			dep.DepartmentName AS CostCenterName,
			a.AttendanceDate,
			a.OTReasonCode,
			b.UDCDesc1 AS OTReasonDesc,
			a.ActionCode,
			c.UDCDesc1 AS ActionDesc,
			a.OTStartTime,
			a.OTEndTime,
			a.ShiftPattern,
			a.ShiftTiming,
			a.WorkDuration,
			a.OTDuration,
			a.Remarks,
			a.StatusID,
			a.StatusCode,
			RTRIM(stat.UDCDesc1) as StatusDesc,
			a.StatusHandlingCode,
			a.CreatedDate,
			a.CreatedBy,
			a.CreatedUserID,
			a.CreatedEmail,
			RTRIM(d.FirstName) + ' ' + RTRIM(d.MiddleName) + ' ' + RTRIM(d.LastName) AS CreatedByName,
			a.LastUpdatedDate,
			a.LastUpdatedBy,
			a.LastUpdatedUserID,
			a.LastUpdatedEmail,
			wf.ApproverNo,			--Rev. #1.1
			wf.ApproverName			--Rev. #1.1
	FROM kenuser.OTRequestWF a WITH (NOLOCK)
		INNER JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.CostCenter) = RTRIM(dep.DepartmentCode)
		OUTER APPLY
		(
			SELECT x.* 
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup  WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'OTREASON')
				AND RTRIM(x.UDCCOde) = RTRIM(a.OTReasonCode)
		) b
		OUTER APPLY
		(
			SELECT x.* 
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup  WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'ATTENDACT')
				AND RTRIM(x.UDCCOde) = RTRIM(a.ActionCode)
		) c
		LEFT JOIN kenuser.Employee d WITH (NOLOCK) ON a.CreatedBy = d.EmployeeNo
		LEFT JOIN kenuser.UserDefinedCode stat WITH (NOLOCK) ON RTRIM(a.StatusCode) = RTRIM(stat.UDCCOde)
		OUTER APPLY		--Rev. #1.1
		(
			SELECT	x.ApproverEmpNo AS ApproverNo, 
					RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName
			FROM kenuser.WorkflowStepInstances x
				INNER JOIN kenuser.WorkflowInstances y WITH (NOLOCK) ON x.WorkflowInstanceId = y.WorkflowInstanceId
				INNER JOIN kenuser.WorkflowDefinitions z WITH (NOLOCK) ON y.WorkflowDefinitionId = z.WorkflowDefinitionId
				LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON x.ApproverEmpNo = emp.EmployeeNo
			WHERE RTRIM(z.EntityName) = 'RTYPEOT'
				AND RTRIM(x.[Status]) = 'Pending'
				AND y.EntityId = a.ExtratimeId
		) wf
	WHERE 
		(a.ExtratimeId = @requestNo OR @requestNo IS NULL)
		AND (a.EmployeeNo = @empNo OR @empNo IS NULL) 
		AND (RTRIM(a.OTReasonCode) = @otReasonCode OR @otReasonCode IS NULL)
		AND (RTRIM(a.CostCenter) = @costCenter OR @costCenter IS NULL)
		AND (RTRIM(a.StatusHandlingCode) = @status OR @status IS NULL)
		AND 
		(
			a.AttendanceDate BETWEEN @startDate AND @endDate
			OR (@startDate IS NULL AND @endDate IS NULL) 
		)
END

/*	Debug:

PARAMETERS:
	@requestNo		BIGINT = NULL,
	@empNo			INT = NULL,
	@costCenter		VARCHAR(20) = NULL,	
	@otReasonCode		VARCHAR(20) = NULL,	
	@status			VARCHAR(20) = NULL,
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL

	EXEC kenuser.Pr_GetOvertimeDetail
	EXEC kenuser.Pr_GetOvertimeDetail 7
	EXEC kenuser.Pr_GetOvertimeDetail 0, 10003632 
	EXEC kenuser.Pr_GetOvertimeDetail 0, 0, '7600'
	EXEC kenuser.Pr_GetOvertimeDetail 0, 0, '', 'BD'
	EXEC kenuser.Pr_GetOvertimeDetail 0, 0, '', '', 'Open'
	EXEC kenuser.Pr_GetOvertimeDetail 0, 10003632, '', '', '', '05/01/2026', '05/31/2026'

*/

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.OTRequestWF 
	SET StatusHandlingCode = 'Cancelled'
	WHERE ExtratimeId = 9

	COMMIT TRAN T1

*/