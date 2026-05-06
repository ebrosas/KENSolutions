/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetDashboardStatistics
*	Description: Get the list of pending, approved, rejected, and on-hold requests based on the employee number
*
*	Date			Author		Rev. #		Comments:
*	25/04/2026		Ervin		1.0			Created
*	06/05/2026		Ervin		1.1			Added "Remarks" field in the returned dataset
*
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetDashboardStatistics
(   
	@searchType		TINYINT,			--(Notes: 0 = All, 1 = Pending request, 2 = Approved request, 3 = Rejected request, 4 = On-hold request)
	@empNo			INT = 0,	
	@requestType	VARCHAR(100) = ''
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	IF ISNULL(@searchType, '') = '' 
		SET @searchType = NULL

	IF ISNULL(@requestType, '') = '' OR RTRIM(@requestType) = '<ALL>'
		SET @requestType = NULL

	IF @searchType = 1
	BEGIN

		SELECT	DISTINCT			
				c.EntityId AS RequestNo,
				RTRIM(b.EntityName) AS RequestTypeCode,
				udc.RequestTypeDesc,
				req.AppliedDate,
				req.RequestedByNo,
				req.RequestedByName,
				req.LeaveDetails AS Detail,
				req.CreatedByEmpNo,
				a.ApprovalStageDesc as ApprovalRole,			
				d.ActivityStatus AS CurrentStatus,
				d.ApproverNo,
				d.ApproverName,
				d.PendingDays,
				d.StepInstanceId,
				'' AS Remarks			--Rev. #1.1
		FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
			INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
			INNER JOIN kenuser.WorkflowInstances c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
			CROSS APPLY
			(
				SELECT RTRIM(x.UDCDesc1) AS RequestTypeDesc  
				FROM kenuser.UserDefinedCode x WITH (NOLOCK)
				WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
					AND RTRIM(x.UDCCode) = RTRIM(b.EntityName)
			) udc 
			INNER JOIN kenuser.Vw_RequestDetail req WITH (NOLOCK) ON req.RequestNo = c.EntityId
			OUTER APPLY
			(
				SELECT	x.[Status] as ActivityStatus,
						x.ApproverEmpNo AS ApproverNo,
						RTRIM(ISNULL(y.FirstName, '')) + ' ' + RTRIM(ISNULL(y.MiddleName, '')) + ' ' + RTRIM(ISNULL(y.LastName, '')) AS ApproverName,
						DATEDIFF(DAY, x.ActionDate, GETDATE()) AS PendingDays,
						x.StepInstanceId		--Rev. #1.1
				FROM kenuser.WorkflowStepInstances x WITH (NOLOCK)
					LEFT JOIN kenuser.Employee y WITH (NOLOCK) ON x.ApproverEmpNo = y.EmployeeNo
				WHERE x.WorkflowInstanceId = c.WorkflowInstanceId
					and x.StepDefinitionId = a.StepDefinitionId
			) d
		WHERE RTRIM(d.ActivityStatus) = 'Pending'
			AND (d.ApproverNo = @empNo OR @empNo IS NULL)
			AND (RTRIM(b.EntityName) = @requestType OR @requestType IS NULL)
		ORDER BY c.EntityId DESC
	END 

	ELSE IF @searchType = 2
	BEGIN

		SELECT	app.RequisitionNo AS RequestNo,
				app.RequestTypeCode,
				udc.RequestTypeDesc,
				req.AppliedDate,
				req.RequestedByNo,
				req.RequestedByName,
				req.LeaveDetails AS Detail,
				req.CreatedByEmpNo,
				wf.ApprovalRole,
				--wf.CurrentStatus,
				'Approved' AS CurrentStatus,
				wf.ApproverNo,
				wf.ApproverName,
				wf.PendingDays,
				wf.StepInstanceId,
				app.Remarks				--Rev. #1.1
		FROM kenuser.RequestApprovals app WITH (NOLOCK)
			CROSS APPLY
			(
				SELECT RTRIM(x.UDCDesc1) AS RequestTypeDesc  
				FROM kenuser.UserDefinedCode x WITH (NOLOCK)
				WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
					AND RTRIM(x.UDCCode) = RTRIM(app.RequestTypeCode)
			) udc 
			INNER JOIN kenuser.Vw_RequestDetail req WITH (NOLOCK) ON app.RequisitionNo = req.RequestNo
			CROSS APPLY 
			(
				SELECT	c.ApprovalRole,
						b.[Status] as CurrentStatus,
						b.ApproverEmpNo AS ApproverNo,
						RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName,
						DATEDIFF(DAY, b.ActionDate, GETDATE()) AS PendingDays,
						b.StepInstanceId
				FROM kenuser.WorkflowInstances a WITH (NOLOCK) 
					INNER JOIN kenuser.WorkflowStepInstances b WITH (NOLOCK) ON a.WorkflowInstanceId = b.WorkflowInstanceId AND RTRIM(b.[Status]) = 'Approved'
					INNER JOIN kenuser.WorkflowStepDefinitions c WITH (NOLOCK) ON b.StepDefinitionId = c.StepDefinitionId
					LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON b.ApproverEmpNo = emp.EmployeeNo
				WHERE a.EntityId = app.RequisitionNo 
			) wf
		WHERE app.IsApproved = 1
			AND (app.AssignedEmpNo = @empNo OR @empNo IS NULL)
			AND (RTRIM(app.RequestTypeCode) = @requestType OR @requestType IS NULL)
		ORDER BY app.RequisitionNo DESC
	END 

	ELSE IF @searchType = 3
	BEGIN

		SELECT	app.RequisitionNo AS RequestNo,
				app.RequestTypeCode,
				udc.RequestTypeDesc,
				req.AppliedDate,
				req.RequestedByNo,
				req.RequestedByName,
				req.LeaveDetails AS Detail,
				req.CreatedByEmpNo,
				wf.ApprovalRole,
				--wf.CurrentStatus,
				'Rejected' AS CurrentStatus,
				wf.ApproverNo,
				wf.ApproverName,
				wf.PendingDays,
				wf.StepInstanceId,
				app.Remarks				--Rev. #1.1
		FROM kenuser.RequestApprovals app WITH (NOLOCK)
			CROSS APPLY
			(
				SELECT RTRIM(x.UDCDesc1) AS RequestTypeDesc  
				FROM kenuser.UserDefinedCode x WITH (NOLOCK)
				WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
					AND RTRIM(x.UDCCode) = RTRIM(app.RequestTypeCode)
			) udc 
			INNER JOIN kenuser.Vw_RequestDetail req WITH (NOLOCK) ON app.RequisitionNo = req.RequestNo
			CROSS APPLY 
			(
				SELECT	c.ApprovalRole,
						b.[Status] as CurrentStatus,
						b.ApproverEmpNo AS ApproverNo,
						RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName,
						DATEDIFF(DAY, b.ActionDate, GETDATE()) AS PendingDays,
						b.StepInstanceId
				FROM kenuser.WorkflowInstances a WITH (NOLOCK) 
					INNER JOIN kenuser.WorkflowStepInstances b WITH (NOLOCK) ON a.WorkflowInstanceId = b.WorkflowInstanceId AND RTRIM(b.[Status]) = 'Rejected'
					INNER JOIN kenuser.WorkflowStepDefinitions c WITH (NOLOCK) ON b.StepDefinitionId = c.StepDefinitionId
					LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON b.ApproverEmpNo = emp.EmployeeNo
				WHERE a.EntityId = app.RequisitionNo 
			) wf
		WHERE ISNULL(app.IsApproved, 0) = 0
			AND (app.AssignedEmpNo = @empNo OR @empNo IS NULL)
			AND (RTRIM(app.RequestTypeCode) = @requestType OR @requestType IS NULL)
		ORDER BY app.RequisitionNo DESC
	END 

	ELSE IF @searchType = 4
	BEGIN

		SELECT	app.RequisitionNo AS RequestNo,
				app.RequestTypeCode,
				udc.RequestTypeDesc,
				req.AppliedDate,
				req.RequestedByNo,
				req.RequestedByName,
				req.LeaveDetails AS Detail,
				req.CreatedByEmpNo,
				wf.ApprovalRole,
				--wf.CurrentStatus,
				'On-hold' AS CurrentStatus,
				wf.ApproverNo,
				wf.ApproverName,
				wf.PendingDays,
				wf.StepInstanceId,
				app.Remarks					--Rev. #1.1
		FROM kenuser.RequestApprovals app WITH (NOLOCK)
			CROSS APPLY
			(
				SELECT RTRIM(x.UDCDesc1) AS RequestTypeDesc  
				FROM kenuser.UserDefinedCode x WITH (NOLOCK)
				WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
					AND RTRIM(x.UDCCode) = RTRIM(app.RequestTypeCode)
			) udc 
			INNER JOIN kenuser.Vw_RequestDetail req WITH (NOLOCK) ON app.RequisitionNo = req.RequestNo
			CROSS APPLY 
			(
				SELECT	c.ApprovalRole,
						b.[Status] as CurrentStatus,
						b.ApproverEmpNo AS ApproverNo,
						RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName,
						DATEDIFF(DAY, b.ActionDate, GETDATE()) AS PendingDays,
						b.StepInstanceId
				FROM kenuser.WorkflowInstances a WITH (NOLOCK) 
					INNER JOIN kenuser.WorkflowStepInstances b WITH (NOLOCK) ON a.WorkflowInstanceId = b.WorkflowInstanceId AND RTRIM(b.[Status]) = 'OnHold'
					INNER JOIN kenuser.WorkflowStepDefinitions c WITH (NOLOCK) ON b.StepDefinitionId = c.StepDefinitionId
					LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON b.ApproverEmpNo = emp.EmployeeNo
				WHERE a.EntityId = app.RequisitionNo 
			) wf
		WHERE app.IsHold = 1
			AND (app.AssignedEmpNo = @empNo OR @empNo IS NULL)
			AND (RTRIM(app.RequestTypeCode) = @requestType OR @requestType IS NULL)
		ORDER BY app.RequisitionNo DESC
	END 

END 

/*	Debug:

	--Staging database
	EXEC kenuser.Pr_GetDashboardStatistics 1
	EXEC kenuser.Pr_GetDashboardStatistics 10003635		
	EXEC kenuser.Pr_GetDashboardStatistics 10003635, 'RTYPELEAVE'

	EXEC kenuser.Pr_GetDashboardStatistics 2, 10003632
	EXEC kenuser.Pr_GetDashboardStatistics 3, 10003632
	
	--Development database
	EXEC kenuser.Pr_GetDashboardStatistics 1
	EXEC kenuser.Pr_GetDashboardStatistics 1, 10003633

	EXEC kenuser.Pr_GetDashboardStatistics 2, 10003632			--Approved
	EXEC kenuser.Pr_GetDashboardStatistics 3, 10003632			--Rejected
	EXEC kenuser.Pr_GetDashboardStatistics 4, 10003632			--Hold

PARAMETERS:
	@searchType		TINYINT,			--(Notes: 0 = All, 1 = Pending request, 2 = Approved request, 3 = Rejected request, 4 = On-hold request)
	@empNo			INT = 0,	
	@requestType	VARCHAR(100) = ''

*/