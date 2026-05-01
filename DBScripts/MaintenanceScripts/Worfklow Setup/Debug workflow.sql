DECLARE @workflowTypeCode	VARCHAR(100) = 'RTYPELEAVE',
		@requestNo			BIGINT = 15

	--Get workflow setup
	SELECT * FROM kenuser.WorkflowDefinitions a
	WHERE RTRIM(a.EntityName) = @workflowTypeCode

	--Get WF activities
	SELECT a.* 
	FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
		INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
		INNER JOIN kenuser.WorkflowInstances c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
	WHERE RTRIM(b.EntityName) = @workflowTypeCode
		AND c.EntityId = @requestNo

	--Get WF activity conditions
	SELECT a.* 
	FROM kenuser.WorkflowConditions a WITH (NOLOCK) 
		INNER JOIN kenuser.WorkflowStepDefinitions b WITH (NOLOCK) ON a.StepDefinitionId = b.StepDefinitionId
		INNER JOIN kenuser.WorkflowDefinitions c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
	WHERE RTRIM(c.EntityName) = @workflowTypeCode

	--Get the workflow instance
	SELECT a.* 
	FROM kenuser.WorkflowInstances a
		INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
	WHERE RTRIM(b.EntityName) = @workflowTypeCode
		AND a.EntityId = @requestNo

	SELECT a.* 
	FROM kenuser.WorkflowStepInstances a
		INNER JOIN kenuser.WorkflowInstances b WITH (NOLOCK) ON a.WorkflowInstanceId = b.WorkflowInstanceId
		INNER JOIN kenuser.WorkflowDefinitions c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
	WHERE RTRIM(c.EntityName) = @workflowTypeCode
		AND b.EntityId = @requestNo

/*	Clear workflow

	BEGIN TRAN T1

	DELETE FROM kenuser.WorkflowDefinitions
	WHERE WorkflowDefinitionId = 11

	UPDATE kenuser.WorkflowStepInstances 
	SET ApproverEmpNo = 10003632
	WHERE StepInstanceId = 19

	COMMIT TRAN T1

*/	

/*	Re-open workflow

	BEGIN TRAN T1

	UPDATE kenuser.WorkflowInstances
	SET Status = 'Running'
	WHERE WorkflowInstanceId = 13

	DELETE FROM kenuser.WorkflowStepInstances
	WHERE StepInstanceId IN (18, 19)

	UPDATE kenuser.WorkflowStepInstances
	SET ApproverUserID = NULL,
		Status = 'Pending',
		ActionDate = null,
		Comments = null
	WHERE StepInstanceId = 17

	ROLLBACK TRAN T1
	COMMIT TRAN T1


*/