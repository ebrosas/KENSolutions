
	EXEC kenuser.Pr_GetAttendanceSummary 10003632, '05/22/2026', '05/22/2026'

	--Get attendance details
	EXEC kenuser.Pr_GetAttendanceDetails 10003632, '05/22/2026'

	EXEC kenuser.Pr_CalculateWorkDuration 10003632, '05/22/2026'

	--Get workflow info
	EXEC kenuser.Pr_GetWorkflowStatus 'RTYPEREGULAR', 5

	SELECT a.ShiftPatCode, a.SchedShiftCode, a.DurationRequired, a.DurationWorked, a.DurationWorkedCumulative, 
		a.TimeIn, a.TimeOut,
		a.* 
	FROM kenuser.AttendanceTimesheet a WITH (NOLOCK) 
	WHERE a.EmpNo = 10003632
		AND a.AttendanceDate = '05/22/2026'
	ORDER By a.AttendanceDate Desc

	SELECT * FROM kenuser.RegularRequestWFs a
	ORDER BY a.RegularizationId desc

	SELECT * FROM kenuser.FileAttachments a
		