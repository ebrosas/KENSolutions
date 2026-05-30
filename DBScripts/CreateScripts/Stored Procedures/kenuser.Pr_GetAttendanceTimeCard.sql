/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetAttendanceTimeCard
*	Description: Get the attendance record that will be displayed in the Timecard page
*
*	Date			Author		Rev. #		Comments:
*	30/05/2026		Ervin		1.0			Created
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetAttendanceTimeCard
(   	
	@startDate		DATETIME,
	@endDate		DATETIME,
	@empNo			INT = 0
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	SELECT	DISTINCT
			a.AutoId,
			RTRIM(b.DepartmentCode) AS CostCenter,
			RTRIM(dep.DepartmentName) AS CostCenterName,
			a.EmpNo, 
			RTRIM(b.FirstName) + ' ' + RTRIM(b.MiddleName) + ' ' + RTRIM(b.LastName) as EmployeeName,
			b.Position,
			a.AttendanceDate,
			UPPER(LEFT(DATENAME(WEEKDAY, a.AttendanceDate), 3)) AS DOW,
			a.ShiftPatCode,
			a.SchedShiftCode,
			sp.ShiftDescription,
			FORMAT(CAST(sp.SchedTimeIn AS DATETIME), 'hh:mm tt') + ' - ' + FORMAT(CAST(sp.SchedTimeOut AS DATETIME), 'hh:mm tt') AS ShiftTiming,
			c.FirstTimeIn,
			d.LastTimeOut,			
			a.DurationRequired,
			a.DurationWorkedCumulative AS WorkDuration,
			a.NoPayHours,
			a.RemarkCode,
			a.LeaveType,
			a.AbsenceReasonCode,
			ROA.ROADesc,
			CASE
				WHEN a.IsPublicHoliday = 1 THEN 'Holiday'
				WHEN RTRIM(a.SchedShiftCode) = 'O' THEN 'Weekly Off'
				WHEN RTRIM(a.RemarkCode) = 'A' THEN 'Absent'
				WHEN ISNULL(a.LeaveType, '') <> '' THEN
					CASE WHEN RTRIM(a.LeaveType) = 'AL' THEN 'On Annual Leave'
						WHEN RTRIM(a.LeaveType) = 'SL' THEN 'On Sick Leave'
						WHEN RTRIM(a.LeaveType) = 'IL' THEN 'On Injury Leave'
						ELSE 'On Leave'
					END
				WHEN ISNULL(a.AbsenceReasonCode, '') <> '' THEN ROA.ROADesc
				ELSE 'Present'
			END AS AttendanceStatus,
			'' AS AttendanceRemarks,
			DATEDIFF(MINUTE, a.OTStartTime, a.OTEndTime) AS OTDuration,
			CASE WHEN a.OTStartTime IS NULL OR a.OTEndTime IS NULL THEN 'Not Applicable' ELSE 'Pending' END AS OTStatus
	FROM kenuser.AttendanceTimesheet a WITH (NOLOCK) 
		INNER JOIN kenuser.Employee b WITH (NOLOCK) ON a.EmpNo = b.EmployeeNo
		INNER JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(b.DepartmentCode) = RTRIM(dep.DepartmentCode)
		CROSS APPLY
		(
			SELECT x.ShiftDescription, y.ArrivalTo AS SchedTimeIn, y.DepartFrom AS SchedTimeOut
			FROM kenuser.MasterShiftPattern x WITH (NOLOCK) 
				INNER JOIN kenuser.MasterShiftTime y WITH (NOLOCK) ON RTRIM(x.ShiftPatternCode) = RTRIM(y.ShiftPatternCode) AND RTRIM(x.ShiftCode) = RTRIM(y.ShiftCode)
			WHERE RTRIM(x.ShiftPatternCode) = RTRIM(a.ShiftPatCode)
		) sp
		OUTER APPLY
		(
			SELECT TOP 1 x.TimeIn AS FirstTimeIn
			FROM kenuser.AttendanceTimesheet x WITH (NOLOCK)
			WHERE x.EmpNo = a.EmpNo
				AND x.AttendanceDate = a.AttendanceDate
			ORDER BY x.AutoId ASC
		) c
		OUTER APPLY
		(
			SELECT TOP 1 x.[TimeOut] AS LastTimeOut
			FROM kenuser.AttendanceTimesheet x WITH (NOLOCK)
			WHERE x.EmpNo = a.EmpNo
				AND x.AttendanceDate = a.AttendanceDate
			ORDER BY x.AutoId DESC	
		) d
		OUTER APPLY 
		(
			SELECT x.UDCDesc1 AS ROADesc 
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'ROATYPE')
				AND RTRIM(x.UDCCode) = RTRIM(a.AbsenceReasonCode)
		) ROA
		--OUTER APPLY 
		--(
		--	SELECT x.UDCDesc1 AS JobTitle 
		--	FROM kenuser.UserDefinedCode x WITH (NOLOCK)
		--	WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'JOBTITLE')
		--		AND RTRIM(x.UDCCode) = RTRIM(b.Position)
		--) jt
	WHERE a.IsLastRow = 1
		AND (a.EmpNo = @empNo OR @empNo IS NULL)
		AND a.AttendanceDate BETWEEN @startDate AND @endDate
	ORDER BY a.EmpNo, a.AttendanceDate DESC

END 

/*	Debug:

	EXEC kenuser.Pr_GetAttendanceTimeCard '05/01/2026', '05/31/2026'
	EXEC kenuser.Pr_GetAttendanceTimeCard '05/01/2026', '05/31/2026', 10003632

PARAMETERS:   	
	@startDate		DATETIME,
	@endDate		DATETIME,
	@empNo			INT = 0

*/