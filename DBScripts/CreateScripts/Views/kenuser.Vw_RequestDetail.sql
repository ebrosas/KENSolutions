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
			a.StatusHandlingCode			
	FROM kenuser.LeaveRequisitionWF a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITh (NOLOCK) ON a.LeaveCreatedBy = b.EmployeeNo
		LEFT JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.LeaveEmpCostCenter) = RTRIM(dep.DepartmentCode)
		CROSS APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVETYPES')
				AND RTRIM(UDCCode) = RTRIM(a.LeaveType)
		) udc 

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
			'Reason: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
				'Employee: ' + a.EmployeeName + CHAR(13) + CHAR(10) + 
				'Attendance Date: ' + FORMAT(a.AttendanceDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
				'Total Hours: ' + ISNULL(dur.WorkDurationText, '') + CHAR(13) + CHAR(10) +
				'Deficit Hours: ' + ISNULL(dur.NPHText, '') + CHAR(13) + CHAR(10) +
				'Regularized Time In: ' + FORMAT(CAST(a.RegularizedTimeIn AS DATETIME), 'hh:mm tt') + CHAR(13) + CHAR(10) +
				'Regularized Time Out: ' + FORMAT(CAST(a.RegularizedTimeOut AS DATETIME), 'hh:mm tt')  + CHAR(13) + CHAR(10) AS RequestDetail,
		a.CreatedBy AS CreatedByEmpNo,
		a.StatusHandlingCode
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
			'Reason: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
				'Employee: ' + a.EmployeeName + CHAR(13) + CHAR(10) + 
				'Attendance Date: ' + FORMAT(a.AttendanceDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
				'Total Hours: ' + ISNULL(dur.WorkDurationText, '') + CHAR(13) + CHAR(10) +
				'Overtime Hours: ' + ISNULL(dur.OTDurationText, '') + CHAR(13) + CHAR(10) +
				'OT Start Time: ' + FORMAT(CAST(a.OTStartTime AS DATETIME), 'hh:mm tt') + CHAR(13) + CHAR(10) +
				'OT End Time: ' + FORMAT(CAST(a.OTEndTime AS DATETIME), 'hh:mm tt')  + CHAR(13) + CHAR(10) AS RequestDetail,
		a.CreatedBy AS CreatedByEmpNo,
		a.StatusHandlingCode
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

GO 

/*	Debug:

	SELECT * FROM kenuser.Vw_RequestDetail a
	WHERE RTRIM(a.RequestTypeCode) = 'RTYPEOT'
	ORDER BY a.RequestTypeCode, a.RequestNo

*/