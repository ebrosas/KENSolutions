
	SELECT a.EmployeeId, a.EmployeeNo, a.HireDate, a.TerminationDate,
		a.DepartmentCode, a.PayGrade, a.EmploymentTypeCode, a.RoleCode, a.JobTitleCode, a.FirstAttendanceModeCode, a.SecondAttendanceModeCode, a.ThirdAttendanceModeCode,
		a.SecondReportingManagerCode, a.* 
	FROM kenuser.Employee a
	--WHERE a.EmployeeNo = 10003632
	ORDER BY a.EmployeeNo

	SELECT * FROM [kenuser].[ShiftPatternChange] a WITH (NOLOCK)
	--WHERE a.EmpNo = 10003632

	SELECT * FROM [kenuser].[AttendanceTimesheet] a
	WHERE a.AttendanceDate = '01/17/2026'
		AND a.EmpNo = 10003633
	ORDER BY a.EmpNo, a.AttendanceDate

	SELECT * FROM [kenuser].[AttendanceTimesheet] a
	WHERE a.EmpNo = 10003633
	ORDER BY a.EmpNo, a.AttendanceDate

	SELECT * FROM [kenuser].[AttendanceSwipeLog] a
	--WHERE a.SwipeDate = '02/07/2026'
	WHERE a.EmpNo = 10003633
	ORDER BY a.EmpNo, a.SwipeDate, a.SwipeTime

/*

	BEGIN TRAN T1

	DELETE FROM [kenuser].[AttendanceTimesheet] 
	WHERE AttendanceDate = '02/07/2026'

	UPDATE [kenuser].[AttendanceSwipeLog] 
	SET EmpNo = 10003633,
		SwipeTime = '2026-02-07 07:56:45.527'
	WHERE SwipeID = 21

	UPDATE [kenuser].[AttendanceSwipeLog] 
	SET EmpNo = 10003633
	WHERE SwipeID = 22

	COMMIT TRAN T1

*/