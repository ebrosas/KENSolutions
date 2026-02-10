
	SELECT * FROM [kenuser].[ShiftPatternChange] a
	WHERE a.EmpNo = 10003632

	SELECT * FROM [kenuser].[AttendanceSwipeLog] a
	WHERE a.EmpNo = 10003632
	ORDER BY a.SwipeDate
	
	

	SELECT DISTINCT --COUNT(fswipe.AutoId) AS CountLate--, 
		fswipe.*
	FROM kenuser.AttendanceTimesheet ats WITH (NOLOCK)
		CROSS APPLY	
		(
			SELECT TOP 1 x.AutoId, x.EmpNo, x.AttendanceDate, x.TimeIn, x.[TimeOut] 
			FROM kenuser.AttendanceTimesheet x WITH (NOLOCK)
			WHERE x.EmpNo = ats.EmpNo
				AND x.AttendanceDate = ats.AttendanceDate
			ORDER BY x.TimeIn
		) fswipe
		INNER JOIN kenuser.MasterShiftTime mst WITH (NOLOCK) ON RTRIM(ats.SchedShiftCode) = RTRIM(mst.ShiftCode)
	WHERE ats.EmpNo = 10003632
		AND CAST(fswipe.TimeIn AS TIME) > mst.ArrivalTo


	SELECT TOP 1 a.EmpNo, a.AttendanceDate, a.TimeIn FROM [kenuser].[AttendanceTimesheet] a
	WHERE a.EmpNo = 10003632
	GROUP BY a.EmpNo, a.AttendanceDate, a.TimeIn
	ORDER BY a.TimeIn ASC

	WITH FirstInPerDay AS
	(
		SELECT
			EmpNo,
			SwipeDate,
			MIN(SwipeTime) AS FirstInTime
		FROM kenuser.AttendanceSwipeLog a WITH (NOLOCK)
		WHERE SwipeType = 'IN'
			AND a.EmpNo = 10003632
		GROUP BY EmpNo, SwipeDate
	)
	SELECT
		COUNT(*) AS LateArrivalCount
	FROM FirstInPerDay
	WHERE CAST(FirstInTime AS time) > '08:00:00';

	SELECT DISTINCT mst.*
	FROM kenuser.AttendanceTimesheet ats WITH (NOLOCK)
		INNER JOIN kenuser.MasterShiftTime mst WITH (NOLOCK) ON RTRIM(ats.ShiftPatCode) = RTRIM(mst.ShiftPatternCode) AND RTRIM(ats.SchedShiftCode) = RTRIM(mst.ShiftCode)
	WHERE ats.EmpNo = 10003632

	SELECT * FROM [kenuser].[AttendanceTimesheet] a
	WHERE a.EmpNo = 10003632
	ORDER BY a.AttendanceDate

	SELECT
		SUM(ISNULL(NoPayHours, 0)) AS TotalDeficitHours,
		SUM(ISNULL(DurationWorkedCumulative, 0)) AS TotalWorkHours,
		COUNT(DISTINCT CAST(AttendanceDate AS DATE)) AS TotalDaysWorked,
		CASE 
			WHEN COUNT(DISTINCT CAST(AttendanceDate AS DATE)) = 0 THEN 0
			ELSE 
				CAST(SUM(ISNULL(DurationWorkedCumulative, 0)) AS DECIMAL(10,2))
				/ COUNT(DISTINCT CAST(AttendanceDate AS DATE))
		END AS AverageHoursWorked
	FROM kenuser.AttendanceTimesheet
	WHERE EmpNo = 10003632
	  AND AttendanceDate BETWEEN '2026-02-01' AND '2026-02-28';

	SELECT 
		ISNULL(SUM(x.NoPayHours), 0) AS TotalNPH,
		ISNULL(SUM(x.DurationWorkedCumulative), 0) AS TotalWorkHour, 
		COUNT(x.AttendanceDate) AS TotalDaysWorked
	FROM kenuser.AttendanceTimesheet x WITH (NOLOCK)
	WHERE x.EmpNo = 10003632
		AND x.AttendanceDate BETWEEN '02/01/2026' AND '02/28/2026'

	

