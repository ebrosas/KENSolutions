/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetWorkflowStatus
*	Description: Get the workflow information of specific request
*
*	Date			Author		Rev. #		Comments:
*	21/04/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetWorkflowStatus
(   
	@workflowTypeCode	VARCHAR(100),
	@requestNo			BIGINT	
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	SELECT	DISTINCT			
			c.EntityId AS RequestNo,
			RTRIM(b.EntityName) AS WorkflowType,
			c.[Status] AS WorkflowStatus,
			a.StepDefinitionId AS ActivityID,
			a.ApprovalStageDesc as ActivityName,			
			a.StepOrder as ActivityOrder,
			d.ActivityStatus,
			d.ApproverNo,
			d.ApproverName
			--a.* 
	FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
		INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
		INNER JOIN kenuser.WorkflowInstances c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
		OUTER APPLY
		(
			SELECT	x.[Status] as ActivityStatus,
					x.ApproverEmpNo AS ApproverNo,
					RTRIM(ISNULL(y.FirstName, '')) + RTRIM(ISNULL(y.MiddleName, '')) + RTRIM(ISNULL(y.LastName, '')) AS ApproverName
			FROM kenuser.WorkflowStepInstances x WITH (NOLOCK)
				LEFT JOIN kenuser.Employee y WITH (NOLOCK) ON x.ApproverEmpNo = y.EmployeeNo
			WHERE x.WorkflowInstanceId = c.WorkflowInstanceId
				and x.StepDefinitionId = a.StepDefinitionId
		) d
	WHERE RTRIM(b.EntityName) = @workflowTypeCode
		AND c.EntityId = @requestNo
	ORDER BY a.StepOrder

END 

/*	Test:

	EXEC kenuser.Pr_GetWorkflowStatus 'RTYPELEAVE', 15

*/