/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Vw_RequestDetail
*	Description: Get the consolidated request details
*
*	Date			Author		Rev. #		Comments:
*	04/05/2026		Ervin		1.0			Created
*	30/05/2026		Ervin		1.1			Populated return dataset for Regularization Request
*	09/06/2026		Ervin		1.2			Populated return dataset for Extra Time Request
*	19/06/2026		Ervin		1.3			Added the following fields in the returned dataset: CurrentlyAssignedEmpNo, CurrentlyAssignedEmpName
*	26/06/2026		Ervin		1.4			Added "StatusCode" and "StatusDesc" fields		
*	30/06/2026		Ervin		1.5			Populated return dataset for Outdoor Request
******************************************************************************************************************************************************************************/

ALTER VIEW kenuser.Vw_RequestDetail
AS 

	--Leave Requisitions
	SELECT	DISTINCT
			'RTYPELEAVE' AS RequestTypeCode,
			'Leave Requisition' AS RequestTypeDesc,
			a.LeaveRequestId AS RequestNo,
			a.LeaveStartDate AS RequestDate,
			a.LeaveEmpNo AS OrigEmpNo,
			a.LeaveEmpName AS OrigEmpName,
			RTRIM(a.LeaveEmpCostCenter) AS CostCenter,
			RTRIM(dep.DepartmentName) AS CostCenterName,
			a.LeaveCreatedDate AS AppliedDate,
			a.LeaveCreatedBy AS RequestedByNo,	
			RTRIM(ISNULL(b.FirstName, '')) + ' ' + RTRIM(ISNULL(b.MiddleName, '')) + ' ' + RTRIM(ISNULL(b.LastName, '')) AS RequestedByName,			
			'Leave Type: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
				'Employee: ' + a.LeaveEmpName + CHAR(13) + CHAR(10) + 
				'Leave Start Date: ' + FORMAT(a.LeaveStartDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
				'Leave Resume Date: ' + FORMAT(a.LeaveResumeDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) AS RequestDetail,
			a.LeaveCreatedBy AS CreatedByEmpNo,		--Rev. #1.2
			a.LeaveStatusCode AS StatusCode,		--Rev. #1.4
			stat.UDCDesc1 as StatusDesc, 			--Rev. #1.4
			a.StatusHandlingCode,
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverNo ELSE NULL END AS CurrentlyAssignedEmpNo,			--Rev. #1.3
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverName ELSE NULL END AS CurrentlyAssignedEmpName		--Rev. #1.3
	FROM kenuser.LeaveRequisitionWF a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITh (NOLOCK) ON a.LeaveCreatedBy = b.EmployeeNo
		LEFT JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.LeaveEmpCostCenter) = RTRIM(dep.DepartmentCode)
		CROSS APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVETYPES')
				AND RTRIM(UDCCode) = RTRIM(a.LeaveType)
		) udc 
		OUTER APPLY		--Rev. #1.3
		(
			SELECT	x.ApproverEmpNo AS ApproverNo, 
					RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName
			FROM kenuser.WorkflowStepInstances x
				INNER JOIN kenuser.WorkflowInstances y WITH (NOLOCK) ON x.WorkflowInstanceId = y.WorkflowInstanceId
				INNER JOIN kenuser.WorkflowDefinitions z WITH (NOLOCK) ON y.WorkflowDefinitionId = z.WorkflowDefinitionId
				LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON x.ApproverEmpNo = emp.EmployeeNo
			WHERE RTRIM(z.EntityName) = 'RTYPELEAVE'
				AND RTRIM(x.[Status]) = 'Pending'
				AND y.EntityId = a.LeaveRequestId
		) wf
		OUTER APPLY
		(
			SELECT y.* 
			FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
				INNER JOIN kenuser.UserDefinedCode y WITH (NOLOCK) ON x.UDCGroupId = y.GroupID
			where x.UDCGCode = 'STATUS'
				AND y.UDCCode = a.LeaveStatusCode
		) stat

	UNION

	--Regularization Request (Rev. #1.1)
	SELECT	DISTINCT
			'RTYPEREGULAR' AS RequestTypeCode,
			'Regularization' AS RequestTypeDesc,
			a.RegularizationId AS RequestNo,
			a.AttendanceDate AS RequestDate,
			a.EmployeeNo AS OrigEmpNo,
			a.EmployeeName AS OrigEmpName,
			a.CostCenter,
			RTRIM(dep.DepartmentName) AS CostCenterName,
			a.CreatedDate AS AppliedDate,
			a.CreatedBy AS RequestedByNo,	
			RTRIM(ISNULL(b.FirstName, '')) + ' ' + RTRIM(ISNULL(b.MiddleName, '')) + ' ' + RTRIM(ISNULL(b.LastName, '')) AS RequestedByName,			
			'Regularization Reason: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
				'Employee: ' + a.EmployeeName + CHAR(13) + CHAR(10) + 
				'Attendance Date: ' + FORMAT(a.AttendanceDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
				'Total Hours: ' + ISNULL(dur.WorkDurationText, '') + CHAR(13) + CHAR(10) +
				'Deficit Hours: ' + ISNULL(dur.NPHText, '') + CHAR(13) + CHAR(10) +
				'Regularized Time In: ' + FORMAT(CAST(a.RegularizedTimeIn AS DATETIME), 'hh:mm tt') + CHAR(13) + CHAR(10) +
				'Regularized Time Out: ' + FORMAT(CAST(a.RegularizedTimeOut AS DATETIME), 'hh:mm tt')  + CHAR(13) + CHAR(10) AS RequestDetail,
			a.CreatedBy AS CreatedByEmpNo,
			a.StatusCode,					--Rev. #1.4
			stat.UDCDesc1 as StatusDesc, 	--Rev. #1.4
			a.StatusHandlingCode,
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverNo ELSE NULL END AS CurrentlyAssignedEmpNo,			--Rev. #1.3
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverName ELSE NULL END AS CurrentlyAssignedEmpName		--Rev. #1.3
	FROM kenuser.RegularRequestWFs a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITh (NOLOCK) ON a.CreatedBy = b.EmployeeNo
		LEFT JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.CostCenter) = RTRIM(dep.DepartmentCode)
		CROSS APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'ROATYPE')
				AND RTRIM(UDCCode) = RTRIM(a.ROACode)
		) udc 
		CROSS APPLY
		(
			SELECT
				RIGHT('00' + CAST(x.WorkDuration / 60 AS VARCHAR(2)), 2)
					+ ':' + RIGHT('00' + CAST(x.WorkDuration % 60 AS VARCHAR(2)), 2) AS WorkDurationText,
				RIGHT('00' + CAST(x.NoPayHours / 60 AS VARCHAR(2)), 2)
					+ ':' + RIGHT('00' + CAST(x.NoPayHours % 60 AS VARCHAR(2)), 2) AS NPHText
			FROM kenuser.RegularRequestWFs x WITH (NOLOCK)
			WHERE x.RegularizationId = a.RegularizationId
		) dur
		OUTER APPLY		--Rev. #1.3
		(
			SELECT	x.ApproverEmpNo AS ApproverNo, 
					RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName
			FROM kenuser.WorkflowStepInstances x
				INNER JOIN kenuser.WorkflowInstances y WITH (NOLOCK) ON x.WorkflowInstanceId = y.WorkflowInstanceId
				INNER JOIN kenuser.WorkflowDefinitions z WITH (NOLOCK) ON y.WorkflowDefinitionId = z.WorkflowDefinitionId
				LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON x.ApproverEmpNo = emp.EmployeeNo
			WHERE RTRIM(z.EntityName) = 'RTYPEREGULAR'
				AND RTRIM(x.[Status]) = 'Pending'
				AND y.EntityId = a.RegularizationId
		) wf
		OUTER APPLY
		(
			SELECT y.* 
			FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
				INNER JOIN kenuser.UserDefinedCode y WITH (NOLOCK) ON x.UDCGroupId = y.GroupID
			where x.UDCGCode = 'STATUS'
				AND y.UDCCode = a.[StatusCode]
		) stat

	UNION

	--Extra Time Request (Rev. #1.2)
	SELECT	DISTINCT
			'RTYPEOT' AS RequestTypeCode,
			'Extra Time' AS RequestTypeDesc,
			a.ExtratimeId AS RequestNo,
			a.AttendanceDate AS RequestDate,
			a.EmployeeNo AS OrigEmpNo,
			a.EmployeeName AS OrigEmpName,
			a.CostCenter,
			RTRIM(dep.DepartmentName) AS CostCenterName,
			a.CreatedDate AS AppliedDate,
			a.CreatedBy AS RequestedByNo,	
			RTRIM(ISNULL(b.FirstName, '')) + ' ' + RTRIM(ISNULL(b.MiddleName, '')) + ' ' + RTRIM(ISNULL(b.LastName, '')) AS RequestedByName,			
			'Overtime Reason: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
				'Employee: ' + a.EmployeeName + CHAR(13) + CHAR(10) + 
				'Attendance Date: ' + FORMAT(a.AttendanceDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
				'Total Hours: ' + ISNULL(dur.WorkDurationText, '') + CHAR(13) + CHAR(10) +
				'Overtime Hours: ' + ISNULL(dur.OTDurationText, '') + CHAR(13) + CHAR(10) +
				'OT Start Time: ' + FORMAT(CAST(a.OTStartTime AS DATETIME), 'hh:mm tt') + CHAR(13) + CHAR(10) +
				'OT End Time: ' + FORMAT(CAST(a.OTEndTime AS DATETIME), 'hh:mm tt')  + CHAR(13) + CHAR(10) AS RequestDetail,
			a.CreatedBy AS CreatedByEmpNo,
			a.StatusCode,					--Rev. #1.4
			stat.UDCDesc1 as StatusDesc, 	--Rev. #1.4
			a.StatusHandlingCode,
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverNo ELSE NULL END AS CurrentlyAssignedEmpNo,			--Rev. #1.3
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverName ELSE NULL END AS CurrentlyAssignedEmpName		--Rev. #1.3
	FROM kenuser.OTRequestWF a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITh (NOLOCK) ON a.CreatedBy = b.EmployeeNo
		LEFT JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.CostCenter) = RTRIM(dep.DepartmentCode)
		CROSS APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'OTREASON')
				AND RTRIM(UDCCode) = RTRIM(a.OTReasonCode)
		) udc 
		CROSS APPLY
		(
			SELECT
				RIGHT('00' + CAST(x.WorkDuration / 60 AS VARCHAR(2)), 2)
					+ ':' + RIGHT('00' + CAST(x.WorkDuration % 60 AS VARCHAR(2)), 2) AS WorkDurationText,
				RIGHT('00' + CAST(x.OTDuration / 60 AS VARCHAR(2)), 2)
					+ ':' + RIGHT('00' + CAST(x.OTDuration % 60 AS VARCHAR(2)), 2) AS OTDurationText
			FROM kenuser.OTRequestWF x WITH (NOLOCK)
			WHERE x.ExtratimeId = a.ExtratimeId
		) dur
		OUTER APPLY		--Rev. #1.3
		(
			SELECT	x.ApproverEmpNo AS ApproverNo, 
					RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName
			FROM kenuser.WorkflowStepInstances x
				INNER JOIN kenuser.WorkflowInstances y WITH (NOLOCK) ON x.WorkflowInstanceId = y.WorkflowInstanceId
				INNER JOIN kenuser.WorkflowDefinitions z WITH (NOLOCK) ON y.WorkflowDefinitionId = z.WorkflowDefinitionId
				LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON x.ApproverEmpNo = emp.EmployeeNo
			WHERE RTRIM(z.EntityName) = 'RTYPEOT'
				AND RTRIM(x.[Status]) = 'Pending'
				AND y.EntityId = a.ExtratimeId
		) wf
		OUTER APPLY
		(
			SELECT y.* 
			FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
				INNER JOIN kenuser.UserDefinedCode y WITH (NOLOCK) ON x.UDCGroupId = y.GroupID
			where x.UDCGCode = 'STATUS'
				AND y.UDCCode = a.[StatusCode]
		) stat

	UNION

	--Outdoor Request (Rev. #1.5)
	SELECT	DISTINCT
			'RTYPEOUTDOOR' AS RequestTypeCode,
			'Outdoor' AS RequestTypeDesc,
			a.OutdoorId AS RequestNo,
			a.StartDate AS RequestDate,
			a.EmpNo AS OrigEmpNo,
			a.EmpName AS OrigEmpName,
			a.CostCenter,
			RTRIM(dep.DepartmentName) AS CostCenterName,
			a.CreatedDate AS AppliedDate,
			a.CreatedBy AS RequestedByNo,	
			RTRIM(ISNULL(b.FirstName, '')) + ' ' + RTRIM(ISNULL(b.MiddleName, '')) + ' ' + RTRIM(ISNULL(b.LastName, '')) AS RequestedByName,			
			'Outdoor Type: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) +
				'Employee: ' + a.EmpName + CHAR(13) + CHAR(10) +
				'Start Date: ' + CASE WHEN a.StartDate IS NOT NULL THEN FORMAT(a.StartDate, 'dd-MMM-yyyy') ELSE '' END + CHAR(13) + CHAR(10) +
				'End Date: ' + CASE WHEN a.EndDate IS NOT NULL THEN FORMAT(a.EndDate, 'dd-MMM-yyyy') ELSE '' END + CHAR(13) + CHAR(10) +
				'Day of Week: ' + RTRIM(ISNULL(dow.UDCDesc1, '')) + CHAR(13) + CHAR(10) +
				'Start Time: ' + CASE WHEN a.StartTime IS NOT NULL THEN FORMAT(CAST(a.StartTime AS DATETIME), 'hh:mm tt') ELSE '' END + CHAR(13) + CHAR(10) +
				'End Time: ' + CASE WHEN a.EndTime IS NOT NULL THEN FORMAT(CAST(a.EndTime AS DATETIME), 'hh:mm tt') ELSE '' END + CHAR(13) + CHAR(10) AS RequestDetail,
			a.CreatedBy AS CreatedByEmpNo,
			a.StatusCode,					
			stat.UDCDesc1 as StatusDesc, 	
			a.StatusHandlingCode,
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverNo ELSE NULL END AS CurrentlyAssignedEmpNo,			
			CASE WHEN RTRIM(a.StatusHandlingCode) = 'Open' THEN wf.ApproverName ELSE NULL END AS CurrentlyAssignedEmpName		
	FROM kenuser.OutdoorRequestWF a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITh (NOLOCK) ON a.CreatedBy = b.EmployeeNo
		LEFT JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.CostCenter) = RTRIM(dep.DepartmentCode)
		CROSS APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'ROATYPE')
				AND RTRIM(UDCCode) = RTRIM(a.ROACode)
		) udc 
		OUTER APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'DOWTYPES')
				AND RTRIM(UDCCode) = RTRIM(a.DOWCode)
		) dow
		OUTER APPLY		
		(
			SELECT	x.ApproverEmpNo AS ApproverNo, 
					RTRIM(ISNULL(emp.FirstName, '')) + ' ' + RTRIM(ISNULL(emp.MiddleName, '')) + ' ' + RTRIM(ISNULL(emp.LastName, '')) AS ApproverName
			FROM kenuser.WorkflowStepInstances x
				INNER JOIN kenuser.WorkflowInstances y WITH (NOLOCK) ON x.WorkflowInstanceId = y.WorkflowInstanceId
				INNER JOIN kenuser.WorkflowDefinitions z WITH (NOLOCK) ON y.WorkflowDefinitionId = z.WorkflowDefinitionId
				LEFT JOIN kenuser.Employee emp WITH (NOLOCK) ON x.ApproverEmpNo = emp.EmployeeNo
			WHERE RTRIM(z.EntityName) = 'RTYPEOUTDOOR'
				AND RTRIM(x.[Status]) = 'Pending'
				AND y.EntityId = a.OutdoorId
		) wf
		OUTER APPLY
		(
			SELECT y.* 
			FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
				INNER JOIN kenuser.UserDefinedCode y WITH (NOLOCK) ON x.UDCGroupId = y.GroupID
			where x.UDCGCode = 'STATUS'
				AND y.UDCCode = a.[StatusCode]
		) stat

GO 

/*	Debug:

	SELECT * FROM kenuser.Vw_RequestDetail a
	-- WHERE RTRIM(a.RequestTypeCode) = 'RTYPEOT'
	ORDER BY a.RequestTypeCode, a.RequestNo

	SELECT * FROM kenuser.Vw_RequestDetail a
	WHERE RTRIM(a.RequestTypeCode) = 'RTYPEREGULAR'
		AND a.RequestNo = 6

*/