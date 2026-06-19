/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetRegularizationDetail
*	Description: Get the leave requisition details based on the specified Leave No.
*
*	Date			Author		Rev. #		Comments:
*	22/03/2026		Ervin		1.0			Created
*	07/06/2026		Ervin		1.1			Refactored the logic in fetching the ROA description and Action description
*	19/06/2026		Ervin		1.2			Returned the current approver emp. no and name
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetRegularizationDetail
(   	
	@requestNo		BIGINT = NULL,
	@empNo			INT = NULL,
	@costCenter		VARCHAR(20) = NULL,	
	@roaCode		VARCHAR(20) = NULL,	
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

	IF ISNULL(@roaCode, 0) = ''
		SET @roaCode = NULL

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

	SELECT	a.RegularizationId,
			a.AttachmentId,
			a.WorkflowId,
			a.EmployeeNo,
			a.EmployeeName,
			a.CostCenter,
			dep.DepartmentName AS CostCenterName,
			a.AttendanceDate,
			a.ROACode,
			b.UDCDesc1 AS ROADesc,
			a.ActionCode,
			c.UDCDesc1 AS ActionDesc,
			a.RegularizedTimeIn,
			a.RegularizedTimeOut,
			a.ShiftPattern,
			a.ShiftTiming,
			a.WorkDuration,
			a.NoPayHours,
			a.RegularizedDescription,
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
			wf.ApproverNo,			--Rev. #1.2
			wf.ApproverName			--Rev. #1.2
	FROM kenuser.RegularRequestWFs a WITH (NOLOCK)
		INNER JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.CostCenter) = RTRIM(dep.DepartmentCode)
		OUTER APPLY		--Rev. #1.1
		(
			SELECT x.* 
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup  WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'ROATYPE')
				AND RTRIM(x.UDCCOde) = RTRIM(a.ROACode)
		) b
		OUTER APPLY		--Rev. #1.1
		(
			SELECT x.* 
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup  WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'ATTENDACT')
				AND RTRIM(x.UDCCOde) = RTRIM(a.ActionCode)
		) c
		LEFT JOIN kenuser.Employee d WITH (NOLOCK) ON a.CreatedBy = d.EmployeeNo
		LEFT JOIN kenuser.UserDefinedCode stat WITH (NOLOCK) ON RTRIM(a.StatusCode) = RTRIM(stat.UDCCOde)
		OUTER APPLY		--Rev. #1.2
		(
			SELECT	x.ApproverEmpNo AS ApproverNo, 
					RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName
			FROM kenuser.WorkflowStepInstances x
				INNER JOIN kenuser.WorkflowInstances y WITH (NOLOCK) ON x.WorkflowInstanceId = y.WorkflowInstanceId
				INNER JOIN kenuser.WorkflowDefinitions z WITH (NOLOCK) ON y.WorkflowDefinitionId = z.WorkflowDefinitionId
				LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON x.ApproverEmpNo = emp.EmployeeNo
			WHERE RTRIM(z.EntityName) = 'RTYPEREGULAR'
				AND RTRIM(x.[Status]) = 'Pending'
				AND y.EntityId = a.RegularizationId
		) wf
	WHERE 
		(a.RegularizationId = @requestNo OR @requestNo IS NULL)
		AND (a.EmployeeNo = @empNo OR @empNo IS NULL) 
		AND (RTRIM(a.ROACode) = @roaCode OR @roaCode IS NULL)
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
	@roaCode		VARCHAR(20) = NULL,	
	@status			VARCHAR(20) = NULL,
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL

	EXEC kenuser.Pr_GetRegularizationDetail
	EXEC kenuser.Pr_GetRegularizationDetail 2
	EXEC kenuser.Pr_GetRegularizationDetail 0, 10003632 
	EXEC kenuser.Pr_GetRegularizationDetail 0, 0, '7600'
	EXEC kenuser.Pr_GetRegularizationDetail 0, 0, '', 'XL'
	EXEC kenuser.Pr_GetRegularizationDetail 0, 0, '', '', 'Open'
	EXEC kenuser.Pr_GetRegularizationDetail 0, 10003632, '', '', '', '05/01/2026', '05/31/2026'

*/

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.RegularRequestWFs 
	SET StatusHandlingCode = 'Cancelled'
	WHERE LeaveRequestId = 9

	UPDATE kenuser.RegularRequestWFs 
	SET LeaveApprovalFlag = 'W'
	WHERE LeaveRequestId = 7

	COMMIT TRAN T1

*/