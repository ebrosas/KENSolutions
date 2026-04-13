
	--Application Request Types
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'REQTYPE')

	SELECT * FROM kenuser.RequestApprovals a WITH (NOLOCK)

	SELECT * FROM [kenuser].[LeaveRequisitionWF] a

	SELECT a.EmployeeNo, a.ReportingManagerCode, a.SecondReportingManagerCode, 
	* FROM kenuser.Employee a
	ORDER BY a.EmployeeNo

	SELECT * FROM [kenuser].[WorkflowApprovalRoles] a WITH (NOLOCK)
	SELECT * FROM kenuser.WorkflowDefinitions a
	SELECT * FROM [kenuser].[WorkflowConditions] A
	SELECT * FROM kenuser.WorkflowStepDefinitions a
	SELECT * FROM kenuser.WorkflowInstances a
	SELECT * FROM kenuser.WorkflowStepInstances a
