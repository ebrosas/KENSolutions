/*********************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_ApprovalHistory_CRUD
*	Description: This stored procedure is used to perform CRUD operations for "RequestApprovals" table
*
*	Date			Author		Revision No.	Comments:
*	03/05/2026		Ervin		1.0				Created
*
***********************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_ApprovalHistory_CRUD
(	
	@actionType			TINYINT,	--(Notes: 0 = Check records, 1 = Insert, 2 = Update, 3 = Delete)	
	@stepInstanceId		INT,
	@approverEmpNo		INT,
	@approverUserID		VARCHAR(50),
	@isApproved			BIT,
	@isHold				BIT, 
	@approverRemarks	VARCHAR(500)
)
AS	
BEGIN

	IF @actionType = 1
	BEGIN

		INSERT INTO kenuser.RequestApprovals
		(
			[RequestTypeCode]
			,[RequisitionNo]
			,[RoutineSequence]
			,[AssignedEmpNo]
			,[AssignedEmpName]
			,[ApprovalRole]
			,[ActionRole]
			,[IsApproved]
			,[IsHold]
			,[Remarks]
			,[CreatedDate]
			,[CreatedBy]
			,[CreatedUserID]
		)
		SELECT	D.EntityName AS RequestTypeCode,
				b.EntityId AS RequisitionNo,
				c.StepOrder AS RoutineSequence,
				a.ApproverEmpNo AS AssignedEmpNo,
				RTRIM(f.FirstName) + ' ' + RTRIM(f.MiddleName) + ' ' + RTRIM(f.LastName) AS AssignedEmpName,
				e.ApprovalGroupDesc AS ApprovalRole,
				e.GroupType AS ActionRole,
				@isApproved AS IsApproved,
				@isHold AS IsHold,
				@approverRemarks AS Remarks,
				GETDATE() AS CreatedDate,
				@approverEmpNo AS CreatedBy,
				@approverUserID AS CreatedUserID
		FROM kenuser.WorkflowStepInstances a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowInstances b WITH (NOLOCK) ON a.WorkflowInstanceId = b.WorkflowInstanceId
			INNER JOIN kenuser.WorkflowStepDefinitions c WITH (NOLOCK) ON a.StepDefinitionId = c.StepDefinitionId
			INNER JOIN kenuser.WorkflowDefinitions d WITH (NOLOCK) ON c.WorkflowDefinitionId = d.WorkflowDefinitionId
			INNER JOIN kenuser.WorkflowApprovalRoles e WITH (NOLOCK) ON RTRIM(c.ApprovalRole) = RTRIM(e.ApprovalGroupCode)
			LEFT JOIN kenuser.Employee f WITH (NOLOCK) ON a.ApproverEmpNo = f.EmployeeNo
		WHERE a.StepInstanceId = @stepInstanceId
	END 

END 

/*	Debug:

	EXEC kenuser.Pr_ApprovalHistory_CRUD 1, 17, 10003632, 'ervin', 1, 0, 'Test approval'

PARAMETERS:
	@actionType			TINYINT,	--(Notes: 0 = Check records, 1 = Insert, 2 = Update, 3 = Delete)	
	@stepInstanceId		INT,
	@approverEmpNo		INT,
	@approverUserID		VARCHAR(50),
	@isApproved			BIT,
	@isHold				BIT, 
	@approverRemarks	VARCHAR(500)

	

*/

/*	Manage data:

	SELECT * FROM kenuser.RequestApprovals a

	BEGIN TRAN T1

	TRUNCATE TABLE kenuser.RequestApprovals

	COMMIT TRAN T1

*/
