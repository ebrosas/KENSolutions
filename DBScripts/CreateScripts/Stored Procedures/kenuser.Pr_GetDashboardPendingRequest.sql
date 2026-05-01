/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetDashboardPendingRequest
*	Description: Get the list of pending requests based on the supplier employee number
*
*	Date			Author		Rev. #		Comments:
*	25/04/2026		Ervin		1.0			Created
*	01/05/2026		Ervin		1.1			Added "StepInstanceId" field
*
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetDashboardPendingRequest
(   
	@empNo			INT = 0,
	@requestType	VARCHAR(100) = ''
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	IF ISNULL(@requestType, '') = '' OR RTRIM(@requestType) = '<ALL>'
		SET @requestType = NULL

	IF @requestType IS NOT NULL
	BEGIN

		SELECT	DISTINCT			
				c.EntityId AS RequestNo,
				RTRIM(b.EntityName) AS RequestTypeCode,
				udc.RequestTypeDesc,
				lv.AppliedDate,
				lv.RequestedByNo,
				lv.RequestedByName,
				lv.LeaveDetails AS Detail,
				a.ApprovalStageDesc as ApprovalRole,			
				d.ActivityStatus AS CurrentStatus,
				d.ApproverNo,
				d.ApproverName,
				d.PendingDays,
				d.StepInstanceId	--Rev. #1.1
		FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
			INNER JOIN kenuser.WorkflowInstances c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
			CROSS APPLY
			(
				SELECT RTRIM(x.UDCDesc1) AS RequestTypeDesc  
				FROM kenuser.UserDefinedCode x WITH (NOLOCK)
				WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
					AND RTRIM(x.UDCCode) = RTRIM(b.EntityName)
			) udc 
			CROSS APPLY
			(
				SELECT	x.LeaveCreatedDate AS AppliedDate,
						x.LeaveCreatedBy AS RequestedByNo,	
						RTRIM(ISNULL(y.FirstName, '')) + ' ' + RTRIM(ISNULL(y.MiddleName, '')) + ' ' + RTRIM(ISNULL(y.LastName, '')) AS RequestedByName,
						'Leave Type: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
							'Originator Employee: ' + x.LeaveEmpName + CHAR(13) + CHAR(10) + 
							'Leave Start Date: ' + FORMAT(x.LeaveStartDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
							'Leave Resume Date: ' + FORMAT(x.LeaveResumeDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) AS LeaveDetails
				FROM kenuser.LeaveRequisitionWF x WITH (NOLOCK) 
					INNER JOIN kenuser.Employee y WITh (NOLOCK) ON x.LeaveCreatedBy = y.EmployeeNo
					CROSS APPLY
					(
						SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
						WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVETYPES')
							AND RTRIM(UDCCode) = RTRIM(x.LeaveType)
					) udc 
				WHERE x.LeaveRequestId = c.EntityId
			) lv
			OUTER APPLY
			(
				SELECT	x.[Status] as ActivityStatus,
						x.ApproverEmpNo AS ApproverNo,
						RTRIM(ISNULL(y.FirstName, '')) + ' ' + RTRIM(ISNULL(y.MiddleName, '')) + ' ' + RTRIM(ISNULL(y.LastName, '')) AS ApproverName,
						DATEDIFF(DAY, x.ActionDate, GETDATE()) AS PendingDays,
						x.StepInstanceId		--Rev. #1.1
				FROM kenuser.WorkflowStepInstances x WITH (NOLOCK)
					LEFT JOIN kenuser.Employee y WITH (NOLOCK) ON x.ApproverEmpNo = y.EmployeeNo
				WHERE x.WorkflowInstanceId = c.WorkflowInstanceId
					and x.StepDefinitionId = a.StepDefinitionId
			) d
		WHERE RTRIM(d.ActivityStatus) = 'Pending'
			AND (d.ApproverNo = @empNo OR @empNo IS NULL)
			AND RTRIM(b.EntityName) = @requestType
		ORDER BY c.EntityId
	END

	ELSE 
	BEGIN

		--Leave Requisition
		SELECT	DISTINCT			
			c.EntityId AS RequestNo,
			RTRIM(b.EntityName) AS RequestTypeCode,
			udc.RequestTypeDesc,
			lv.AppliedDate,
			lv.RequestedByNo,
			lv.RequestedByName,
			lv.LeaveDetails AS Detail,
			a.ApprovalStageDesc as ApprovalRole,			
			d.ActivityStatus AS CurrentStatus,
			d.ApproverNo,
			d.ApproverName,
			d.PendingDays,
			d.StepInstanceId		--Rev. #1.1
		FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
			INNER JOIN kenuser.WorkflowInstances c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
			CROSS APPLY
			(
				SELECT RTRIM(x.UDCDesc1) AS RequestTypeDesc  
				FROM kenuser.UserDefinedCode x WITH (NOLOCK)
				WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
					AND RTRIM(x.UDCCode) = RTRIM(b.EntityName)
			) udc 
			CROSS APPLY
			(
				SELECT	x.LeaveCreatedDate AS AppliedDate,
						x.LeaveCreatedBy AS RequestedByNo,	
						RTRIM(ISNULL(y.FirstName, '')) + ' ' + RTRIM(ISNULL(y.MiddleName, '')) + ' ' + RTRIM(ISNULL(y.LastName, '')) AS RequestedByName,
						'Leave Type: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
							'Originator Employee: ' + x.LeaveEmpName + CHAR(13) + CHAR(10) + 
							'Leave Start Date: ' + FORMAT(x.LeaveStartDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
							'Leave Resume Date: ' + FORMAT(x.LeaveResumeDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) AS LeaveDetails
				FROM kenuser.LeaveRequisitionWF x WITH (NOLOCK) 
					INNER JOIN kenuser.Employee y WITh (NOLOCK) ON x.LeaveCreatedBy = y.EmployeeNo
					CROSS APPLY
					(
						SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
						WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVETYPES')
							AND RTRIM(UDCCode) = RTRIM(x.LeaveType)
					) udc 
				WHERE x.LeaveRequestId = c.EntityId
			) lv
			OUTER APPLY
			(
				SELECT	x.[Status] as ActivityStatus,
						x.ApproverEmpNo AS ApproverNo,
						RTRIM(ISNULL(y.FirstName, '')) + ' ' + RTRIM(ISNULL(y.MiddleName, '')) + ' ' + RTRIM(ISNULL(y.LastName, '')) AS ApproverName,
						DATEDIFF(DAY, x.ActionDate, GETDATE()) AS PendingDays,
						x.StepInstanceId		--Rev. #1.1
				FROM kenuser.WorkflowStepInstances x WITH (NOLOCK)
					LEFT JOIN kenuser.Employee y WITH (NOLOCK) ON x.ApproverEmpNo = y.EmployeeNo
				WHERE x.WorkflowInstanceId = c.WorkflowInstanceId
					and x.StepDefinitionId = a.StepDefinitionId
			) d
		WHERE RTRIM(d.ActivityStatus) = 'Pending'
			AND (d.ApproverNo = @empNo OR @empNo IS NULL)
		ORDER BY c.EntityId
	END 

END 

/*	Debug:

	--Staging database
	EXEC kenuser.Pr_GetDashboardPendingRequest
	EXEC kenuser.Pr_GetDashboardPendingRequest 10003635		
	EXEC kenuser.Pr_GetDashboardPendingRequest 10003635, 'RTYPELEAVE'
	
	--Development database
	EXEC kenuser.Pr_GetDashboardPendingRequest 10003632
	EXEC kenuser.Pr_GetDashboardPendingRequest 10003632, 'RTYPELEAVE'


Parameters:
	@empNo			INT,
	@requestType	VARCHAR(100)

*/