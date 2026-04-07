/************************************************************************************************************************************************************************
*	Revision History
*
*	Name: tas.fnGetAssignedRequestCount
*	Description: This function is used to get the total late count of an employee on a given period
*
*	Date:			Author:		Rev.#:		Comments:
*	10/02/2026		Ervin		1.0			Created
**************************************************************************************************************************************************************************/

ALTER FUNCTION kenuser.fnGetAssignedRequestCount
(
	@requestTypeCode	VARCHAR(20),
    @empNo				INT,
    @startDate			DATETIME,
    @endDate			DATETIME
)
RETURNS INT
AS
BEGIN

    DECLARE @assignedCount INT;

	--Validate parameters
	IF ISNULL(@startDate, '') = '' OR CAST(@startDate AS DATETIME) = CAST('' AS DATETIME)
		SET @startDate = NULL

	IF ISNULL(@endDate, '') = '' OR CAST(@endDate AS DATETIME) = CAST('' AS DATETIME)
		SET @endDate = NULL

	SELECT @assignedCount = COUNT(a.ApprovalId)
	FROM kenuser.RequestApprovals a WITH (NOLOCK)
	WHERE a.AssignedEmpNo = @empNo
		AND RTRIM(a.RequestTypeCode) = @requestTypeCode
		AND 
		(
			(a.CreatedDate BETWEEN @startDate AND @endDate AND @startDate IS NOT NULL AND @endDate IS NOT NULL)
			OR (@startDate IS NULL AND @endDate IS NULL)
		)

    RETURN @assignedCount;
END

GO

/*	Test:

	SELECT  kenuser.fnGetAssignedRequestCount('RTYPELEAVE', 10003632, '', '')
	SELECT  kenuser.fnGetAssignedRequestCount(10003632, '02/01/2026', '02/28/2026', NULL)

*/