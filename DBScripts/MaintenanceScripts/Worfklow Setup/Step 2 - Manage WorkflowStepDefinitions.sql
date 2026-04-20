DECLARE	@actionType				TINYINT = 0,		--(Notes: 0 = Check records, 1 = Create new workflow, 2 = Update record)
		@isCommitTrans			BIT = 0,
		@workflowTypeCode		VARCHAR(100) = 'RTYPELEAVE',
		@stepName				VARCHAR(200),
		@stepOrder				INT,
		@approvalRole			VARCHAR(50),
		@isParallelGroup		BIT = 0,
		@requiresAllParallel	BIT = 0,
		@approvalStageDesc		VARCHAR(300)

	DECLARE @workflowDefinitionId INT = 0

	--Get the work definition id
	SELECT @workflowDefinitionId = a.WorkflowDefinitionId 
	FROM kenuser.WorkflowDefinitions a WITH (NOLOCK)
	WHERE RTRIM(a.EntityName) = @workflowTypeCode

	--Get the GUID
	DECLARE @parallelGroupId AS UNIQUEIDENTIFIER = NEWID() 

/*	Setup workflow for Leave Requisition

	--Add Direct Supervisor
	SELECT	@stepName				= 'Approval by Direct Supervisor',
			@stepOrder				= 1,
			@approvalRole			= 'SUPERVISOR',
			@isParallelGroup		= 0,
			@requiresAllParallel	= 0,
			@approvalStageDesc		= 'Direct Supervisor'

	--Add Cost Center Manager
	SELECT	@stepName				= 'Approval by Department Manager',
			@stepOrder				= 2,
			@approvalRole			= 'CCMANAGER',
			@isParallelGroup		= 0,
			@requiresAllParallel	= 0,
			@approvalStageDesc		= 'Department Manager'

	--Add Head of HR
	SELECT	@stepName				= 'Final approval by Head of HR',
			@stepOrder				= 3,
			@approvalRole			= 'HRHEAD',
			@isParallelGroup		= 0,
			@requiresAllParallel	= 0,
			@approvalStageDesc		= 'Head of HR'

*/

	IF @actionType = 0
	BEGIN
    
		SELECT * FROM kenuser.WorkflowDefinitions a
		WHERE RTRIM(a.EntityName) = @workflowTypeCode

		SELECT a.* 
		FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
		WHERE RTRIM(b.EntityName) = @workflowTypeCode
	END

	ELSE IF @actionType = 1
	BEGIN
    
		BEGIN TRAN T1

		INSERT INTO kenuser.WorkflowStepDefinitions
        (
			[WorkflowDefinitionId]
           ,[StepName]
           ,[StepOrder]
           ,[ApprovalRole]
           ,[IsParallelGroup]
           ,[ParallelGroupId]
           ,[RequiresAllParallel]
           ,[ApprovalStageDesc]
		)
		SELECT	a.WorkflowDefinitionId,
				@stepName,
				@stepOrder,
				@approvalRole,
				@isParallelGroup,
				@parallelGroupId,
				@requiresAllParallel,
				@approvalStageDesc	
		FROM kenuser.WorkflowDefinitions a WITH (NOLOCK)
		WHERE RTRIM(a.EntityName) = @workflowTypeCode

		--Check
		SELECT a.* 
		FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
		WHERE RTRIM(b.EntityName) = @workflowTypeCode

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1
	END 

	


