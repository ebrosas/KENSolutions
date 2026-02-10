/************************************************************************************************************************************************************************
*	Revision History
*
*	Name: tas.fnGetLateCount
*	Description: This function is used to get the total late count of an employee on a given period
*
*	Date:			Author:		Rev.#:		Comments:
*	10/02/2026		Ervin		1.0			Created
**************************************************************************************************************************************************************************/

ALTER FUNCTION kenuser.fnGetLateCount
(
    @empNo			INT,
    @startDate		DATETIME,
    @endDate		DATETIME,
	@arrivalTime	TIME
)
RETURNS INT
AS
BEGIN

    DECLARE @LateCount INT;

    WITH FirstSwipePerDay AS
    (
        SELECT
            a.SwipeDate,
            MIN(a.SwipeTime) AS FirstSwipeTime
        FROM kenuser.AttendanceSwipeLog a WITH (NOLOCK)
        WHERE a.EmpNo = @empNo
          AND a.SwipeDate BETWEEN @startDate AND @endDate
          AND UPPER(RTRIM(a.SwipeType)) = 'IN'
        GROUP BY a.SwipeDate
    )
    SELECT
        @LateCount = COUNT(*)
    FROM FirstSwipePerDay
    WHERE CAST(FirstSwipeTime AS TIME) > @arrivalTime;

    RETURN @LateCount;
END

GO

/*	Test:

	SELECT  kenuser.fnGetLateCount(10003632, '02/01/2026', '02/28/2026', '08:00:00')
	SELECT  kenuser.fnGetLateCount(10003632, '02/01/2026', '02/28/2026', NULL)

*/