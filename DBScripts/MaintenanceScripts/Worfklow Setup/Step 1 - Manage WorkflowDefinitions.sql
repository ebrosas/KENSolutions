DECLARE	@actionType				TINYINT = 0,		--(Notes: 0 = Check records, 1 = Create new workflow, 2 = Update record, 3 = Delete record)
		@isCommitTrans			BIT = 0,
		@workflowDefinitionId	INT = 10,
		@workflowName			VARCHAR(150) = 'Leave Requisition',
		@workflowTypeCode		VARCHAR(100) = 'RTYPELEAVE',
		@newWorkflowTypeCode	VARCHAR(100) = 'RTYPELEAVETEST',
		@isActive				BIT = 1

	IF @actionType = 0
	BEGIN
    
		SELECT * FROM kenuser.WorkflowDefinitions a
		WHERE RTRIM(a.EntityName) = @workflowTypeCode
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

