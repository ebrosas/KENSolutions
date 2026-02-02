/*****************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_CalculateWorkDuration
*	Description: This procedure is used to calculate the employee's total work duration on a given day
*
*	Date			Author		Rev. #		Comments:
*	31/01/2026		Ervin		1.0			Created
*	
*****************************************************************************************************************************************/

CREATE PROCEDURE kenuser.Pr_CalculateWorkDuration
(   		
	@empNo				INT,
	@attendanceDate		DATETIME
)
AS
BEGIN

    WITH OrderedSwipes AS
    (
        SELECT
            EmpNo,
            SwipeDate,
            SwipeTime,
            SwipeType,
            LEAD(SwipeTime) OVER (PARTITION BY EmpNo, SwipeDate ORDER BY SwipeTime) AS NextSwipeTime,
            LEAD(SwipeType) OVER (PARTITION BY EmpNo, SwipeDate ORDER BY SwipeTime) AS NextSwipeType
        FROM kenuser.AttendanceSwipeLog a WITH (NOLOCK)
        WHERE a.EmpNo = @empNo
          AND a.SwipeDate = @attendanceDate
    )
    SELECT
        EmpNo,
        SwipeDate,
        CONVERT(varchar(8),
            DATEADD(SECOND,
                SUM(DATEDIFF(SECOND, SwipeTime, NextSwipeTime)),
                0),
            108) AS TotalWorkDuration
    FROM OrderedSwipes
    WHERE SwipeType = 'IN'
      AND NextSwipeType = 'OUT'
    GROUP BY EmpNo, SwipeDate;

END 

/*  Debug:

    EXEC kenuser.Pr_CalculateWorkDuration 10003632, '02/01/2026'

*/
