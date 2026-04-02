/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetLeaveEntitlement
*	Description: Get the leave entitlement information
*
*	Date			Author		Rev. #		Comments:
*	02/04/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetLeaveEntitlement
(   
	@entitlementId		INT = 0,
	@empNo				INT = 0,
	@costCenter			VARCHAR(20) = NULL,
	@startDate			DATETIME = NULL,
	@endDate			DATETIME = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@entitlementId, 0) = 0
		SET @entitlementId = NULL

	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	IF ISNULL(@costCenter, '') = ''
		SET @costCenter = NULL

	SELECT	a.LeaveEntitlementId,
			a.EmployeeNo,
			RTRIM(b.FirstName) + ' ' + RTRIM(b.MiddleName) + ' ' + RTRIM(b.LastName) AS EmployeeName,
			b.DepartmentCode,
			c.DepartmentName,
			a.EffectiveDate,
			a.ALEntitlementCount,
			a.SLEntitlementCount,
			a.ALRenewalType,
			RTRIM(d.UDCDesc1) AS ALRenewalTypeDesc,
			a.SLRenewalType,
			RTRIM(e.UDCDesc1) AS SLRenewalTypeDesc,
			a.LeaveUOM,
			RTRIM(f.UDCDesc1) AS LeaveUOMDesc,
			a.SickLeaveUOM,
			RTRIM(g.UDCDesc1) AS SickLeaveUOMDesc,
			a.LeaveBalance,
			a.SLBalance,
			a.DILBalance,
			a.CreatedDate,
			a.LeaveCreatedBy,
			a.CreatedUserID,
			a.LastUpdatedDate,
			a.LastUpdatedBy,
			a.LastUpdatedUserID
	FROM kenuser.LeaveEntitlement a WITH (NOLOCK)
		INNER JOIN kenuser.Employee b WITH (NOLOCK) ON a.EmployeeNo = b.EmployeeNo
		INNER JOIN kenuser.DepartmentMaster c WITH (NOLOCK) ON RTRIM(b.DepartmentCode) = RTRIM(c.DepartmentCode)
		LEFT JOIN kenuser.UserDefinedCode d WITH (NOLOCK) ON RTRIM(a.ALRenewalType) = RTRIM(d.UDCCode)
		LEFT JOIN kenuser.UserDefinedCode e WITH (NOLOCK) ON RTRIM(a.SLRenewalType) = RTRIM(e.UDCCode)
		LEFT JOIN kenuser.UserDefinedCode f WITH (NOLOCK) ON RTRIM(a.LeaveUOM) = RTRIM(f.UDCCode)
		LEFT JOIN kenuser.UserDefinedCode g WITH (NOLOCK) ON RTRIM(a.SickLeaveUOM) = RTRIM(g.UDCCode)
	WHERE 
		(a.LeaveEntitlementId = @entitlementId OR @entitlementId IS NULL)
		AND (a.EmployeeNo = @empNo OR @empNo IS NULL)
		AND (RTRIM(b.DepartmentCode) = @costCenter OR @costCenter IS NULL)
		AND 
		(
			(a.EffectiveDate BETWEEN @startDate AND @endDate
			AND (@startDate IS NOT NULL AND @endDate IS NOT NULL))
			OR (@startDate IS NULL AND @endDate IS NULL)
		)
	
	
END

/*	Debug:

PARAMETERS:
	@entitlementId		INT = 0,
	@empNo				INT = 0,
	@costCenter			VARCHAR(20) = NULL,
	@startDate			DATETIME = NULL,
	@endDate			DATETIME = NULL

	EXEC kenuser.Pr_GetLeaveEntitlement
	EXEC kenuser.Pr_GetLeaveEntitlement 0, 10003632
	EXEC kenuser.Pr_GetLeaveEntitlement 0, 0, '7600'
	EXEC kenuser.Pr_GetLeaveEntitlement 0, 0, '', '03/01/2026', '03/31/2026'

*/