/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_GetPayrollPeriod
*	Description: This stored procedure is used to fetch the payroll periods for specific year
*
*	Date			Author		Rev. #		Comments:
*	13/02/2026		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_GetPayrollPeriod
(   		
	@fiscalYear	INT = 0
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	IF ISNULL(@fiscalYear, 0) = 0
		SET @fiscalYear = NULL 

	SELECT	pp.PayrollPeriodId,
			pp.FiscalYear,
			pp.FiscalMonth,
			pp.PayrollStartDate,
			pp.PayrollEndDate,
			pp.IsActive
	FROM kenuser.PayrollPeriod pp WITH (NOLOCK)
	WHERE (pp.FiscalYear = @fiscalYear OR @fiscalYear IS NULL)
	ORDER BY pp.FiscalYear DESC, pp.FiscalMonth
	
	
END

/*	Debug:

PARAMETERS:
	@fiscalYear	INT

	EXEC kenuser.Pr_GetPayrollPeriod
	EXEC kenuser.Pr_GetPayrollPeriod 2026

*/