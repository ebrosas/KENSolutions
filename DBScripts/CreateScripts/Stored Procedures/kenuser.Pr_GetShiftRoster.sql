/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetShiftRoster
*	Description: Get the shift roster information
*
*	Date			Author		Rev. #		Comments:
*	14/12/2025		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetShiftRoster
(   		
	@loadType				TINYINT,
	@shiftPatternCode		VARCHAR(20) = NULL,
	@shiftCode				VARCHAR(10) = NULL,
	@activeFlag				TINYINT = 0
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 
	
	IF ISNULL(@shiftPatternCode, '') = ''
		SET @shiftPatternCode = NULL

	IF ISNULL(@shiftCode, '') = ''
		SET @shiftCode = NULL

	SELECT @activeFlag = ISNULL(@activeFlag, 2)
	
	IF @loadType = 0
	BEGIN 

		SELECT	a.ShiftPatternId,
				a.ShiftPatternCode,
				a.ShiftPatternDescription,
				a.IsActive,
				a.IsDayShift,
				a.IsFlexiTime,
				a.CreatedByEmpNo,
				a.CreatedByName,
				a.CreatedByUserID,
				a.CreatedDate,
				a.LastUpdateDate,
				a.LastUpdateEmpNo,
				a.LastUpdateUserID,
				a.LastUpdatedByName
		FROM kenuser.MasterShiftPatternTitle a WITH (NOLOCK)
		WHERE (RTRIM(a.ShiftPatternCode) = @shiftPatternCode OR @shiftPatternCode IS NULL)
			AND 
			(
				(a.IsActive = 1 AND @activeFlag = 1)		--Return only active shift rosters
				OR (a.IsActive = 0 AND @activeFlag = 0)		--Return only inactive shift rosters
				OR @activeFlag = 2
			)
		ORDER BY a.ShiftPatternCode
	END 

END

/*	Debug:

PARAMETERS:
	@loadType				TINYINT,
	@shiftPatternCode		VARCHAR(20) = NULL,
	@shiftCode				VARCHAR(10) = NULL,
	@activeFlag				TINYINT = 0

	EXEC kenuser.Pr_GetShiftRoster 0
	EXEC kenuser.Pr_GetShiftRoster 0, '', '', 1


*/