/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetPendingRequest
*	Description: Get the number of assigned requests
*
*	Date			Author		Rev. #		Comments:
*	07/04/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetPendingRequest
(   
	@empNo				INT = 0,
	@requestType		VARCHAR(20) = NULL,
	@periodType			TINYINT = NULL,
	@startDate			DATETIME = NULL,
	@endDate			DATETIME = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	IF ISNULL(@requestType, '') = ''
		SET @requestType = NULL

	IF ISNULL(@periodType, 0) = 0			--(Notes: 0 = Current Year, 1 = Last Year, 2 = Custom Period)
		SET @periodType = 0

	SELECT	RTRIM(a.UDCCode) AS RequestTypeCode,
			RTRIM(a.UDCDesc1) AS RequestTypeName,
			RTRIM(a.UDCDesc2) AS RequestTypeDesc,
			CASE WHEN RTRIM(a.UDCCode) = 'RTYPELEAVE' THEN 'fas fa-calendar-alt'			--Leave Requisition
				WHEN RTRIM(a.UDCCode) = 'RTYPETRAVEL' THEN 'fas fa-plane-departure'			--Travel Request
				WHEN RTRIM(a.UDCCode) = 'RTYPEEXPENSE' THEN 'fas fa-hand-holding-usd'		--Expense Claim
				WHEN RTRIM(a.UDCCode) = 'RTYPEOT' THEN 'fas fa-user-clock'					--Overtime
				WHEN RTRIM(a.UDCCode) = 'RTYPEREGULAR' THEN 'far fa-calendar-check'			--Regularization
				WHEN RTRIM(a.UDCCode) = 'RTYPERECRUIT' THEN 'fas fa-users'					--Recruitment Offer
				ELSE ''
			END AS IconName,
			kenuser.fnGetAssignedRequestCount(RTRIM(a.UDCCode), @empNo, @startDate, @endDate) AS AssignedCount
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'REQTYPE')

	
END

/*	Debug:

PARAMETERS:
	@empNo				INT = 0,
	@requestType		VARCHAR(20) = NULL,
	@periodType			TINYINT = NULL,
	@startDate			DATETIME = NULL,
	@endDate			DATETIME = NULL

	EXEC kenuser.Pr_GetPendingRequest
	EXEC kenuser.Pr_GetPendingRequest 0, 10003632
	EXEC kenuser.Pr_GetPendingRequest 0, 0, '7600'
	EXEC kenuser.Pr_GetPendingRequest 0, 0, '', '03/01/2026', '03/31/2026'

*/