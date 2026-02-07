/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetAttendanceSummary
*	Description: Get the attendance summary of the specified employee
*
*	Date			Author		Rev. #		Comments:
*	11/01/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetAttendanceSummary
(   		
	@empNo				INT,
	@startDate			DATETIME = NULL,
	@endDate			DATETIME = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--SELECT	@empNo AS EmployeeNo,
	--		'ERVIN OLINAS BROSAS' AS EmployeeName,
	--		'D5' AS ShiftRoster,
	--		'08:00 AM - 04:30 PM' AS ShiftTiming,
	--		FORMAT(GETDATE(), 'dd-MMM-yyyy hh:mm tt')  AS ActualTiming,
	--		5 AS TotalAbsent,
	--		3 AS TotalHalfDay,
	--		10 AS TotalLeave,
	--		6 AS TotalLate,
	--		2 AS TotalEarlyOut,
	--		3.5 AS TotalDeficitHour,
	--		150.75 AS TotalWorkHour,
	--		26.0 AS TotalDaysWorked,
	--		8.0 AS AverageWorkHour,
	--		56.0 AS TotalLeaveBalance,
	--		30.0 AS TotalSLBalance,
	--		4.0 AS TotalDILBalance

	SELECT	a.EmployeeNo,
			RTRIM(a.FirstName) + RTRIM(a.MiddleName) + ' ' + RTRIM(a.LastName) AS EmployeeName,
			RTRIM(b.ShiftPatternCode) AS ShiftRoster,
			b.ShiftPatternDescription AS ShiftRosterDesc,
			FORMAT(CAST(b.SchedTimeIn AS DATETIME), 'hh:mm tt') + ' - ' + FORMAT(CAST(b.SchedTimeOut AS DATETIME), 'hh:mm tt') AS ShiftTiming,
			5 AS TotalAbsent,
			3 AS TotalHalfDay,
			ISNULL(CAST(c.LeaveEntitlemnt AS DECIMAL), 0) AS TotalLeave,
			6 AS TotalLate,
			2 AS TotalEarlyOut,
			3.5 AS TotalDeficitHour,
			150.75 AS TotalWorkHour,
			26.0 AS TotalDaysWorked,
			8.0 AS AverageWorkHour,
			56.0 AS TotalLeaveBalance,
			30.0 AS TotalSLBalance,
			4.0 AS TotalDILBalance
	FROM kenuser.Employee a WITH (NOLOCK)
		OUTER APPLY 
		(
			SELECT x.ShiftPatternCode, spt.ShiftPatternDescription, y.ShiftCode, y.ShiftDescription, z.ArrivalTo AS SchedTimeIn, z.DepartFrom AS SchedTimeOut 
			FROM kenuser.ShiftPatternChange x WITH (NOLOCK) 
				INNER JOIN kenuser.MasterShiftPattern y WITH (NOLOCK) ON RTRIM(x.ShiftPatternCode) = RTRIM(y.ShiftPatternCode) AND x.ShiftPointer = y.ShiftPointer
				INNER JOIN kenuser.MasterShiftTime z WITH (NOLOCK) ON RTRIM(y.ShiftCode) = RTRIM(z.ShiftCode)
				INNER JOIN kenuser.MasterShiftPatternTitle spt WITH (NOLOCK) ON RTRIM(x.ShiftPatternCode) = RTRIM(spt.ShiftPatternCode)
			WHERE x.EmpNo = a.EmployeeNo
		) b
		LEFT JOIN kenuser.LeaveEntitlement c WITH (NOLOCK) ON a.EmployeeNo = c.EmployeeNo
	WHERE a.EmployeeNo = @empNo
	
	
END

/*	Debug:

PARAMETERS:
	@empNo				INT,
	@startDate			DATETIME,
	@endDate			DATETIME

	EXEC kenuser.Pr_GetAttendanceSummary 10003632, '01/16/2026', '02/15/2026'

*/