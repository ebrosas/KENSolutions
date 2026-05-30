
	SELECT a.ShiftPatCode, a.SchedShiftCode, a.DurationRequired, a.DurationWorked, a.DurationWorkedCumulative, 
		a.TimeIn, a.TimeOut,
		a.* 
	FROM kenuser.AttendanceTimesheet a WITH (NOLOCK) 
	WHERE a.EmpNo = 10003632
		AND a.AttendanceDate BETWEEN '03/01/2026' AND '03/31/2026'
	ORDER By a.AttendanceDate Desc

	SELECT a.Position, * FROM kenuser.Employee a
	WHERE a.EmployeeNo = 10003632