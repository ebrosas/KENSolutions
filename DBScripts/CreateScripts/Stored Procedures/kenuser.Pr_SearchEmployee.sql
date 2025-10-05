/*****************************************************************************************************************************************************************************
*	Revision History
*
*	Name: kenuser.Pr_SearchEmployee
*	Description: Get the data for the Weekly Overtime Report
*
*	Date			Author		Rev. #		Comments:
*	25/08/2025		Ervin		1.0			Created
*	
******************************************************************************************************************************************************************************/

ALTER PROCEDURE kenuser.Pr_SearchEmployee
(   		
	@empNo				INT = NULL,
	@firstName			VARCHAR(50) = NULL,
	@lastName			VARCHAR(100) = NULL,
	@managerEmpNo		INT = NULL,
	@joiningDate		DATETIME = NULL,
	@statusCode			VARCHAR(20) = NULL,
	@employmentType		VARCHAR(20) = NULL,
	@departmentCode		VARCHAR(20) = NULL
)
AS
BEGIN

	--Tell SQL Engine not to return the row-count information
	SET NOCOUNT ON 

	IF ISNULL(@empNo, 0) = 0 
		SET @empNo = NULL 

	IF ISNULL(@firstName, '') = ''
		SET @firstName = NULL

	IF ISNULL(@lastName, '') = ''
		SET @lastName = NULL

	IF ISNULL(@managerEmpNo, 0) = 0 
		SET @managerEmpNo = NULL 

	IF ISNULL(@joiningDate, '') = '' OR CAST(@joiningDate AS DATETIME) = CAST(NULL AS datetime)
		SET @joiningDate = NULL 

	IF ISNULL(@statusCode, '') = ''
		SET @statusCode = NULL

	IF ISNULL(@employmentType, '') = ''
		SET @employmentType = NULL

	IF ISNULL(@departmentCode, '') = ''
		SET @departmentCode = NULL
	
	SELECT	a.EmployeeId,
			a.EmployeeNo,
			a.FirstName,
			a.MiddleName,
			a.LastName,
			b.UDCDesc1 AS Gender,
			a.HireDate,
			a.EmploymentTypeCode,
			empType.UDCDesc1 AS EmploymentType,
			a.ReportingManagerCode,
			RTRIM(c.FirstName) + ' ' + RTRIM(c.MiddleName) + ' ' + RTRIM(c.LastName) AS ReportingManager, 
			a.DepartmentCode,
			dep.DepartmentName,
			a.EmployeeStatusCode,
			d.UDCDesc1 AS EmployeeStatus
	FROM kenuser.Employee a WITH (NOLOCK)
		INNER JOIN kenuser.UserDefinedCode b WITH (NOLOCK) ON RTRIM(a.GenderCode) = RTRIM(b.UDCCode)
		INNER JOIN kenuser.UserDefinedCode empType WITH (NOLOCK) ON RTRIM(a.EmploymentTypeCode) = RTRIM(empType.UDCCode)
		INNER JOIN kenuser.DepartmentMaster dep WITH (NOLOCK) ON RTRIM(a.DepartmentCode) = RTRIM(dep.DepartmentCode)
		LEFT JOIN kenuser.Employee c WITH (NOLOCK) ON a.ReportingManagerCode = c.EmployeeNo
		LEFT JOIN kenuser.UserDefinedCode d WITH (NOLOCK) ON RTRIM(a.EmployeeStatusCode) = RTRIM(d.UDCCode)
	WHERE 
		(a.EmployeeNo = @empNo OR @empNo IS NULL)
		AND (UPPER(RTRIM(a.FirstName)) LIKE '%' + UPPER(RTRIM(@firstName)) + '%' OR @firstName IS NULL)
		AND (UPPER(RTRIM(a.LastName)) LIKE '%' + UPPER(RTRIM(@lastName)) + '%' OR @lastName IS NULL)
		AND (a.ReportingManagerCode = @managerEmpNo OR @managerEmpNo IS NULL)
		AND (a.HireDate = @joiningDate OR @joiningDate IS NULL)
		AND (RTRIM(a.EmployeeStatusCode) = RTRIM(@statusCode) OR @statusCode IS NULL)
		AND (RTRIM(a.DepartmentCode) = RTRIM(@departmentCode) OR @departmentCode IS NULL)
	ORDER BY a.EmployeeNo

END

/*	Debug:

PARAMETERS:
	@empNo				INT = NULL,
	@firstName			VARCHAR(50) = NULL,
	@lastName			VARCHAR(100) = NULL,
	@managerEmpNo		INT = NULL,
	@joiningDate		DATETIME = NULL,
	@statusCode			VARCHAR(20) = NULL,
	@employmentType		VARCHAR(20) = NULL,
	@departmentCode		VARCHAR(20) = NULL

	EXEC kenuser.Pr_SearchEmployee
	EXEC kenuser.Pr_SearchEmployee 10003632
	EXEC kenuser.Pr_SearchEmployee 0, 'ervin'
	EXEC kenuser.Pr_SearchEmployee 0, '', 'brosas'


*/