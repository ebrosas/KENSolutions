/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetAttendanceDetails
*	Description: Get the employee's attendance details on the specified date
*
*	Date			Author		Rev. #		Comments:
*	22/05/2026		Ervin		1.0			Created
*
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetAttendanceDetails
(   		
	@empNo				INT,
	@attendanceDate		DATETIME 
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	SELECT	@attendanceDate AS AttendanceDate,
			a.EmployeeNo,
			RTRIM(ISNULL(a.FirstName, '')) + RTRIM(ISNULL(a.MiddleName, '')) + ' ' + RTRIM(ISNULL(a.LastName, '')) AS EmployeeName,					
			RTRIM(b.ShiftPatternCode) AS ShiftRoster,
			b.ShiftPatternDescription AS ShiftRosterDesc,			
			FORMAT(CAST(b.SchedTimeIn AS DATETIME), 'hh:mm tt') + ' - ' + FORMAT(CAST(b.SchedTimeOut AS DATETIME), 'hh:mm tt') AS ShiftTiming,
			ISNULL(g.TotalDeficitHour, 0) AS TotalDeficitHour,
			ISNULL(g.TotalWorkHour, 0) AS TotalWorkHour,
			isnull(g.TotalWorkMinute, 0) AS TotalWorkMinute
	FROM kenuser.Employee a WITH (NOLOCK)
		OUTER APPLY 
		(
			SELECT x.ShiftPatternCode, spt.ShiftPatternDescription, y.ShiftCode, y.ShiftDescription, z.ArrivalTo AS SchedTimeIn, z.DepartFrom AS SchedTimeOut 
			FROM kenuser.ShiftPatternChange x WITH (NOLOCK) 
				INNER JOIN kenuser.MasterShiftPattern y WITH (NOLOCK) ON RTRIM(x.ShiftPatternCode) = RTRIM(y.ShiftPatternCode) AND x.ShiftPointer = y.ShiftPointer
				INNER JOIN kenuser.MasterShiftTime z WITH (NOLOCK) ON RTRIM(y.ShiftPatternCode) = RTRIM(z.ShiftPatternCode) --AND RTRIM(y.ShiftCode) = RTRIM(z.ShiftCode)
				INNER JOIN kenuser.MasterShiftPatternTitle spt WITH (NOLOCK) ON RTRIM(x.ShiftPatternCode) = RTRIM(spt.ShiftPatternCode)
			WHERE x.EmpNo = a.EmployeeNo
		) b
		OUTER APPLY	
		(
			SELECT
				-- Total deficit hours
				CAST(SUM(ISNULL(NoPayHours, 0)) AS DECIMAL) / 60 AS TotalDeficitHour,

				-- Total work hours
				CAST(SUM(ISNULL(DurationWorkedCumulative, 0)) AS DECIMAL) / 60 AS TotalWorkHour,

				-- Total work minutes
				SUM(ISNULL(x.DurationWorkedCumulative, 0)) AS TotalWorkMinute
			FROM kenuser.AttendanceTimesheet x WITH (NOLOCK)
			WHERE x.EmpNo = a.EmployeeNo
			  AND x.AttendanceDate = @attendanceDate
		) g
	WHERE a.EmployeeNo = @empNo

END 

/*	Debug:

	EXEC kenuser.Pr_GetAttendanceDetails 10003632, '05/22/2026'
	EXEC kenuser.Pr_GetAttendanceDetails 10003632, '05/18/2026'

PARAMETERS:
	@empNo				INT,
	@attendanceDate		DATETIME 

*/