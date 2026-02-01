/************************************************************************************************************************************************************************
*	Revision History
*
*	Name: tas.fnOTRequestCreator
*	Description: Get the email address of the overtime creator
*
*	Date:			Author:		Rev.#:		Comments:
*	31/12/2017		Ervin		1.0			Created
**************************************************************************************************************************************************************************/

CREATE FUNCTION kenuser.fnGetWorkDuration
(
	@empNo	INT,	
	@attendanceDate		DATETIME
)
RETURNS VARCHAR(50)
AS
BEGIN

	DECLARE @TotalMinutes INT;

    WITH OrderedSwipes AS
    (
        SELECT
            SwipeTime,
            SwipeType,
            LEAD(SwipeTime) OVER (ORDER BY SwipeTime) AS NextSwipeTime,
            LEAD(SwipeType) OVER (ORDER BY SwipeTime) AS NextSwipeType
        FROM AttendanceSwipeLog
        WHERE EmpNo = @empNo
          AND SwipeDate = @attendanceDate
    )
    SELECT
        @TotalMinutes = ISNULL(SUM(DATEDIFF(MINUTE, SwipeTime, NextSwipeTime)), 0)
    FROM OrderedSwipes
    WHERE SwipeType = 'IN'
      AND NextSwipeType = 'OUT';

    RETURN CONCAT(
        @TotalMinutes / 60,
        ' hrs. ',
        @TotalMinutes % 60,
        ' mins.'
    );

END 

/*	Debug:

	SELECT kenuser.fnGetWorkDuration(10003632, '02/01/2026')

*/