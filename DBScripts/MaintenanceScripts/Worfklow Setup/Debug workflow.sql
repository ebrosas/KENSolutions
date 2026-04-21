DECLARE @workflowTypeCode	VARCHAR(100) = 'RTYPELEAVE'

	--Get workflow setup
	SELECT * FROM kenuser.WorkflowDefinitions a
	WHERE RTRIM(a.EntityName) = @workflowTypeCode

	--Get WF activities
	SELECT a.* 
	FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
		INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
	WHERE RTRIM(b.EntityName) = @workflowTypeCode

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

	SELECT a.* 
	FROM kenuser.WorkflowStepInstances a
		INNER JOIN kenuser.WorkflowInstances b WITH (NOLOCK) ON a.WorkflowInstanceId = b.WorkflowInstanceId
		INNER JOIN kenuser.WorkflowDefinitions c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
	WHERE RTRIM(c.EntityName) = @workflowTypeCode

/*

	BEGIN TRAN T1

	DELETE FROM kenuser.WorkflowDefinitions
	WHERE WorkflowDefinitionId = 11

	COMMIT TRAN T1

*/	