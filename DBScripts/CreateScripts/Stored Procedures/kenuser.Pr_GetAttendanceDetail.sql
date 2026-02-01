/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetAttendanceDetail
*	Description: Get the attendance details of the specified employee
*
*	Date			Author		Rev. #		Comments:
*	27/01/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetAttendanceDetail
(   		
	@empNo				INT,
	@attendanceDate		DATETIME
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	SELECT	DISTINCT 
			a.EmpNo AS EmployeeNo, a.SwipeDate AS AttendanceDate,
			b.FirstTimeIn, c.LastTimeOut,
			kenuser.fnGetWorkDuration(a.EmpNo, a.SwipeDate) AS WorkDurationDesc,
			'0' AS DeficitHoursDesc,
			'None' AS RegularizedType,
			'None'  AS RegularizedReason,
			'' AS LeaveStatus,
			'' AS LeaveDetails,
			d.RawSwipes
	FROM kenuser.AttendanceSwipeLog a WITH (NOLOCK)
		OUTER APPLY 
		(
			SELECT TOP 1 x.SwipeTime AS FirstTimeIn 
			FROM kenuser.AttendanceSwipeLog x WITH (NOLOCK)
			WHERE x.EmpNo = a.EmpNo 
				AND x.SwipeDate = a.SwipeDate
				AND RTRIM(x.SwipeType) = 'IN'
			ORDER BY x.SwipeID
		) b
		OUTER APPLY 
		(
			SELECT TOP 1 x.SwipeTime AS LastTimeOut 
			FROM kenuser.AttendanceSwipeLog x WITH (NOLOCK)
			WHERE x.EmpNo = a.EmpNo 
				AND x.SwipeDate = a.SwipeDate
				AND RTRIM(x.SwipeType) = 'OUT'
			ORDER BY x.SwipeID DESC
		) c
		OUTER APPLY	
		(
			SELECT
				STRING_AGG(
					FORMAT(SwipeTime, 'hh:mm:ss tt'),
					', '
				) WITHIN GROUP (ORDER BY SwipeTime) AS RawSwipes
			FROM kenuser.AttendanceSwipeLog x WITH (NOLOCK)
			WHERE x.EmpNo = a.EmpNo
			  AND x.SwipeDate = a.SwipeDate
		) d
	WHERE a.EmpNo = @empNo
		AND a.SwipeDate = @attendanceDate
	
	
END

/*	Debug:

PARAMETERS:
	@empNo				INT,
	@attendanceDate		DATETIME

	EXEC kenuser.Pr_GetAttendanceDetail 10003632, '02/01/2026'

*/