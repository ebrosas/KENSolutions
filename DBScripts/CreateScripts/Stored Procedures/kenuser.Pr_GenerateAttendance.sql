/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetDashboardStatistics
*	Description: Get the list of pending, approved, rejected, and on-hold requests based on the employee number
*
*	Date			Author		Rev. #		Comments:
*	27/02/2026		Ervin		1.0			Created
*	16/05/2026		Ervin		1.1			Set the value of "CreatedDate" field to the current system date
*
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GenerateAttendance
(
    @AttendanceDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        -------------------------------------------------------
        -- 1) Idempotent Delete
        -------------------------------------------------------
        DELETE FROM kenuser.AttendanceTimesheet
        WHERE AttendanceDate = @AttendanceDate;


        -------------------------------------------------------
        -- 2) Active Employees
        -------------------------------------------------------
        IF OBJECT_ID('tempdb..#ActiveEmployees') IS NOT NULL DROP TABLE #ActiveEmployees;

        SELECT 
            e.EmployeeNo AS EmpNo,
            e.DepartmentCode AS CostCenter,
            e.PayGrade
        INTO #ActiveEmployees
        FROM kenuser.Employee e
        WHERE e.HireDate <= @AttendanceDate
          AND e.TerminationDate IS NULL;


        -------------------------------------------------------
        -- 3) Resolve Dynamic Shift Based On Cycle Logic
        -------------------------------------------------------
        IF OBJECT_ID('tempdb..#ShiftResolved') IS NOT NULL DROP TABLE #ShiftResolved;

        ;WITH LatestShift AS
        (
            SELECT *
            FROM (
                SELECT *,
                       ROW_NUMBER() OVER (PARTITION BY EmpNo
                                          ORDER BY EffectiveDate DESC, AutoId DESC) rn
                FROM kenuser.ShiftPatternChange
                WHERE EffectiveDate <= @AttendanceDate
            ) x
            WHERE rn = 1
        ),
        PatternMax AS
        (
            SELECT 
                ShiftPatternCode,
                MAX(ShiftPointer) AS MaxPointer
            FROM kenuser.MasterShiftPattern
            GROUP BY ShiftPatternCode
        )
        SELECT
            ls.EmpNo,
            ls.ShiftPatternCode,

            -- Calculate Dynamic Pointer
            (
                ((ls.ShiftPointer - 1 +
                  DATEDIFF(DAY, ls.EffectiveDate, @AttendanceDate)
                 ) % pm.MaxPointer) + 1
            ) AS CalculatedPointer

        INTO #ShiftPointerCalc
        FROM LatestShift ls
        INNER JOIN PatternMax pm
            ON ls.ShiftPatternCode = pm.ShiftPatternCode;


        -- Now fetch actual ShiftCode
        SELECT
            spc.EmpNo,
            spc.ShiftPatternCode,
            msp.ShiftCode,
            ISNULL(mst.DurationNormal, 0) AS DurationNormal
        INTO #ShiftResolved
        FROM #ShiftPointerCalc spc
        INNER JOIN kenuser.MasterShiftPattern msp
            ON spc.ShiftPatternCode = msp.ShiftPatternCode
           AND spc.CalculatedPointer = msp.ShiftPointer
        LEFT JOIN kenuser.MasterShiftTime mst
            ON msp.ShiftPatternCode = mst.ShiftPatternCode
           AND msp.ShiftCode = mst.ShiftCode;

        DROP TABLE #ShiftPointerCalc;


        -------------------------------------------------------
        -- 4) Materialize Paired Swipes
        -------------------------------------------------------
        IF OBJECT_ID('tempdb..#PairedSwipe') IS NOT NULL DROP TABLE #PairedSwipe;

        ;WITH OrderedSwipe AS
        (
            SELECT 
                s.EmpNo,
                s.SwipeTime,
                s.SwipeType,
                LAG(s.SwipeType) OVER 
                    (PARTITION BY s.EmpNo ORDER BY s.SwipeTime) PrevType
            FROM kenuser.AttendanceSwipeLog s
            WHERE CAST(s.SwipeDate AS DATE) = @AttendanceDate
        ),
        CleanSwipe AS
        (
            SELECT *
            FROM OrderedSwipe
            WHERE SwipeType = 'IN'
              AND (PrevType IS NULL OR PrevType = 'OUT')

            UNION ALL

            SELECT *
            FROM OrderedSwipe
            WHERE SwipeType = 'OUT'
        )
        SELECT
            i.EmpNo,
            i.SwipeTime AS TimeIn,
            (
                SELECT TOP 1 o.SwipeTime
                FROM CleanSwipe o
                WHERE o.EmpNo = i.EmpNo
                  AND o.SwipeType = 'OUT'
                  AND o.SwipeTime > i.SwipeTime
                ORDER BY o.SwipeTime
            ) AS TimeOut,
            ROW_NUMBER() OVER (PARTITION BY i.EmpNo ORDER BY i.SwipeTime) PairRow,
            COUNT(*) OVER (PARTITION BY i.EmpNo) TotalPairs
        INTO #PairedSwipe
        FROM CleanSwipe i
        WHERE i.SwipeType = 'IN';


        /* =====================================================
           INSERT LOGIC (unchanged except now ShiftCode is correct)
        ====================================================== */

        -- NORMAL PRESENT
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            TimeIn, TimeOut, ShavedIn, ShavedOut,
            ShiftPatCode, SchedShiftCode, ActualShiftCode,
            DurationRequired, DurationWorked,
            DurationWorkedCumulative, NetMinutes,
            IsLastRow, 
			CreatedDate		--Rev. #1.1
        )
        SELECT
            ae.EmpNo,
            ae.CostCenter,
            udc.UDCDesc1,
            @AttendanceDate,
            ps.TimeIn,
            ps.TimeOut,
            ps.TimeIn,
            ps.TimeOut,
            sr.ShiftPatternCode,
            sr.ShiftCode,
            sr.ShiftCode,
            sr.DurationNormal,
            ISNULL(DATEDIFF(MINUTE, ps.TimeIn, ps.TimeOut), 0),
            SUM(ISNULL(DATEDIFF(MINUTE, ps.TimeIn, ps.TimeOut),0))
                OVER (PARTITION BY ae.EmpNo),
            ISNULL(DATEDIFF(MINUTE, ps.TimeIn, ps.TimeOut), 0),
            CASE WHEN ps.PairRow = ps.TotalPairs THEN 1 ELSE 0 END,
			GETDATE()		--Rev. #1.1
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        INNER JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode <> 'O';


        -- NORMAL ABSENT
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            ShiftPatCode, SchedShiftCode, ActualShiftCode,
            DurationRequired,
            DurationWorked, DurationWorkedCumulative,
            NetMinutes, RemarkCode, IsLastRow, 
			CreatedDate		--Rev. #1.1
        )
        SELECT
            ae.EmpNo,
            ae.CostCenter,
            udc.UDCDesc1,
            @AttendanceDate,
            sr.ShiftPatternCode,
            sr.ShiftCode,
            sr.ShiftCode,
            sr.DurationNormal,
            0, 0, 0,
            'A',
            1,
			GETDATE()		--Rev. #1.1
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        LEFT JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode <> 'O'
          AND ps.EmpNo IS NULL;


        -- DAY-OFF NO SWIPE
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            ShiftPatCode,
            SchedShiftCode, ActualShiftCode,
            DurationRequired,
            DurationWorked, DurationWorkedCumulative,
            NetMinutes, AbsenceReasonColumn, IsLastRow, 
			CreatedDate		--Rev. #1.1
        )
        SELECT
            ae.EmpNo,
            ae.CostCenter,
            udc.UDCDesc1,
            @AttendanceDate,
            sr.ShiftPatternCode,
            'O',
            'O',
            0,
            0, 0, 0,
            'Day-off',
            1,
			GETDATE()		--Rev. #1.1
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        LEFT JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode = 'O'
          AND ps.EmpNo IS NULL;


        -- DAY-OFF WORKED
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            TimeIn, TimeOut, ShavedIn, ShavedOut,
            ShiftPatCode,
            SchedShiftCode, ActualShiftCode,
            DurationRequired,
            DurationWorked, DurationWorkedCumulative,
            NetMinutes, IsLastRow, 
			CreatedDate		--Rev. #1.1
        )
        SELECT
            ae.EmpNo,
            ae.CostCenter,
            udc.UDCDesc1,
            @AttendanceDate,
            ps.TimeIn,
            ps.TimeOut,
            ps.TimeIn,
            ps.TimeOut,
            sr.ShiftPatternCode,
            'O',
            'O',
            0,
            ISNULL(DATEDIFF(MINUTE, ps.TimeIn, ps.TimeOut), 0),
            SUM(ISNULL(DATEDIFF(MINUTE, ps.TimeIn, ps.TimeOut),0))
                OVER (PARTITION BY ae.EmpNo),
            ISNULL(DATEDIFF(MINUTE, ps.TimeIn, ps.TimeOut), 0),
            CASE WHEN ps.PairRow = ps.TotalPairs THEN 1 ELSE 0 END,
			GETDATE()		--Rev. #1.1
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        INNER JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode = 'O';


        DROP TABLE #ActiveEmployees;
        DROP TABLE #ShiftResolved;
        DROP TABLE #PairedSwipe;

        COMMIT TRAN;

    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        THROW;
    END CATCH
END

/*	Testing:

	EXEC kenuser.Pr_GenerateAttendance '05/14/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/02/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/03/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/04/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/05/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/06/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/07/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/08/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/09/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/10/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/11/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/12/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/13/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/14/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/15/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/16/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/17/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/18/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/19/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/20/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/21/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/22/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/23/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/24/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/25/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/26/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/27/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/28/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/29/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/30/2026'
	EXEC kenuser.Pr_GenerateAttendance '03/31/2026'

*/