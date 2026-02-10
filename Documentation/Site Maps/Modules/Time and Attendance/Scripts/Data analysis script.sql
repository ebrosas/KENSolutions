
	SELECT a.HalfDayLeave, a.HalfDayLeaveFlag, 
		CASE
			WHEN a.HalfDayLeaveFlag = 0 THEN 'No Half Day'
			WHEN a.HalfDayLeaveFlag = 1 THEN 'Half Day on Leave Start Date'
			WHEN a.HalfDayLeaveFlag = 2 THEN 'Half Day on Leave Resume Date'
			WHEN a.HalfDayLeaveFlag = 3 THEN 'Half Day on both Leave Start Date and Leave Resume date '
			ELSE ''
		END AS HalfDayLeaveDesc,
		a.LeaveType,
		a.LeaveStartDate, a.LeaveEndDate, a.LeaveResumeDate,
		a.Remarks, a.LeaveBalance, a.LeaveDuration, a.NoOfHolidays, a.NoOfWeekends,
		a.PlannedLeave, a.LeavePlannedNo,
		a.* 
	FROM secuser.LeaveRequisition2 a
	WHERE a.EmpNo = 10003632
		AND RTRIM(a.ApprovalFlag) NOT IN ('C', 'D', 'R')
	ORDER BY a.RequisitionNo DESC

	