/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GenerateAttendance
*	Description: This stored procedure is used to automate the Timesheet Process
*
*	Date			Author		Rev. #		Comments:
*	11/02/2026		Ervin		1.0			Created
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
        -- 3) Resolve Latest Shift (Supports Day-Off)
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
        )
        SELECT 
            ls.EmpNo,
            ls.ShiftPatternCode,
            msp.ShiftCode,
            ISNULL(mst.DurationNormal, 0) AS DurationNormal
        INTO #ShiftResolved
        FROM LatestShift ls
        INNER JOIN kenuser.MasterShiftPattern msp
            ON ls.ShiftPatternCode = msp.ShiftPatternCode
           AND ls.ShiftPointer = msp.ShiftPointer
        LEFT JOIN kenuser.MasterShiftTime mst
            ON msp.ShiftPatternCode = mst.ShiftPatternCode
           AND msp.ShiftCode = mst.ShiftCode;


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
           5) NORMAL SHIFT – PRESENT
        ====================================================== */
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            TimeIn, TimeOut, ShavedIn, ShavedOut,
            ShiftPatCode, SchedShiftCode, ActualShiftCode,
            DurationRequired, DurationWorked,
            DurationWorkedCumulative, NetMinutes,
            IsLastRow
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
            CASE WHEN ps.PairRow = ps.TotalPairs THEN 1 ELSE 0 END
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        INNER JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode <> 'O';


        /* =====================================================
           6) NORMAL SHIFT – ABSENT
        ====================================================== */
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            ShiftPatCode, SchedShiftCode, ActualShiftCode,
            DurationRequired,
            DurationWorked, DurationWorkedCumulative,
            NetMinutes, RemarkCode, IsLastRow
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
            1
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        LEFT JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode <> 'O'
          AND ps.EmpNo IS NULL;


        /* =====================================================
           7) DAY-OFF – NO SWIPE
        ====================================================== */
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            ShiftPatCode,
            SchedShiftCode, ActualShiftCode,
            DurationRequired,
            DurationWorked, DurationWorkedCumulative,
            NetMinutes, AbsenceReasonColumn, IsLastRow
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
            1
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        LEFT JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode = 'O'
          AND ps.EmpNo IS NULL;


        /* =====================================================
           8) DAY-OFF – WORKED
        ====================================================== */
        INSERT INTO kenuser.AttendanceTimesheet
        (
            EmpNo, CostCenter, PayGrade, AttendanceDate,
            TimeIn, TimeOut, ShavedIn, ShavedOut,
            ShiftPatCode,
            SchedShiftCode, ActualShiftCode,
            DurationRequired,
            DurationWorked, DurationWorkedCumulative,
            NetMinutes, IsLastRow
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
            CASE WHEN ps.PairRow = ps.TotalPairs THEN 1 ELSE 0 END
        FROM #ActiveEmployees ae
        INNER JOIN #ShiftResolved sr ON ae.EmpNo = sr.EmpNo
        INNER JOIN #PairedSwipe ps ON ae.EmpNo = ps.EmpNo
        LEFT JOIN kenuser.UserDefinedCode udc
            ON ae.PayGrade = udc.UDCCode
        WHERE sr.ShiftCode = 'O';


        -------------------------------------------------------
        -- Cleanup
        -------------------------------------------------------
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

	EXEC kenuser.Pr_GenerateAttendance '02/03/2026'

*/
