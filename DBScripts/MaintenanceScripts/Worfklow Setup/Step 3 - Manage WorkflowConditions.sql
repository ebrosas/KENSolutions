DECLARE	@actionType				TINYINT = 0,		--(Notes: 0 = Check records, 1 = Create new workflow, 2 = Update record)
		@isCommitTrans			BIT = 0,
		@workflowTypeCode		VARCHAR(100) = 'RTYPELEAVE',
		@approvalRole			VARCHAR(50),
		@nextApprovalRole		VARCHAR(50),
		@fieldName				VARCHAR(100),
		@operator				VARCHAR(20),
		@compareValue			VARCHAR(50),
		@nextStepDefinitionId	INT,
		@expression				VARCHAR(500),
		@isTerminal				BIT = NULL

	DECLARE @workflowDefinitionId	INT = 0

	--Get the work definition id
	SELECT @workflowDefinitionId = a.WorkflowDefinitionId 
	FROM kenuser.WorkflowDefinitions a WITH (NOLOCK)
	WHERE RTRIM(a.EntityName) = @workflowTypeCode

	--Add condition for Leave Requisition
	SELECT	@approvalRole			= 'CCMANAGER',
			@nextApprovalRole		= 'HRHEAD',
			@fieldName				= 'LeaveConstraints',
			@operator				= '=',
			@compareValue			= '1',
			@expression				= 'LeaveConstraints == "1"',
			@isTerminal				= 0

	--Get the @nextStepDefinitionId
	SELECT @nextStepDefinitionId = a.StepDefinitionId
	FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
		INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
	WHERE RTRIM(b.EntityName) = @workflowTypeCode
		AND RTRIM(a.ApprovalRole) = @nextApprovalRole

	IF @actionType = 0
	BEGIN
    
		SELECT * 
		FROM kenuser.WorkflowConditions a WITH (NOLOCK) 
			INNER JOIN kenuser.WorkflowStepDefinitions b WITH (NOLOCK) ON a.StepDefinitionId = b.StepDefinitionId
			INNER JOIN kenuser.WorkflowDefinitions c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
		WHERE RTRIM(c.EntityName) = @workflowTypeCode

		--Check records to insert
		SELECT	DISTINCT
				a.StepDefinitionId, 
				@fieldName,
				@operator,
				@compareValue,
				@nextStepDefinitionId,
				@expression,
				@isTerminal
		FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
		WHERE RTRIM(b.EntityName) = @workflowTypeCode
			AND RTRIM(a.ApprovalRole) = @approvalRole
	END

	ELSE IF @actionType = 1
	BEGIN
    
		BEGIN TRAN T1

		INSERT INTO kenuser.WorkflowConditions
        (
			[StepDefinitionId]
           ,[FieldName]
           ,[Operator]
           ,[CompareValue]
           ,[NextStepDefinitionId]
           ,[Expression]
           ,[IsTerminal]
		)
		SELECT	DISTINCT
				a.StepDefinitionId, 
				@fieldName,
				@operator,
				@compareValue,
				@nextStepDefinitionId,
				@expression,
				@isTerminal
		FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
		WHERE RTRIM(b.EntityName) = @workflowTypeCode
			AND RTRIM(a.ApprovalRole) = @approvalRole

		SELECT @@ROWCOUNT AS RowsInserted

		--Check
		SELECT a.* 
		FROM kenuser.WorkflowConditions a WITH (NOLOCK) 
			INNER JOIN kenuser.WorkflowStepDefinitions b WITH (NOLOCK) ON a.StepDefinitionId = b.StepDefinitionId
			INNER JOIN kenuser.WorkflowDefinitions c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
		WHERE RTRIM(c.EntityName) = @workflowTypeCode

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1
	END 

	


