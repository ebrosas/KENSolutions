/************************************************************************************************************************************************************************
*	Revision History
*
*	Name: tas.fnGetEarlyOutCount
*	Description: This function is used to get the total late count of an employee on a given period
*
*	Date:			Author:		Rev.#:		Comments:
*	10/02/2026		Ervin		1.0			Created
**************************************************************************************************************************************************************************/

ALTER FUNCTION kenuser.fnGetEarlyOutCount
(
    @empNo			INT,
    @startDate		DATETIME,
    @endDate		DATETIME,
	@departureTime	TIME
)
RETURNS INT
AS
BEGIN

    DECLARE @LateCount INT;

    WITH LastSwipePerDay AS
    (
        SELECT
            a.SwipeDate,
            MIN(a.SwipeTime) AS LastSwipeTime
        FROM kenuser.AttendanceSwipeLog a WITH (NOLOCK)
        WHERE a.EmpNo = @empNo
          AND a.SwipeDate BETWEEN @startDate AND @endDate
          AND UPPER(RTRIM(a.SwipeType)) = 'OUT'
        GROUP BY a.SwipeDate
    )
    SELECT
        @LateCount = COUNT(*)
    FROM LastSwipePerDay
    WHERE CAST(LastSwipeTime AS TIME) > @departureTime;

    RETURN @LateCount;
END

GO

/*	Test:

	SELECT  kenuser.fnGetEarlyOutCount(10003632, '02/01/2026', '02/28/2026', '16:30:00')
	SELECT  kenuser.fnGetEarlyOutCount(10003632, '02/01/2026', '02/28/2026', NULL)

*/