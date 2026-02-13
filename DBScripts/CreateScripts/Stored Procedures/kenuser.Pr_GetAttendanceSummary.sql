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

	DECLARE @fiscalYear	INT = YEAR(GETDATE())

	SELECT	a.EmployeeNo,
			RTRIM(a.FirstName) + RTRIM(a.MiddleName) + ' ' + RTRIM(a.LastName) AS EmployeeName,
			RTRIM(b.ShiftPatternCode) AS ShiftRoster,
			b.ShiftPatternDescription AS ShiftRosterDesc,			
			FORMAT(CAST(b.SchedTimeIn AS DATETIME), 'hh:mm tt') + ' - ' + FORMAT(CAST(b.SchedTimeOut AS DATETIME), 'hh:mm tt') AS ShiftTiming,
			ISNULL(g.TotalAbsent, 0) AS TotalAbsent,
			ISNULL(e.TotalHalfDay, 0) AS TotalHalfDay,
			ISNULL(CAST(d.TotalLeave AS DECIMAL), 0) AS TotalLeave,
			kenuser.fnGetLateCount(a.EmployeeNo, @startDate, @endDate, f.ArrivalTo) AS TotalLate,
			kenuser.fnGetEarlyOutCount(a.EmployeeNo, @startDate, @endDate, f.DepartFrom) AS TotalEarlyOut,
			ISNULL(g.TotalDeficitHour, 0) AS TotalDeficitHour,
			ISNULL(g.TotalWorkHour, 0) AS TotalWorkHour,
			ISNULL(g.TotalDaysWorked, 0) AS TotalDaysWorked,
			ISNULL(g.AverageWorkHour, 0) AS AverageWorkHour,
			ISNULL(CAST(c.LeaveBalance AS DECIMAL), 0) AS TotalLeaveBalance,
			ISNULL(CAST(c.SLBalance AS DECIMAL), 0) AS TotalSLBalance,
			ISNULL(CAST(c.DILBalance AS DECIMAL), 0) AS TotalDILBalance
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
		LEFT JOIN kenuser.LeaveEntitlement c WITH (NOLOCK) ON a.EmployeeNo = c.EmployeeNo
		OUTER APPLY
		(
			SELECT SUM(x.LeaveDuration) AS TotalLeave 
			FROM kenuser.LeaveRequisitionWF x WITH (NOLOCK)
			WHERE RTRIM(x.LeaveApprovalFlag) NOT IN ('C', 'R', 'D')
				AND x.LeaveEmpNo = a.EmployeeNo
				AND x.LeaveStartDate BETWEEN @startDate AND @endDate
		) d
		OUTER APPLY
		(
			SELECT COUNT(x.LeaveRequestId) AS TotalHalfDay
			FROM kenuser.LeaveRequisitionWF x WITH (NOLOCK)
			WHERE RTRIM(x.LeaveApprovalFlag) NOT IN ('C', 'R', 'D')
				AND ISNULL(x.HalfDayLeaveFlag, 0) > 0
				AND x.LeaveEmpNo = a.EmployeeNo
				AND x.LeaveStartDate BETWEEN @startDate AND @endDate
		) e
		OUTER APPLY	
		(
			SELECT DISTINCT mst.ArrivalTo, mst.DepartFrom
			FROM kenuser.AttendanceTimesheet ats WITH (NOLOCK)
				INNER JOIN kenuser.MasterShiftTime mst WITH (NOLOCK) ON RTRIM(ats.ShiftPatCode) = RTRIM(mst.ShiftPatternCode) AND RTRIM(ats.SchedShiftCode) = RTRIM(mst.ShiftCode)
			WHERE ats.EmpNo = a.EmployeeNo
		) f
		OUTER APPLY	
		(
			SELECT
				-- Total deficit hours
				CAST(SUM(ISNULL(NoPayHours, 0)) AS DECIMAL) / 60 AS TotalDeficitHour,

				-- Total work hours
				CAST(SUM(ISNULL(DurationWorkedCumulative, 0)) AS DECIMAL) / 60 AS TotalWorkHour,

				-- Total days worked (distinct attendance dates)
				COUNT(DISTINCT CAST(AttendanceDate AS DATE)) AS TotalDaysWorked,

				-- Average hours worked per day
				CASE 
					WHEN COUNT(DISTINCT CAST(AttendanceDate AS DATE)) = 0 THEN 0
					ELSE 
						CAST(SUM(ISNULL(DurationWorkedCumulative, 0)) AS DECIMAL(10,2))
						/ COUNT(DISTINCT CAST(AttendanceDate AS DATE))
				END AS AverageWorkHour,

				-- Total absent days (RemarkCode = 'A')
				COUNT(DISTINCT
					CASE 
						WHEN RemarkCode = 'A' 
						THEN CAST(AttendanceDate AS DATE)
					END
				) AS TotalAbsent
			FROM kenuser.AttendanceTimesheet x WITH (NOLOCK)
			WHERE x.EmpNo = a.EmployeeNo
			  AND x.AttendanceDate BETWEEN @startDate AND @endDate
		) g
	WHERE a.EmployeeNo = @empNo
	
	
END

/*	Debug:

PARAMETERS:
	@empNo				INT,
	@startDate			DATETIME,
	@endDate			DATETIME

	EXEC kenuser.Pr_GetAttendanceSummary 10003632, '02/01/2026', '02/28/2026'
	EXEC kenuser.Pr_GetAttendanceSummary 10003632, '01/16/2026', '02/15/2026'
	EXEC kenuser.Pr_GetAttendanceSummary 10003632, '02/16/2026', '03/15/2026'

*/