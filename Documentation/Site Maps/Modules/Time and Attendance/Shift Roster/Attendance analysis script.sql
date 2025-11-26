
	--Shift Roster
	SELECT * FROM tas.Master_ShiftPatternTitles a
	WHERE RTRIM(a.ShiftPatCode) = 'AB'

	--Shift Timing Schedule
	SELECT * FROM tas.Master_ShiftTimes a
	WHERE RTRIM(a.ShiftPatCode) = 'AB'

	--Shift Pointer
	SELECT * FROM tas.Master_ShiftPattern a
	WHERE RTRIM(a.ShiftPatCode) = 'AB'

	SELECT CAST(GETDATE() AS TIME) AS TestTime
	SELECT CAST(GETDATE() AS TIMESTAMP) AS TestTime

	--Get all shift timings
	SELECT * FROM tas.syJDE_F0005 a
	WHERE LTRIM(RTRIM(a.DRSY)) = '06' 
		AND UPPER(LTRIM(RTRIM(a.DRRT))) = 'SH'