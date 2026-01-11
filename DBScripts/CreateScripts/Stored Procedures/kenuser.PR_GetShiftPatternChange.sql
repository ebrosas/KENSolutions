/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.PR_GetShiftPatternChange
*	Description: Get the shift pattern change history log information
*
*	Date			Author		Rev. #		Comments:
*	11/01/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.PR_GetShiftPatternChange
(   		
	@loadType				TINYINT,
	@autoID					INT = 0,
	@empNo					INT = 0,
	@changeType				VARCHAR(20) = '',
	@shiftPatternCode		VARCHAR(20) = NULL,
	@startDate				DATETIME = NULL,
	@endDate				DATETIME = NULL	
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 
	
	IF ISNULL(@autoID, 0) = 0
		SET @autoID = NULL

	IF ISNULL(@empNo, 0) = 0
		SET @empNo = NULL

	IF ISNULL(@changeType, '') = ''
		SET @changeType = NULL

	IF ISNULL(@shiftPatternCode, '') = ''
		SET @shiftPatternCode = NULL

	IF @loadType = 0		--Get all shift pattern changes
	BEGIN 

		SELECT	a.AutoId,
				a.EmpNo,
				RTRIM(b.FirstName) + ' ' + RTRIM(b.MiddleName) + ' ' + RTRIM(b.LastName) AS EmpName,
				b.DepartmentCode,
				c.DepartmentName,
				a.ShiftPatternCode,
				a.ShiftPointer,
				a.ChangeType AS ChangeTypeCode,
				d.UDCDesc1 AS ChangeTypeDesc,
				a.EffectiveDate,
				a.EndingDate,
				a.CreatedByEmpNo,
				a.CreatedByName,
				a.CreatedByUserID,
				a.CreatedDate,
				a.LastUpdateDate,
				a.LastUpdateEmpNo,
				a.LastUpdateUserID,
				a.LastUpdatedByName
		FROM kenuser.ShiftPatternChange a WITH (NOLOCK)
			INNER JOIN kenuser.Employee b WITH (NOLOCK) ON a.EmpNo = b.EmployeeNo
			INNER JOIN kenuser.DepartmentMaster c WITH (NOLOCK) ON RTRIM(b.DepartmentCode) = RTRIM(c.DepartmentCode)
			INNER JOIN kenuser.UserDefinedCode d WITH (NOLOCK) ON RTRIM(a.ChangeType) = RTRIM(d.UDCCode)
		WHERE (a.AutoID = @autoID OR @autoID IS NULL)
			AND (a.EmpNo = @empNo OR @empNo IS NULL)
			AND (RTRIM(a.ShiftPatternCode) = @shiftPatternCode OR @shiftPatternCode IS NULL)
			AND (RTRIM(a.ChangeType) = @changeType OR @changeType IS NULL)
			AND 
			(
				(a.EffectiveDate BETWEEN @startDate AND @endDate AND @startDate IS NOT NULL AND @endDate IS NOT NULL)
				OR (@startDate IS NULL AND @endDate IS NULL)
			)
		ORDER BY a.EffectiveDate DESC
	END 

	ELSE IF @loadType = 1		--Get specific shift pattern change record
	BEGIN 

		SELECT	a.AutoId,
				a.EmpNo,
				RTRIM(b.FirstName) + ' ' + RTRIM(b.MiddleName) + ' ' + RTRIM(b.LastName) AS EmpName,
				b.DepartmentCode,
				c.DepartmentName,
				a.ShiftPatternCode,
				a.ShiftPointer,
				a.ChangeType AS ChangeTypeCode,
				d.UDCDesc1 AS ChangeTypeDesc,
				a.EffectiveDate,
				a.EndingDate,
				a.CreatedByEmpNo,
				a.CreatedByName,
				a.CreatedByUserID,
				a.CreatedDate,
				a.LastUpdateDate,
				a.LastUpdateEmpNo,
				a.LastUpdateUserID,
				a.LastUpdatedByName
		FROM kenuser.ShiftPatternChange a WITH (NOLOCK)
			INNER JOIN kenuser.Employee b WITH (NOLOCK) ON a.EmpNo = b.EmployeeNo
			INNER JOIN kenuser.DepartmentMaster c WITH (NOLOCK) ON RTRIM(b.DepartmentCode) = RTRIM(c.DepartmentCode)
			INNER JOIN kenuser.UserDefinedCode d WITH (NOLOCK) ON RTRIM(a.ChangeType) = RTRIM(d.UDCCode)
		WHERE a.AutoID = @autoID 
	END 

END

/*	Debug:

PARAMETERS:
	@loadType				TINYINT,
	@autoID					INT = 0,
	@empNo					INT = 0,
	@changeType				VARCHAR(20) = '',
	@shiftPatternCode		VARCHAR(20) = NULL,
	@startDate				DATETIME = NULL,
	@endDate				DATETIME = NULL	

	EXEC kenuser.PR_GetShiftPatternChange 0
	EXEC kenuser.PR_GetShiftPatternChange 1, 1


*/