
	--Attendance Legends
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'ATTENDLEGEND')	

	SELECT * FROM kenuser.MasterShiftPatternTitle a 
	WHERE RTRIM(a.ShiftPatternCode) = 'D8'

	SELECT * FROM kenuser.MasterShiftTime a
	WHERE a.ShiftPatternCode = 
	(
		SELECT x.ShiftPatternCode FROM kenuser.MasterShiftPatternTitle x 
		WHERE RTRIM(x.ShiftPatternCode) = 'D8'
	)

	SELECT * FROM kenuser.MasterShiftPattern a
	WHERE a.ShiftPatternCode = 
	(
		SELECT x.ShiftPatternCode FROM kenuser.MasterShiftPatternTitle x 
		WHERE RTRIM(x.ShiftPatternCode) = 'D8'
	)

	SELECT	a.AutoId, a.EmpNo, a.AttendanceDate, a.TimeIn, a.TimeOut,
			a.RemarkCode, a.LeaveType, a.AbsenceReasonCode, a.SchedShiftCode,
			a.* 
	FROM [kenuser].[AttendanceTimesheet] a
	WHERE a.EmpNo = 10003632
		AND MONTH(a.AttendanceDate) = 3
	ORDER BY a.AttendanceDate 

	SELECT	 DATEDIFF(MINUTE, CAST(a.[TimeOut] AS TIME), b.DepartFrom) AS TimeOutDiff,
			CAST(a.[TimeOut] AS TIME) AS OutTime,
			b.DepartFrom,
			a.AutoId, a.EmpNo, a.AttendanceDate, a.TimeIn, a.TimeOut, 
			a.RemarkCode, a.LeaveType, a.AbsenceReasonCode, a.SchedShiftCode,
			a.* 
	FROM [kenuser].[AttendanceTimesheet] a
		LEFT JOIN kenuser.MasterShiftTime b WITH (NOLOCK) ON RTRIM(a.ShiftPatCode) = RTRIM(b.ShiftPatternCode)
	WHERE a.EmpNo = 10003632
		AND a.AttendanceDate = '03/01/2026'

/*

	BEGIN TRAN T1

	UPDATE [kenuser].[AttendanceTimesheet]
	SET TimeIn = '2026-03-30 08:06:31.107'
	WHERE AutoId = 10192

	UPDATE [kenuser].[AttendanceTimesheet]
	SET TimeIn = '2026-03-01 07:45:31.107',
		TimeOut = '2026-03-01 15:50:31.107',
		RemarkCode = ''
	WHERE AutoId = 10130

	UPDATE [kenuser].[AttendanceTimesheet]
	SET LeaveType = 'AL',
		RemarkCode = ''
	WHERE AutoId IN (10172, 10174)

	UPDATE [kenuser].[AttendanceTimesheet]
	SET LeaveType = 'SL',
		RemarkCode = ''
	WHERE AutoId IN (10176, 10178)

	UPDATE [kenuser].[AttendanceTimesheet]
	SET LeaveType = 'IL',
		RemarkCode = ''
	WHERE AutoId IN (10180)

	UPDATE [kenuser].[AttendanceTimesheet]
	SET AbsenceReasonCode = 'BT',
		RemarkCode = ''
	WHERE AutoId IN (10132, 10134)

	UPDATE [kenuser].[AttendanceTimesheet]
	SET AbsenceReasonCode = 'WH',
		RemarkCode = ''
	WHERE AutoId IN (10136)

	COMMIT TRAN T1

*/