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

	SELECT	10003632 AS EmployeeNo,
			'ERVIN OLINAS BROSAS' AS EmployeeName,
			'D5' AS ShiftRoster,
			'08:00 AM - 04:30 PM' AS ShiftTiming,
			FORMAT(GETDATE(), 'dd-MMM-yyyy hh:mm tt')  AS ActualTiming,
			5 AS TotalAbsent,
			3 AS TotalHalfDay,
			10 AS TotalLeave,
			6 AS TotalLate,
			2 AS TotalEarlyOut,
			3.5 AS TotalDeficitHour,
			150.75 AS TotalWorkHour,
			26.0 AS TotalDaysWorked,
			8.0 AS AverageWorkHour,
			56.0 AS TotalLeaveBalance,
			30.0 AS TotalSLBalance,
			4.0 AS TotalDILBalance
	
	
END

/*	Debug:

PARAMETERS:
	@empNo				INT,
	@startDate			DATETIME,
	@endDate			DATETIME

	EXEC kenuser.Pr_GetAttendanceSummary 10003632, '01/16/2026', '02/15/2026'

*/