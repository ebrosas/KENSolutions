/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Vw_RequestDetail
*	Description: Get the consolidated request details
*
*	Date			Author		Rev. #		Comments:
*	04/05/2026		Ervin		1.0			Created
*	30/05/2026		Ervin		1.1			Populated return dataset for Regularization Request
******************************************************************************************************************************************************************************/

ALTER VIEW kenuser.Vw_RequestDetail
AS 

	--Leave Requisitions
	SELECT	DISTINCT
			a.LeaveRequestId AS RequestNo,
			a.LeaveCreatedDate AS AppliedDate,
			a.LeaveCreatedBy AS RequestedByNo,	
			RTRIM(ISNULL(b.FirstName, '')) + ' ' + RTRIM(ISNULL(b.MiddleName, '')) + ' ' + RTRIM(ISNULL(b.LastName, '')) AS RequestedByName,
			'Leave Type: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
				'Originator Employee: ' + a.LeaveEmpName + CHAR(13) + CHAR(10) + 
				'Leave Start Date: ' + FORMAT(a.LeaveStartDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
				'Leave Resume Date: ' + FORMAT(a.LeaveResumeDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) AS LeaveDetails,
			a.LeaveCreatedBy AS CreatedByEmpNo,		--Rev. #1.2
			a.StatusHandlingCode
	FROM kenuser.LeaveRequisitionWF a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITh (NOLOCK) ON a.LeaveCreatedBy = b.EmployeeNo
		CROSS APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'LEAVETYPES')
				AND RTRIM(UDCCode) = RTRIM(a.LeaveType)
		) udc 

	UNION

	--Regularization Request (Rev. #1.1)
	SELECT	DISTINCT
			a.RegularizationId AS RequestNo,
			a.CreatedDate AS AppliedDate,
			a.CreatedBy AS RequestedByNo,	
			RTRIM(ISNULL(b.FirstName, '')) + ' ' + RTRIM(ISNULL(b.MiddleName, '')) + ' ' + RTRIM(ISNULL(b.LastName, '')) AS RequestedByName,
			'Regularization Reason: ' + RTRIM(udc.UDCDesc1) + CHAR(13) + CHAR(10) + 
			'Originator Employee: ' + a.EmployeeName + CHAR(13) + CHAR(10) + 
			'Attendance Date: ' + FORMAT(a.AttendanceDate, 'dd-MMM-yyyy') + CHAR(13) + CHAR(10) +
			'Regularized Time In: ' + FORMAT(CAST(a.RegularizedTimeIn AS DATETIME), 'hh:mm tt') + CHAR(13) + CHAR(10) +
			'Regularized Time Out: ' + FORMAT(CAST(a.RegularizedTimeOut AS DATETIME), 'hh:mm tt')  + CHAR(13) + CHAR(10) AS LeaveDetails,
		a.CreatedBy AS CreatedByEmpNo,
		a.StatusHandlingCode
	FROM kenuser.RegularRequestWFs a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITh (NOLOCK) ON a.CreatedBy = b.EmployeeNo
		CROSS APPLY
		(
			SELECT * FROM kenuser.UserDefinedCode WITH (NOLOCK)
			WHERE GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'ROATYPE')
				AND RTRIM(UDCCode) = RTRIM(a.ROACode)
		) udc 

GO 

/*	Debug:

	SELECT * FROM kenuser.Vw_RequestDetail a

*/