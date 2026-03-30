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
				WHEN RTRIM(a.LeaveType) = 'AL' THEN 'ALLEAVE'
				WHEN RTRIM(a.LeaveType) = 'SL' THEN 'ALSICK'
				WHEN RTRIM(a.LeaveType) = 'IL' THEN 'ALINJURY'
				WHEN RTRIM(a.AbsenceReasonCode) = 'BT' THEN 'ALBUSTRIP'
				WHEN ISNULL(a.AbsenceReasonCode, '') <> '' AND RTRIM(a.AbsenceReasonCode) <> 'BT' THEN 'ALEXCUSE'
				WHEN (a.TimeIn IS NOT NULL AND b.ArrivalTo IS NOT NULL AND DATEDIFF(MINUTE, b.ArrivalTo, CAST(a.TimeIn AS TIME)) > 5) THEN 'ALLATE'
				WHEN (a.[TimeOut] IS NOT NULL AND b.DepartFrom IS NOT NULL AND DATEDIFF(MINUTE, CAST(a.[TimeOut] AS TIME), b.DepartFrom) > 5) THEN 'ALLEFTEARLY'
				ELSE 'ALPRESENT'
			END AS LegendCode,
			CASE WHEN RTRIM(a.RemarkCode) = 'A' THEN 'Absent'
				WHEN RTRIM(a.LeaveType) = 'AL' THEN 'On-leave'
				WHEN RTRIM(a.LeaveType) = 'SL' THEN 'Sick Leave'
				WHEN RTRIM(a.LeaveType) = 'IL' THEN 'Injury Leave'
				WHEN RTRIM(a.AbsenceReasonCode) = 'BT' THEN 'Business Trip'
				WHEN ISNULL(a.AbsenceReasonCode, '') <> '' AND RTRIM(a.AbsenceReasonCode) <> 'BT' THEN 'Excused'
				WHEN (a.TimeIn IS NOT NULL AND b.ArrivalTo IS NOT NULL AND DATEDIFF(MINUTE, b.ArrivalTo, CAST(a.TimeIn AS TIME)) > 5) THEN 'Late'
				WHEN (a.[TimeOut] IS NOT NULL AND b.DepartFrom IS NOT NULL AND DATEDIFF(MINUTE, CAST(a.[TimeOut] AS TIME), b.DepartFrom) > 5) THEN 'Left Early'
				ELSE 'Present'
			END AS LegendDesc
	FROM kenuser.AttendanceTimesheet a WITH (NOLOCK)
		LEFT JOIN kenuser.MasterShiftTime b WITH (NOLOCK) ON RTRIM(a.ShiftPatCode) = RTRIM(b.ShiftPatternCode)
	WHERE a.EmpNo = @empNo
		AND YEAR(a.AttendanceDate) = @year
		AND MONTH(a.AttendanceDate) = @month
	
	
END

/*	Debug:

PARAMETERS:
	@empNo		INT,
	@year		INT,
	@month		INT 

	EXEC kenuser.Pr_GetAttendanceLegend 10003632, 2026, 3

*/