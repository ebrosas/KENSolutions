/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_SearchAttendanceCorrection
*	Description: Search for the regularization, extra time, and outdoor requisitions
*
*	Date			Author		Rev. #		Comments:
*	22/03/2026		Ervin		1.0			Created
*
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_SearchAttendanceCorrection
(   	
	@requestNo			BIGINT = NULL,
	@empNo				INT = NULL,
	@costCenter			VARCHAR(20) = NULL,	
	@requestType		VARCHAR(20) = NULL,	
	@status				VARCHAR(20) = NULL,
	@startDate			DATETIME = NULL,
	@endDate			DATETIME = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	--Validate parameters
	IF ISNULL(@requestNo, 0) = 0
		SET @requestNo = NULL

	IF ISNULL(@requestType, 0) = ''
		SET @requestType = NULL

	IF ISNULL(@costCenter, 0) = ''
		SET @costCenter = NULL

	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	IF ISNULL(@status, 0) = ''
		SET @status = NULL

	IF	@startDate IS NOT NULL AND CAST(@startDate AS DATETIME) = CAST('' AS DATETIME)
		SET @startDate = NULL

	IF	@endDate IS NOT NULL AND CAST(@endDate AS DATETIME) = CAST('' AS DATETIME)
		SET @endDate = NULL

	SELECT	DISTINCT
			a.RequestTypeCode,
			a.RequestTypeDesc,
			a.RequestNo,
			a.RequestDate,
			a.OrigEmpNo,
			a.OrigEmpName,
			a.CostCenter,
			a.CostCenterName,
			a.AppliedDate,
			a.RequestedByNo,
			a.RequestedByName,
			a.RequestDetail,
			a.StatusHandlingCode AS CurrentStatus
	FROM kenuser.Vw_RequestDetail a
		CROSS APPLY
		(
			SELECT x.* 
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
				AND RTRIM(x.UDCSpecialHandlingCode) = 'ATTENDANCE'
				AND RTRIM(x.UDCCode) = RTRIM(a.RequestTypeCode)
		) b
	WHERE 
		(a.RequestNo = @requestNo OR @requestNo IS NULL)
		AND (a.OrigEmpNo = @empNo OR @empNo IS NULL) 
		AND (RTRIM(a.CostCenter) = @costCenter OR @costCenter IS NULL)
		AND (RTRIM(a.StatusHandlingCode) = @status OR @status IS NULL)
		AND 
		(
			a.RequestDate BETWEEN @startDate AND @endDate
			OR (@startDate IS NULL AND @endDate IS NULL) 
		)
		AND (RTRIM(a.RequestTypeCode) = @requestType OR @requestType IS NULL) 
	ORDER BY a.RequestTypeCode, a.RequestNo DESC
	
END

/*	Debug:

PARAMETERS:
	@requestNo		BIGINT = NULL,
	@empNo			INT = NULL,
	@costCenter		VARCHAR(20) = NULL,	
	@requestType		VARCHAR(20) = NULL,	
	@status			VARCHAR(20) = NULL,
	@startDate		DATETIME = NULL,
	@endDate		DATETIME = NULL

	EXEC kenuser.Pr_SearchAttendanceCorrection
	EXEC kenuser.Pr_SearchAttendanceCorrection 2
	EXEC kenuser.Pr_SearchAttendanceCorrection 0, 10003632 
	EXEC kenuser.Pr_SearchAttendanceCorrection 0, 0, '7600'
	EXEC kenuser.Pr_SearchAttendanceCorrection 0, 0, '', 'RTYPEOT'
	EXEC kenuser.Pr_SearchAttendanceCorrection 0, 0, '', '', 'Open'
	EXEC kenuser.Pr_SearchAttendanceCorrection 0, 10003632, '', '', '', '05/01/2026', '05/31/2026'

*/

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.RegularRequestWFs 
	SET StatusHandlingCode = 'Cancelled'
	WHERE LeaveRequestId = 9

	UPDATE kenuser.RegularRequestWFs 
	SET LeaveApprovalFlag = 'W'
	WHERE LeaveRequestId = 7

	COMMIT TRAN T1

*/