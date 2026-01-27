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

	SELECT	10003632 AS EmployeeNo,
			@attendanceDate AS AttendanceDate,
			CAST('2026-01-27 08:06:05' AS DATETIME) AS FirstTimeIn,
			CAST('2026-01-27 05:15:42' AS DATETIME) AS LastTimeOut,
			'8 hrs. 45 mins.' AS WorkDurationDesc,
			'0' AS DeficitHoursDesc,
			'None' AS RegularizedType,
			'None'  AS RegularizedReason,
			'Pending for approval' AS LeaveStatus,
			'Leave No. 124556' AS LeaveDetails,
			'08:15 AM, 9:30 AM, 4:30 PM' AS RawSwipes
	
	
END

/*	Debug:

PARAMETERS:
	@empNo				INT,
	@attendanceDate		DATETIME

	EXEC kenuser.Pr_GetAttendanceDetail 10003632, '01/16/2026', '02/15/2026'

*/