
	SELECT a.EmployeeNo, a.CostCenter, a.TS_AutoId, a.AttendanceDate, a.OTReasonCode, * 
	FROM kenuser.[OTRequestWF] a

	EXEC kenuser.Pr_GetOvertimeDetail 3

	--Get attendance records
	SELECT 
		a.EmpNo, a.AttendanceDate,
		a.ShiftPatCode, a.SchedShiftCode, a.DurationRequired, 
		a.DurationWorked, a.DurationWorkedCumulative, 
		a.TimeIn, a.TimeOut,
		a.* 
	FROM kenuser.AttendanceTimesheet a WITH (NOLOCK) 
	WHERE a.EmpNo = 10003632
		AND (a.TimeIn is not null and a.[TimeOut] IS NOT NULL)
		--AND a.AttendanceDate = '05/22/2026'
	ORDER By a.AttendanceDate Desc

	SELECT * FROM [kenuser].[PayrollPeriod] a

/*	Debug:

	BEGIN TRAN T1
	
	UPDATE [kenuser].[PayrollPeriod]
	SET IsActive = 1
	WHERE PayrollPeriodId = 18

	UPDATE [kenuser].[PayrollPeriod]
	SET IsActive = 0
	WHERE PayrollPeriodId = 16

	COMMIT TRAN T1
*/