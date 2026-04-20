
	--Application Request Types
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'REQTYPE')

	SELECT * FROM kenuser.RequestApprovals a WITH (NOLOCK)

	SELECT * FROM [kenuser].[WorkflowApprovalRoles] a

	SELECT a.EmployeeNo, a.UserID, a.ReportingManagerCode, a.SecondReportingManagerCode, 
		a.OfficialEmail, a.PersonalEmail, a.HireDate,
	* FROM kenuser.Employee a
	ORDER BY a.EmployeeNo

	SELECT a.LeaveType, a.LeaveDuration, a.LeaveEmpCostCenter,
	* FROM [kenuser].[LeaveRequisitionWF] a

	select * from kenuser.DepartmentMaster a

	SELECT * FROM [kenuser].[WorkflowApprovalRoles] a WITH (NOLOCK)

	SELECT * FROM [kenuser].[LeaveRequisitionWF] a
	WHERE a.LeaveRequestId = 15

	SELECT * FROM kenuser.Employee a

	SELECT * FROM kenuser.WorkflowDefinitions a
	SELECT * FROM kenuser.WorkflowStepDefinitions a
	SELECT * FROM kenuser.WorkflowConditions a	
	SELECT * FROM kenuser.WorkflowInstances a
	SELECT * FROM kenuser.WorkflowStepInstances a

/*

	TRUNCATE TABLE kenuser.WorkflowDefinitions
	TRUNCATE TABLE kenuser.WorkflowStepDefinitions
	TRUNCATE TABLE kenuser.WorkflowInstances
	TRUNCATE TABLE kenuser.WorkflowStepInstances

	DELETE FROM kenuser.WorkflowDefinitions 	
	DELETE  FROM kenuser.WorkflowStepDefinitions 
	DELETE FROM kenuser.WorkflowInstances 
	DELETE FROM kenuser.WorkflowStepInstances

	BEGIN TRAN T1

	UPDATE kenuser.WorkflowConditions
	SET StepDefinitionId = 25

	UPDATE kenuser.WorkflowDefinitions
	SET EntityName = 'RTYPELEAVE'
	WHERE WorkflowDefinitionId = 7 

	UPDATE kenuser.Employee
	SET UserID = 'anne'
	WHERE EmployeeNo = 10003633

	UPDATE kenuser.Employee
	SET UserID = 'abdulla'
	WHERE EmployeeNo = 10003635

	DELETE FROM kenuser.WorkflowInstances

	DELETE FROM kenuser.WorkflowStepInstances
	WHERE StepInstanceId IN (7, 8)

	UPDATE kenuser.WorkflowStepInstances
	SET ApproverUserID = NULL,
		Status = 'Pending',
		ActionDate = null,
		Comments = null
	WHERE StepInstanceId = 4

	COMMIT TRAN T1

*/