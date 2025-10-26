DECLARE	@actionType					TINYINT = 1,		--(Notes: 0 = Check records; 1 = Insert new record)
		@isCommitTrans				BIT = 1,
		@UDCGCode					VARCHAR(20) = '',
		@UDCGDesc1					VARCHAR(150) = '',
		@UDCGDesc2					VARCHAR(150) = '',
		@UDCGSpecialHandlingCode	VARCHAR(20) = ''

	--Add Employee Status
	--SELECT	@UDCGCode					= 'EMPSTATUS',
	--		@UDCGDesc1					= 'Employee Status',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Employment Type
	--SELECT	@UDCGCode					= 'EMPLOYTYPE',
	--		@UDCGDesc1					= 'Employment Type',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Countries
	--SELECT	@UDCGCode					= 'COUNTRY',
	--		@UDCGDesc1					= 'Countries',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Religions
	--SELECT	@UDCGCode					= 'RELIGION',
	--		@UDCGDesc1					= 'Religions',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Gender
	--SELECT	@UDCGCode					= 'GENDER',
	--		@UDCGDesc1					= 'Gender',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Marital Status
	--SELECT	@UDCGCode					= 'MARSTAT',
	--		@UDCGDesc1					= 'Marital Status',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Salutations
	--SELECT	@UDCGCode					= 'SALUTE',
	--		@UDCGDesc1					= 'Salutations',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Departments
	--SELECT	@UDCGCode					= 'DEPARTMENT',
	--		@UDCGDesc1					= 'Departments',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Employee Class
	--SELECT	@UDCGCode					= 'EMPCLASS',
	--		@UDCGDesc1					= 'Employee Class',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Education Level
	--SELECT	@UDCGCode					= 'EDUCLEVEL',
	--		@UDCGDesc1					= 'Education Level',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Account Type
	--SELECT	@UDCGCode					= 'ACCOUNTTYPE',
	--		@UDCGDesc1					= 'Account Type',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Bank Names
	--SELECT	@UDCGCode					= 'BANKNAME',
	--		@UDCGDesc1					= 'Bank Name',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Job Titles
	--SELECT	@UDCGCode					= 'JOBTITLE',
	--		@UDCGDesc1					= 'Job Titles',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Pay Grades
	--SELECT	@UDCGCode					= 'PAYGRADE',
	--		@UDCGDesc1					= 'Pay Grades',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Company Branches
	--SELECT	@UDCGCode					= 'COMPANYBRANCH',
	--		@UDCGDesc1					= 'Company Branches',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Visa Types
	--SELECT	@UDCGCode					= 'VISATYPE',
	--		@UDCGDesc1					= 'Visa Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Relationship Types
	--SELECT	@UDCGCode					= 'RELATIONTYPE',
	--		@UDCGDesc1					= 'Relationship Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Qualification Types
	--SELECT	@UDCGCode					= 'QUALIFACTIONTYPE',
	--		@UDCGDesc1					= 'Qualification Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Qualification Modes
	--SELECT	@UDCGCode					= 'QUALIFACTIONMODE',
	--		@UDCGDesc1					= 'Qualification Modes',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Stream Types
	--SELECT	@UDCGCode					= 'STREAMTYPE',
	--		@UDCGDesc1					= 'Stream Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Stream Types
	--SELECT	@UDCGCode					= 'SPECIALIZATION',
	--		@UDCGDesc1					= 'Specialization Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Month Codes
	--SELECT	@UDCGCode					= 'MONTHCODE',
	--		@UDCGDesc1					= 'Month Codes',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Skills Level
	--SELECT	@UDCGCode					= 'SKILLLEVEL',
	--		@UDCGDesc1					= 'Skill Levels',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Skills Languages
	--SELECT	@UDCGCode					= 'LANGUAGE',
	--		@UDCGDesc1					= 'Languages',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Content Types
	--SELECT	@UDCGCode					= 'CONTENTTYPE',
	--		@UDCGDesc1					= 'Content Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Documents Types
	--SELECT	@UDCGCode					= 'DOCTYPE',
	--		@UDCGDesc1					= 'Document Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Salary Types
	--SELECT	@UDCGCode					= 'SALARYTYPE',
	--		@UDCGDesc1					= 'Salary Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Currency Types
	--SELECT	@UDCGCode					= 'CURRENCYTYPE',
	--		@UDCGDesc1					= 'Currency Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Attendance Modes
	--SELECT	@UDCGCode					= 'ATTENDANCEMODE',
	--		@UDCGDesc1					= 'Attendance Modes',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Role Types
	--SELECT	@UDCGCode					= 'ROLETYPES',
	--		@UDCGDesc1					= 'Roles Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Department Groups
	--SELECT	@UDCGCode					= 'DEPTGROUP',
	--		@UDCGDesc1					= 'Department Groups',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Position Types
	--SELECT	@UDCGCode					= 'POSITIONTYPE',
	--		@UDCGDesc1					= 'Position Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Interview Process Types
	--SELECT	@UDCGCode					= 'INTERVIEWWF',
	--		@UDCGDesc1					= 'Interview Process',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	--Add Ethnicity Types
	--SELECT	@UDCGCode					= 'ETHNICTYPE',
	--		@UDCGDesc1					= 'Ethnicity Types',
	--		@UDCGDesc2					= NULL,
	--		@UDCGSpecialHandlingCode	= NULL

	IF @actionType = 0
	BEGIN

		IF ISNULL(@UDCGCode, '') = ''
			SET @UDCGCode = NULL 

		SELECT * FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE (RTRIM(a.UDCGCode) = @UDCGCode OR @UDCGCode IS NULL)
		ORDER BY a.UDCGDesc1 --a.UDCGroupId
    END 

	ELSE IF @actionType = 1
	BEGIN
    
		BEGIN TRAN T1
	
		INSERT INTO [kenuser].[UserDefinedCodeGroup]
			   ([UDCGCode]
			   ,[UDCGDesc1]
			   ,[UDCGDesc2]
			   ,[UDCGSpecialHandlingCode])
		 SELECT @UDCGCode, @UDCGDesc1, @UDCGDesc2, @UDCGSpecialHandlingCode

		SELECT @@ROWCOUNT AS RowsAffected

		--Check
		SELECT * FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = @UDCGCode

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1

	END 

/*	Debug:

	BEGIN TRAN T1

	DELETE FROM [kenuser].[UserDefinedCodeGroup]
	WHERE UDCGroupId = 21

	COMMIT TRAN T1

*/