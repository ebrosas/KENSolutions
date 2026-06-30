DECLARE	@actionType				TINYINT = 0,		--(Notes: 0 = Check records, 1 = Create new workflow, 2 = Update record, 3 = Delete record)
		@isCommitTrans			BIT,
		@workflowDefinitionId	INT,
		@workflowName			VARCHAR(150),
		@workflowTypeCode		VARCHAR(100),
		@newWorkflowTypeCode	VARCHAR(100),
		@isActive				BIT = 1

	--Leave Requisition Workflow
	--SELECT	@actionType				= 0,		
	--		@isCommitTrans			= 0,
	--		@workflowDefinitionId	= 10,
	--		@workflowName			= 'Leave Requisition',
	--		@workflowTypeCode		= 'RTYPELEAVE',
	--		@newWorkflowTypeCode	= 'RTYPELEAVETEST',
	--		@isActive				= 1	

	--Regularization Request Workflow
	--SELECT	@actionType				= 0,		
	--		@isCommitTrans			= 0,
	--		@workflowName			= 'Regularization',
	--		@workflowTypeCode		= 'RTYPEREGULAR',
	--		@isActive				= 1	

	--Regularization Request Workflow
	--SELECT	@actionType				= 0,		
	--		@isCommitTrans			= 0,
	--		@workflowName			= 'Overtime Request',
	--		@workflowTypeCode		= 'RTYPEOT',
	--		@isActive				= 1	

	--Outdoor Request Workflow
	--SELECT	@actionType				= 0,		
	--		@isCommitTrans			= 0,
	--		@workflowName			= 'Outdoor Request',
	--		@workflowTypeCode		= 'RTYPEOUTDOOR',
	--		@isActive				= 1	

	IF @actionType = 0
	BEGIN
    
		SELECT * FROM kenuser.WorkflowDefinitions a
		WHERE RTRIM(a.EntityName) = @workflowTypeCode

		--Get all workflows
		SELECT * FROM kenuser.WorkflowDefinitions a
	END

	ELSE IF @actionType = 1
	BEGIN
    
		BEGIN TRAN T1

		INSERT INTO kenuser.WorkflowDefinitions
        (
			[Name]
           ,[EntityName]
           ,[IsActive]
		)
		VALUES
        (
			@workflowName,
			@workflowTypeCode,
			@isActive
		)

		--Check
		SELECT * FROM kenuser.WorkflowDefinitions a
		WHERE RTRIM(a.EntityName) = @workflowTypeCode

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1
	END 

	ELSE IF @actionType = 2
	BEGIN
    
		BEGIN TRAN T1

		UPDATE kenuser.WorkflowDefinitions
        SET [Name] = @workflowName,
			EntityName = @newWorkflowTypeCode,
			IsActive = @isActive
		WHERE RTRIM(EntityName) = @workflowTypeCode

		--Check
		SELECT * FROM kenuser.WorkflowDefinitions a
		WHERE RTRIM(a.EntityName) = @newWorkflowTypeCode

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1
	END 

	ELSE IF @actionType = 3
	BEGIN
    
		BEGIN TRAN T1

		DELETE FROM kenuser.WorkflowDefinitions
		WHERE WorkflowDefinitionId = @WorkflowDefinitionId

		SELECT @@ROWCOUNT AS RowsDeleted

		--Check
		SELECT * FROM kenuser.WorkflowDefinitions a
		WHERE WorkflowDefinitionId = @WorkflowDefinitionId

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1
	END 

