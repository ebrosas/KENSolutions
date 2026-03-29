/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetAttendanceLegend
*	Description: Get the monthly attendance legend that will be mapped to the calendar control in the Attendance Dashboard
*
*	Date			Author		Rev. #		Comments:
*	29/03/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetAttendanceLegend
(   		
	@empNo		INT,
	@year		INT,
	@month		INT 
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	SELECT	a.EmpNo,
			a.AttendanceDate,
			CASE WHEN RTRIM(a.RemarkCode) = 'A' THEN 'ALABSENT'
				ELSE ''
			END AS LegendCode,
			CASE WHEN RTRIM(a.RemarkCode) = 'A' THEN 'ALABSENT'
				ELSE ''
			END AS LegendDesc
	FROM kenuser.AttendanceTimesheet a WITH (NOLOCK)
	WHERE a.EmpNo = @empNo
		AND YEAR(a.AttendanceDate) = @year
		AND MONTH(a.AttendanceDate) = @month
	
	
END

/*	Debug:

PARAMETERS:
	@empNo				INT,
	@attendanceDate		DATETIME

	EXEC kenuser.Pr_GetAttendanceLegend 10003632, '03/28/2026'
	EXEC kenuser.Pr_GetAttendanceLegend 10003632, '02/01/2026'
	EXEC kenuser.Pr_GetAttendanceLegend 10003632, '02/07/2026'

*/