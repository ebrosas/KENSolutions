DECLARE	@actionType					TINYINT = 1,		--(Notes: 0 = Check records; 1 = Insert new record)
		@isCommitTrans				BIT = 1,
		@UDCCode					VARCHAR(20) = '',
		@UDCDesc1					VARCHAR(150) = NULL,
		@UDCDesc2					VARCHAR(150) = NULL,
		@UDCSpecialHandlingCode		VARCHAR(20) = NULL,
		@SequenceNo					INT = NULL,
		@IsActive					BIT = 1,
		@Amount						DECIMAL(13,3) = NULL,
		@GroupID					INT = 6 

	/*	Populate Gender
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'GENDER'

		SELECT	@UDCCode					= 'MALE',
				@UDCDesc1					= 'Male',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'FEMALE',
				@UDCDesc1					= 'Female',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

	*/

	/*	Populate Employee Status
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'EMPSTATUS'

		SELECT	@UDCCode					= 'STATEXIST',
				@UDCDesc1					= 'Existing',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STATRESIGNED',
				@UDCDesc1					= 'Resigned',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STATRETIRED',
				@UDCDesc1					= 'Retired',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STATHOLD',
				@UDCDesc1					= 'On-Hold',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STATABSCORD',
				@UDCDesc1					= 'Abscording',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STATNEWJOIN',
				@UDCDesc1					= 'New Joinee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'STATPENDING',
				@UDCDesc1					= 'Pending',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'STATNOTJOIN',
				@UDCDesc1					= 'Not Joined',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'STATTERMINATED',
				@UDCDesc1					= 'Terminated',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'STATSUSPENDED',
				@UDCDesc1					= 'Suspended',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'STATACTIVE',
				@UDCDesc1					= 'Active',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'STATINACTIVE',
			@UDCDesc1						= 'Inactive',
			@UDCDesc2						= NULL,
			@UDCSpecialHandlingCode			= NULL,
			@SequenceNo						= NULL,
			@IsActive						= 1,
			@Amount							= NULL
		
	*/

	/*	Populate Employment Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'EMPLOYTYPE'

		SELECT	@UDCCode					= 'ETYPAGENT',
				@UDCDesc1					= 'Agent',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETYPCONSULT',
				@UDCDesc1					= 'Consultant',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETYPCONTRACT',
				@UDCDesc1					= 'Contract',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETYPFULLTIME',
				@UDCDesc1					= 'Full Time',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETYPPERMANENT',
				@UDCDesc1					= 'Permanent',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETYPTRAINEE',
				@UDCDesc1					= 'Trainee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETYPLIMITCNT',
				@UDCDesc1					= 'Limited Contract Employee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'ETYPUNLICONT',
				@UDCDesc1					= 'Unlimited Contract Employee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= NULL,
				@IsActive					= 1,
				@Amount						= NULL
	*/

	/*	Populate Employee Class
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'EMPCLASS'

		SELECT	@UDCCode					= 'ECBAHRAINI',
				@UDCDesc1					= 'Bahraini',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ECBAHRAINNOGOSI',
				@UDCDesc1					= 'Bahraini without GOSI',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ECGCC',
				@UDCDesc1					= 'GCC',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ECWESTERN',
				@UDCDesc1					= 'Western',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ECEASTERN',
				@UDCDesc1					= 'Eastern',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ECASPIRE',
				@UDCDesc1					= 'Aspire',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Education Level
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'EDUCLEVEL'

		SELECT	@UDCCode					= 'ELCOLLEGEGRAD',
				@UDCDesc1					= 'College Graduate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELCOLUNDERGRAD',
				@UDCDesc1					= 'College Undergraduate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELINTERMED',
				@UDCDesc1					= 'Intermediate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELMASTER',
				@UDCDesc1					= 'Master',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELPHD',
				@UDCDesc1					= 'PHD',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELPOSTGRAD',
				@UDCDesc1					= 'Post Graduate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELPOSTUNDERGRAD',
				@UDCDesc1					= 'Post Undergraduate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELPRIMARY',
				@UDCDesc1					= 'Primary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELSECONDARY',
				@UDCDesc1					= 'Secondary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ELNOEDUC',
				@UDCDesc1					= 'No Formal Education',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Bank Account Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'ACCOUNTTYPE'

		SELECT	@UDCCode					= 'ACCCURRENT',
				@UDCDesc1					= 'Current Account',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ACCSAVINGS',
				@UDCDesc1					= 'Savings Account',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ACCFDA',
				@UDCDesc1					= 'Fixed Deposit Account',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ACCSALARY',
				@UDCDesc1					= 'Salary Account',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ACCJOINT',
				@UDCDesc1					= 'Joint Account',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ACCMINOR',
				@UDCDesc1					= 'Children / Minor Account',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Bank Names
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'BANKNAME'

		SELECT	@UDCCode					= 'BNGIB',
				@UDCDesc1					= 'Gulf International Bank (GIB)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BNAUB',
				@UDCDesc1					= 'Ahli United Bank (AUB)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BNABG',
				@UDCDesc1					= 'Al Baraka Group (ABG)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BNASB',
				@UDCDesc1					= 'Al Salam Bank',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BNNBB',
				@UDCDesc1					= 'National Bank of Bahrain (NBB)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BNBBK',
				@UDCDesc1					= 'Bank of Bahrain and Kuwait (BBK)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BNIB',
				@UDCDesc1					= 'Ithmaar Bank',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BNBISB',
				@UDCDesc1					= 'Bahrain Islamic Bank (BisB)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Visa Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'VISATYPE'

		SELECT	@UDCCode					= 'VTTOURIST',
				@UDCDesc1					= 'Tourist Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTBUSINESS',
				@UDCDesc1					= 'Business Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTTRANSIT',
				@UDCDesc1					= 'Transit Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTVISAONARRIVAL',
				@UDCDesc1					= 'Visa on Arrival',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTWORK',
				@UDCDesc1					= 'Work Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTSTUDENT',
				@UDCDesc1					= 'Student Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTFAMILY',
				@UDCDesc1					= 'Family Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTSELFSPONSOR',
				@UDCDesc1					= 'Sel-Sponsor Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTINVESTMENT',
				@UDCDesc1					= 'Investment Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VTGOLDER',
				@UDCDesc1					= 'Golden Residency Visa',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Company Branch
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'COMPANYBRANCH'

		SELECT	@UDCCode					= 'CBMAIN',
				@UDCDesc1					= 'GARMCO Main (Bahrain)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CBMAIN',
				@UDCDesc1					= 'GARMCO Foil Mill',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CBSINGAPORE',
				@UDCDesc1					= 'GARMCO Singapore',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Relationship Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'RELATIONTYPE'

		SELECT	@UDCCode					= 'RELBROTHER',
				@UDCDesc1					= 'Brother',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RELSISTER',
				@UDCDesc1					= 'Sister',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RELMOTHER',
				@UDCDesc1					= 'Mother',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RELFATHER',
				@UDCDesc1					= 'Father',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RELSPOUSE',
				@UDCDesc1					= 'Spouse',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RELSON',
				@UDCDesc1					= 'Son',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RELDAUGHTER',
				@UDCDesc1					= 'Daughter',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RELOTHER',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Qualification Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'QUALIFACTIONTYPE'

		SELECT	@UDCCode					= 'QTHSDIPLOMA',
				@UDCDesc1					= 'High School Diploma',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTTRADECERT',
				@UDCDesc1					= 'Trade Certificate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTVOCTRAIN',
				@UDCDesc1					= 'Vacational Training Certificate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTCERTPROG',
				@UDCDesc1					= 'Certificate Programs',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTDIPLOMA',
				@UDCDesc1					= 'Diploma Programs',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTASSOCDEG',
				@UDCDesc1					= 'Associate Degree',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTBACHELOR',
				@UDCDesc1					= 'Bachelors Degree',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTPOSTGRAD',
				@UDCDesc1					= 'Post Graduate Diploma',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTPOSTGRADCERT',
				@UDCDesc1					= 'Post Graduate Certificate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTMASTERDEG',
				@UDCDesc1					= 'Masters Degree',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTDOCTORDEG',
				@UDCDesc1					= 'Doctoral Degree',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 11,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QTOTHERS',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 12,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Qualification Modes
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'QUALIFACTIONMODE'

		SELECT	@UDCCode					= 'QMFULLTIME',
				@UDCDesc1					= 'Full Time',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QMPARTIME',
				@UDCDesc1					= 'Part Time',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

	*/

	/*	Populate Streams
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'STREAMTYPE'

		SELECT	@UDCCode					= 'STMEDUCATION',
				@UDCDesc1					= 'Education',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMARTSCOMM',
				@UDCDesc1					= 'Arts & Humanities',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMSOCIAL',
				@UDCDesc1					= 'Social Sciences, Journalism & Information',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMBUSINESS',
				@UDCDesc1					= 'Business, Administration & Law',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMNATURALSCN',
				@UDCDesc1					= 'Natural Sciences, Mathematics & Statistics',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMICT',
				@UDCDesc1					= 'Information & Communication Technologies ',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMENGNEERING',
				@UDCDesc1					= 'Engineering, Manufacturing & Construction',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMAGRICULTURE',
				@UDCDesc1					= 'Agriculture, Forestry, Fisheries & Veterinary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMHEALTH',
				@UDCDesc1					= 'Health & Welfare',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STMSERVICES',
				@UDCDesc1					= 'Services (hospitality, transport, personal services, etc.)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 
		
		SELECT	@UDCCode					= 'STMOTHERS',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 11,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Specialization Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'SPECIALIZATION'

		SELECT	@UDCCode					= 'SPESCIENCE',
				@UDCDesc1					= 'Science & Technology',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SPECENGING',
				@UDCDesc1					= 'Engineering',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SPECMEDICAL',
				@UDCDesc1					= 'Medical & Health Sciences',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SPECBUSINESS',
				@UDCDesc1					= 'Business & Commerce',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SPECARTS',
				@UDCDesc1					= 'Arts & Humanities',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SPECLAW',
				@UDCDesc1					= 'Law',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SPECDESIGN',
				@UDCDesc1					= 'Creative & Design',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 						
		
		SELECT	@UDCCode					= 'SPECOTHERS',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Month Codes
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'MONTHCODE'

		SELECT	@UDCCode					= 'JAN',
				@UDCDesc1					= 'January',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'FEB',
				@UDCDesc1					= 'February',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'MAR',
				@UDCDesc1					= 'March',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'APR',
				@UDCDesc1					= 'April',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'MAY',
				@UDCDesc1					= 'May',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'Jun',
				@UDCDesc1					= 'June',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'JUL',
				@UDCDesc1					= 'July',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 						
		
		SELECT	@UDCCode					= 'AUG',
				@UDCDesc1					= 'August',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SEP',
				@UDCDesc1					= 'September',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'OCT',
				@UDCDesc1					= 'October',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'NOV',
				@UDCDesc1					= 'November',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 11,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DEC',
				@UDCDesc1					= 'December',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 12,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Skill Level
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'SKILLLEVEL'

		SELECT	@UDCCode					= 'BEGINNER',
				@UDCDesc1					= 'Beginner',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'INTERMEDIATE',
				@UDCDesc1					= 'Intermediate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ADVANCED',
				@UDCDesc1					= 'Advanced',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'EXPERT',
				@UDCDesc1					= 'Expert',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'MASTER',
				@UDCDesc1					= 'Master',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

	*/

	/*	Populate Languages
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'LANGUAGE'

		SELECT	@UDCCode					= 'EN',
				@UDCDesc1					= 'English',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ZH',
				@UDCDesc1					= 'Mandarin Chinese',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'HI',
				@UDCDesc1					= 'Hindi',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ES',
				@UDCDesc1					= 'Spanish',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'FR',
				@UDCDesc1					= 'French',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'AR',
				@UDCDesc1					= 'Arabic',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BN',
				@UDCDesc1					= 'Bengali',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'PT',
				@UDCDesc1					= 'Portuguese',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RU',
				@UDCDesc1					= 'Russian',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'UR',
				@UDCDesc1					= 'Urdu',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 11,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ID',
				@UDCDesc1					= 'Indonesian',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 12,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DE',
				@UDCDesc1					= 'German',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 13,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'JA',
				@UDCDesc1					= 'Japanese',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 14,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'TG',
				@UDCDesc1					= 'TAGALOG',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 15,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'TE',
				@UDCDesc1					= 'Telugu',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 16,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'TR',
				@UDCDesc1					= 'Turkish',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 17,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'TA',
				@UDCDesc1					= 'Tamil',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 19,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'PA',
				@UDCDesc1					= 'Punjabi ',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 18,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'VI',
				@UDCDesc1					= 'Vietnamese',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 20,
				@IsActive					= 1,
				@Amount						= NULL 

	*/

	/*	Populate Document Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'DOCTYPE'

		SELECT	@UDCCode					= 'RESUME',
				@UDCDesc1					= 'Resume / Curriculum Vitae',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'COVERLETTER',
				@UDCDesc1					= 'Cover Letter / Application Letter',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CERTFRANS',
				@UDCDesc1					= 'Certificates & Transcripts',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'IDENDOCS',
				@UDCDesc1					= 'Identification Documents',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'WORKEXP',
				@UDCDesc1					= 'Work Experience Documents',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'PORTFOLIO',
				@UDCDesc1					= 'Portfolio / Work Samples',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'POLICECLEAR',
				@UDCDesc1					= 'Police Clearance Certificate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'MEDICALCERT',
				@UDCDesc1					= 'Medical Certificate',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SKILLSCERT',
				@UDCDesc1					= 'Certificates of Skills',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RECOMLETTER',
				@UDCDesc1					= 'Recommendation Letters',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DOCOTHER',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 11,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Content Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'CONTENTTYPE'

		SELECT	@UDCCode					= 'CTWORD',
				@UDCDesc1					= 'Microsoft Word (.doc / .docx)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTPOWERPT',
				@UDCDesc1					= 'PowerPoint (.ppt / .pptx)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTEXCEL',
				@UDCDesc1					= 'Excel (.xls / .xlsx)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTPDF',
				@UDCDesc1					= 'PDF (.pdf)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTPORTFOLIO',
				@UDCDesc1					= 'Portfolio File (PDF, ZIP)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTPLAINTXT',
				@UDCDesc1					= 'Plain Text (.txt)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTIMAGE',
				@UDCDesc1					= 'Image Files (JPEG, PNG, TIFF, etc.)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTOTHERS',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Salary Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'SALARYTYPE'

		SELECT	@UDCCode					= 'STBASIC',
				@UDCDesc1					= 'Basic Salary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STGROSS',
				@UDCDesc1					= 'Gross Salary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STNET',
				@UDCDesc1					= 'Net Salary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STCTC',
				@UDCDesc1					= 'Cost-to-Company (CTC)',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'STOTHERS',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

	*/

	/*	Populate Currency Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'CURRENCYTYPE'

		SELECT	@UDCCode					= 'USD',
				@UDCDesc1					= 'United States Dollar',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'EUR',
				@UDCDesc1					= 'Euro',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GBP',
				@UDCDesc1					= 'British Pound Sterling',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'BHD',
				@UDCDesc1					= 'Bahraini Dinar',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'SAR',
				@UDCDesc1					= 'Saudi Riyal',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'AED',
				@UDCDesc1					= 'UAE Dirham',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QAR',
				@UDCDesc1					= 'Qatari Riyal',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'KWD',
				@UDCDesc1					= 'Kuwaiti Dinar',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'QMR',
				@UDCDesc1					= 'Omani Rial',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'INR',
				@UDCDesc1					= 'Indian Rupee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'PKR',
				@UDCDesc1					= 'Pakistani Rupee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 11,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'PHP',
				@UDCDesc1					= 'Philippine Peso ',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 12,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CUROTHER',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 13,
				@IsActive					= 1,
				@Amount						= NULL 		
	*/

	/*	Populate Pay Grades
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'PAYGRADE'

		SELECT	@UDCCode					= 'GRADEZERO',
				@UDCDesc1					= '0',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADEONE',
				@UDCDesc1					= '1',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADETWO',
				@UDCDesc1					= '2',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADETHREE',
				@UDCDesc1					= '3',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADEFOUR',
				@UDCDesc1					= '4',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADEFIVE',
				@UDCDesc1					= '5',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADESIX',
				@UDCDesc1					= '6',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADESEVEN',
				@UDCDesc1					= '7',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADEEIGHT',
				@UDCDesc1					= '8',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADENINE',
				@UDCDesc1					= '9',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADETEN',
				@UDCDesc1					= '10',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 11,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADEELEVEN',
				@UDCDesc1					= '11',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 12,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADETWELVE',
				@UDCDesc1					= '12',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 13,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADETHIRTEEN',
				@UDCDesc1					= '13',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 14,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADEFOURTEEN',
				@UDCDesc1					= '14',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 15,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADEFIFTEEN',
				@UDCDesc1					= '15',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 16,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'GRADENA',
				@UDCDesc1					= 'N/A',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 17,
				@IsActive					= 1,
				@Amount						= NULL 
	*/

	/*	Populate Attendance Modes
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'ATTENDANCEMODE'

		SELECT	@UDCCode					= 'BIOMETRIC',
				@UDCDesc1					= 'Biometric',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'PUNCHINOUT',
				@UDCDesc1					= 'Punch In/Out',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'MOBILE',
				@UDCDesc1					= 'Mobile',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DIGITALID',
				@UDCDesc1					= 'Digital ID',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

	*/

	/*	Populate Role types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'ROLETYPES'

		SELECT	@UDCCode					= 'RTEMPLOYEE',
				@UDCDesc1					= 'Employee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RTMANAGER',
				@UDCDesc1					= 'Manager',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RTEXECUTIVE',
				@UDCDesc1					= 'Executive',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RTTRAINEE',
				@UDCDesc1					= 'Trainee',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'RTPROBY',
				@UDCDesc1					= 'Probationary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL
	*/

	/*	Populate Department Groups
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'DEPTGROUP'

		SELECT	@UDCCode					= 'DGADMIN',
				@UDCDesc1					= 'Administration',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DGPRODUCTION',
				@UDCDesc1					= 'Production',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DGOPERATION',
				@UDCDesc1					= 'Operation',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DGSALESMKT',
				@UDCDesc1					= 'Sales & Marketing',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'DGSAFETY',
				@UDCDesc1					= 'Safety & Security',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL

	*/

	/*	Populate Position Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'POSITIONTYPE'

		SELECT	@UDCCode					= 'POSTYPNEW',
				@UDCDesc1					= 'New',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'POSTYPREPLACE',
				@UDCDesc1					= 'Replacement',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'POSTYPTEMP',
				@UDCDesc1					= 'Temporary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Interview Process
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'INTERVIEWWF'

		SELECT	@UDCCode					= 'INTPRCSEQUEMCE',
				@UDCDesc1					= 'Sequential',
				@UDCDesc2					= 'Sequential Interview Workflow',
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'INTPRCPARALLEL',
				@UDCDesc1					= 'Parallel',
				@UDCDesc2					= 'Parallel Interview Workflow',
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'INTPRCSEQPAR',
				@UDCDesc1					= 'Sequential and Parallel',
				@UDCDesc2					= 'Sequential and Parallel Interview Workflow',
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Ethnicity Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'ETHNICTYPE'

		SELECT	@UDCCode					= 'ETHCOMMOM',
				@UDCDesc1					= 'Common',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETHEXPAT',
				@UDCDesc1					= 'Expat',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETHLOCAL',
				@UDCDesc1					= 'Local',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ETHNATIONAL',
				@UDCDesc1					= 'Nationals',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Customer List
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'CUSTOMERLIST'

		SELECT	@UDCCode					= 'CUSTGARMCO',
				@UDCDesc1					= 'GARMCO',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= 'Bahrain',
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CUSTBAPCO',
				@UDCDesc1					= 'BAPCO',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= 'Bahrain',
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CUSTALBA',
				@UDCDesc1					= 'Alba',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= 'Bahrain',
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CUSTMOW',
				@UDCDesc1					= 'Ministry of Works',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= 'Bahrain',
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Shift Pattern Change Types
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'SHIFTCHANGETYPE'

		SELECT	@UDCCode					= 'CTTEMP',
				@UDCDesc1					= 'Temporary',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'CTPERMNT',
				@UDCDesc1					= 'Permanent',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 
		
	*/

	/*	Populate Attendance Legends
	
		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'ATTENDLEGEND'

		SELECT	@UDCCode					= 'ALABSENT',
				@UDCDesc1					= 'Absent',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 1,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALPRESENT',
				@UDCDesc1					= 'Present',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 2,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALLATE',
				@UDCDesc1					= 'Late',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 3,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALLEFTEARLY',
				@UDCDesc1					= 'Left Early',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 4,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALLEAVE',
				@UDCDesc1					= 'On-leave',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 5,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALSICK',
				@UDCDesc1					= 'Sick Leave',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 6,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALINJURY',
				@UDCDesc1					= 'Injury Leave',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 7,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALBUSTRIP',
				@UDCDesc1					= 'Business Trip',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 8,
				@IsActive					= 1,
				@Amount						= NULL 

		SELECT	@UDCCode					= 'ALEXCUSE',
				@UDCDesc1					= 'Excused',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 9,
				@IsActive					= 1,
				@Amount						= NULL

		SELECT	@UDCCode					= 'ALOTHERS',
				@UDCDesc1					= 'Others',
				@UDCDesc2					= NULL,
				@UDCSpecialHandlingCode		= NULL,
				@SequenceNo					= 10,
				@IsActive					= 1,
				@Amount						= NULL
		
	*/
	

	IF @actionType = 0
	BEGIN

		IF ISNULL(@UDCCode, '') = ''
			SET @UDCCode = NULL 

		IF ISNULL(@GroupID, 0) = 0
			SET @GroupID = NULL 

		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE (RTRIM(a.UDCCode) = @UDCCode OR @UDCCode IS NULL)
			AND (a.GroupID = @GroupID OR @GroupID IS NULL)
    END 

	ELSE IF @actionType = 1
	BEGIN
    
		BEGIN TRAN T1
	
		INSERT INTO [kenuser].[UserDefinedCode]
        (
			[UDCCode]
           ,[UDCDesc1]
           ,[UDCDesc2]
           ,[UDCSpecialHandlingCode]
           ,[SequenceNo]
           ,[IsActive]
           ,[Amount]
           ,[GroupID]
		)
		SELECT	@UDCCode,
				@UDCDesc1,
				@UDCDesc2,
				@UDCSpecialHandlingCode,
				@SequenceNo,
				@IsActive,
				@Amount,
				@GroupID

		SELECT @@ROWCOUNT AS RowsAffected

		--Check
		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE a.GroupID = @GroupID
		ORDER BY a.UDCId

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1

	END 

/*	Data updates:

	BEGIN TRAN T1

	DELETE FROM kenuser.UserDefinedCode
	WHERE UDCId = 3384

	COMMIT TRAN T1

*/


