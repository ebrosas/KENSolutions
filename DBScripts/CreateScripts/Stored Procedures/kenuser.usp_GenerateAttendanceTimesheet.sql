ALTER PROCEDURE kenuser.usp_GenerateAttendanceTimesheet
(
    @AttendanceDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #EmployeeShift (
        EmpNo INT PRIMARY KEY,
        ShiftPatternCode VARCHAR(20),
        ShiftPointer INT
    );

    ---------------------------------------------------------
    -- 1. IDEMPOTENCY: Remove existing records for the date
    ---------------------------------------------------------
    DELETE FROM kenuser.AttendanceTimesheet
    WHERE AttendanceDate = @AttendanceDate;

    ---------------------------------------------------------
    -- 2. Latest Shift per Employee
    ---------------------------------------------------------
    ;WITH LatestShift AS (
        SELECT *,
               ROW_NUMBER() OVER (
                   PARTITION BY EmpNo
                   ORDER BY EffectiveDate DESC, AutoId DESC
               ) AS rn
        FROM kenuser.ShiftPatternChange
        WHERE EffectiveDate <= @AttendanceDate
    )
    INSERT INTO #EmployeeShift (EmpNo, ShiftPatternCode, ShiftPointer)
    SELECT
        EmpNo,
        ShiftPatternCode,
        ShiftPointer
    FROM LatestShift
    WHERE rn = 1;

    ---------------------------------------------------------
    -- 3. Swipe Pairing (IN → OUT)
    ---------------------------------------------------------
    ;WITH SwipePairs AS (
        SELECT
            EmpNo,
            CAST(SwipeDate AS DATE) AS AttendanceDate,
            SwipeTime,
            SwipeType,
            LEAD(SwipeTime) OVER (
                PARTITION BY EmpNo, CAST(SwipeDate AS DATE)
                ORDER BY SwipeTime
            ) AS NextSwipeTime,
            LEAD(SwipeType) OVER (
                PARTITION BY EmpNo, CAST(SwipeDate AS DATE)
                ORDER BY SwipeTime
            ) AS NextSwipeType,
            ROW_NUMBER() OVER (
                PARTITION BY EmpNo, CAST(SwipeDate AS DATE)
                ORDER BY SwipeTime
            ) AS rn
        FROM kenuser.AttendanceSwipeLog
        WHERE CAST(SwipeDate AS DATE) = @AttendanceDate
    ),

    ValidPairs AS (
        SELECT
            EmpNo,
            AttendanceDate,
            SwipeTime AS TimeIn,
            CASE
                WHEN NextSwipeType = 'OUT' THEN NextSwipeTime
                ELSE NULL
            END AS TimeOut,
            rn
        FROM SwipePairs
        WHERE SwipeType = 'IN'
    )

    ---------------------------------------------------------
    -- 4. INSERT Attendance Records (Present Employees)
    ---------------------------------------------------------
    INSERT INTO kenuser.AttendanceTimesheet (
        EmpNo,
        CostCenter,
        PayGrade,
        AttendanceDate,
        TimeIn,
        TimeOut,
        ShavedIn,
        ShavedOut,
        ShiftPatCode,
        SchedShiftCode,
        ActualShiftCode,
        DurationWorked,
        NetMinutes,
        DurationWorkedCumulative,
        IsLastRow
    )
    SELECT
        e.EmployeeNo,
        e.DepartmentCode,
        e.PayGrade,
        @AttendanceDate,

        v.TimeIn,
        v.TimeOut,
        v.TimeIn,
        v.TimeOut,

        es.ShiftPatternCode,
        m.ShiftCode,
        m.ShiftCode,

        -- DurationWorked (night-safe)
        CASE
            WHEN v.TimeOut IS NOT NULL
            THEN DATEDIFF(
                MINUTE,
                v.TimeIn,
                CASE
                    WHEN v.TimeOut < v.TimeIn
                    THEN DATEADD(DAY, 1, v.TimeOut)
                    ELSE v.TimeOut
                END
            )
        END,

        -- NetMinutes
        CASE
            WHEN v.TimeOut IS NOT NULL
            THEN DATEDIFF(
                MINUTE,
                v.TimeIn,
                CASE
                    WHEN v.TimeOut < v.TimeIn
                    THEN DATEADD(DAY, 1, v.TimeOut)
                    ELSE v.TimeOut
                END
            )
        END,

        -- DurationWorkedCumulative
        SUM(
            CASE
                WHEN v.TimeOut IS NOT NULL
                THEN DATEDIFF(
                    MINUTE,
                    v.TimeIn,
                    CASE
                        WHEN v.TimeOut < v.TimeIn
                        THEN DATEADD(DAY, 1, v.TimeOut)
                        ELSE v.TimeOut
                    END
                )
                ELSE 0
            END
        ) OVER (PARTITION BY v.EmpNo, v.AttendanceDate),

        -- IsLastRow
        CASE
            WHEN v.rn = MAX(v.rn) OVER (PARTITION BY v.EmpNo, v.AttendanceDate)
            THEN 1 ELSE 0
        END

    FROM kenuser.Employee e
    LEFT JOIN #EmployeeShift es ON es.EmpNo = e.EmployeeNo
    LEFT JOIN kenuser.MasterShiftPattern m
        ON m.ShiftPatternCode = es.ShiftPatternCode
       AND m.ShiftPointer = es.ShiftPointer
    INNER JOIN ValidPairs v
        ON v.EmpNo = e.EmployeeNo
       AND v.AttendanceDate = @AttendanceDate
    WHERE
        e.HireDate <= @AttendanceDate
        AND e.TerminationDate IS NULL;

    ---------------------------------------------------------
    -- 5. INSERT ABSENT Employees
    ---------------------------------------------------------
    INSERT INTO kenuser.AttendanceTimesheet (
        EmpNo,
        CostCenter,
        PayGrade,
        AttendanceDate,
        ShiftPatCode,
        SchedShiftCode,
        ActualShiftCode,
        RemarkCode,
        IsLastRow
    )
    SELECT
        e.EmployeeNo,
        e.DepartmentCode,
        e.PayGrade,
        @AttendanceDate,
        es.ShiftPatternCode,
        m.ShiftCode,
        m.ShiftCode,
        'A',
        1
    FROM kenuser.Employee e
    LEFT JOIN #EmployeeShift es
        ON es.EmpNo = e.EmployeeNo
    LEFT JOIN kenuser.MasterShiftPattern m
        ON m.ShiftPatternCode = es.ShiftPatternCode
       AND m.ShiftPointer = es.ShiftPointer
    WHERE
        e.HireDate <= @AttendanceDate
        AND e.TerminationDate IS NULL
        AND NOT EXISTS (
            SELECT 1
            FROM kenuser.AttendanceSwipeLog s
            WHERE s.EmpNo = e.EmployeeNo
              AND CAST(s.SwipeDate AS DATE) = @AttendanceDate
        );

    DROP TABLE #EmployeeShift;
END
GO

/*  TESTING:

    EXEC kenuser.usp_GenerateAttendanceTimesheet '02/07/2026'

*/