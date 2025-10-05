DECLARE	@actionType		TINYINT = 0,		--(Notes: 0 = Check records, 1 = Insert new records)
		@isCommitTrans	BIT = 0

	IF @actionType = 0
	BEGIN

		SELECT * FROM kenuser.Employee a
		ORDER BY a.EmployeeNo

	END 

	ELSE IF @actionType = 1
	BEGIN

		BEGIN TRAN T1

		INSERT INTO [kenuser].[Employee]
			   ([FirstName]
			   ,[MiddleName]
			   ,[LastName]
			   ,[Position]
			   ,[DOB]
			   ,[NationalityCode]
			   ,[ReligionCode]
			   ,[GenderCode]
			   ,[MaritalStatusCode]
			   ,[Salutation]
			   ,[OfficialEmail]
			   ,[PersonalEmail]
			   ,[AlternateEmail]
			   ,[OfficeLandlineNo]
			   ,[ResidenceLandlineNo]
			   ,[OfficeExtNo]
			   ,[MobileNo]
			   ,[AlternateMobileNo]
			   ,EmployeeStatusCode
			   ,[ReportingManagerCode]
			   ,[WorkPermitID]
			   ,[WorkPermitExpiryDate]
			   ,[HireDate]
			   ,[DateOfConfirmation]
			   ,[TerminationDate]
			   ,[AccountHolderName]
			   ,[AccountNumber]
			   ,[AccountTypeCode]
			   ,[BankBranchCode]
			   ,[BankNameCode]
			   ,[Company]
			   ,[DateOfSuperannuation]
			   ,[EducationCode]
			   ,[EmployeeClassCode]
			   ,[EmployeeNo]
			   ,[FacebookAccount]
			   ,[IBANNumber]
			   ,[InstagramAccount]
			   ,[JobTitle]
			   ,[LinkedInAccount]
			   ,[OldEmployeeNo]
			   ,[PayGrade]
			   ,[PermanentAddress]
			   ,[PermanentAreaCode]
			   ,[PermanentCityCode]
			   ,[PermanentContactNo]
			   ,[PermanentCountryCode]
			   ,[PermanentMobileNo]
			   ,[PresentAddress]
			   ,[PresentAreaCode]
			   ,[PresentCityCode]
			   ,[PresentContactNo]
			   ,[PresentCountryCode]
			   ,[PresentMobileNo]
			   ,[Reemployed]
			   ,[TaxNumber]
			   ,[TwitterAccount])
		SELECT	'Ervin' AS [FirstName],
				'Olinas' AS [MiddleName],
				'Brosas' AS [LastName],
				'Software Engineer' AS [Position],
				'12/20/1978' AS [DOB],
				'PH' AS [NationalityCode],
				'CH' AS [ReligionCode],
				'MALE' AS [GenderCode],
				'MS-M' AS [MaritalStatusCode],
				'SAL-MR' AS [Salutation],
				'ervin.brosas@garmco.com' AS [OfficialEmail],
				'ervin.brosas@yahoo.com' AS [PersonalEmail],
				'ervin_brosas@yahoo.com' AS [AlternateEmail],
				'32229611' AS [OfficeLandlineNo],
				'' AS [ResidenceLandlineNo],
				'3152' AS [OfficeExtNo],
				'32229611' AS [MobileNo],
				'38403062' AS [AlternateMobileNo],
				NULL AS [EmployeeStatusID],
				10003662 AS [ReportingManagerCode],
				NULL AS [WorkPermitID],
				NULL AS [WorkPermitExpiryDate],
				'10/17/2011' AS [HireDate],
				'10/17/2011' AS [DateOfConfirmation],
				NULL AS [TerminationDate],
				NULL AS [AccountHolderName],
				NULL AS [AccountNumber],
				NULL AS [AccountTypeCode],
				NULL AS [BankBranchCode],
				NULL AS [BankNameCode],
				'GARMCO' AS [Company],
				NULL AS [DateOfSuperannuation],
				NULL AS [EducationCode],
				'STATACTIVE' AS [EmployeeClassCode],
				10003632 AS [EmployeeNo],
				NULL AS [FacebookAccount],
				NULL AS [IBANNumber],
				NULL AS [InstagramAccount],
				'Software Engineer' AS [JobTitle],
				NULL AS [LinkedInAccount],
				NULL AS [OldEmployeeNo],
				10 AS [PayGrade],
				'679 Burgos Street. Brgy. Bagong Anyo Liliw, Laguna' AS [PermanentAddress],
				'LAG' AS [PermanentAreaCode],
				'LILIW' AS [PermanentCityCode],
				NULL AS [PermanentContactNo],
				'PH' AS [PermanentCountryCode],
				NULL AS [PermanentMobileNo],
				'Flat 67 Bldg. 972 Road 2578 Block 234' AS [PresentAddress],
				NULL AS [PresentAreaCode],
				'JUFFAIR' AS [PresentCityCode],
				NULL AS [PresentContactNo],
				'BH' AS [PresentCountryCode],
				'32229611' AS [PresentMobileNo],
				NULL AS [Reemployed],
				NULL AS [TaxNumber],
				NULL AS [TwitterAccount]

			UNION
        
			SELECT	'Anne Kirsten' AS [FirstName],
				'Ramirez' AS [MiddleName],
				'Brosas' AS [LastName],
				'Clerk' AS [Position],
				'12/21/2007' AS [DOB],
				'PH' AS [NationalityCode],
				'CH' AS [ReligionCode],
				'FEMALE' AS [GenderCode],
				'MS-S' AS [MaritalStatusCode],
				'SAL-MISS' AS [Salutation],
				'anne.brosas@yahoo.com' AS [OfficialEmail],
				'anne.brosas@yahoo.com' AS [PersonalEmail],
				'anne.brosas@yahoo.com' AS [AlternateEmail],
				'38456987' AS [OfficeLandlineNo],
				'' AS [ResidenceLandlineNo],
				'3133' AS [OfficeExtNo],
				'38456987' AS [MobileNo],
				'39877856' AS [AlternateMobileNo],
				NULL AS [EmployeeStatusID],
				10003632 AS [ReportingManagerCode],
				NULL AS [WorkPermitID],
				NULL AS [WorkPermitExpiryDate],
				'01/01/2020' AS [HireDate],
				'01/01/2020' AS [DateOfConfirmation],
				NULL AS [TerminationDate],
				NULL AS [AccountHolderName],
				NULL AS [AccountNumber],
				NULL AS [AccountTypeCode],
				NULL AS [BankBranchCode],
				NULL AS [BankNameCode],
				'GARMCO' AS [Company],
				NULL AS [DateOfSuperannuation],
				NULL AS [EducationCode],
				'STATACTIVE' AS [EmployeeClassCode],
				10003633 AS [EmployeeNo],
				NULL AS [FacebookAccount],
				NULL AS [IBANNumber],
				NULL AS [InstagramAccount],
				'Clerk' AS [JobTitle],
				NULL AS [LinkedInAccount],
				NULL AS [OldEmployeeNo],
				10 AS [PayGrade],
				'679 Burgos Street. Brgy. Bagong Anyo Liliw, Laguna' AS [PermanentAddress],
				'LAG' AS [PermanentAreaCode],
				'LILIW' AS [PermanentCityCode],
				NULL AS [PermanentContactNo],
				'PH' AS [PermanentCountryCode],
				NULL AS [PermanentMobileNo],
				'Flat 67 Bldg. 972 Road 2578 Block 234' AS [PresentAddress],
				NULL AS [PresentAreaCode],
				'JUFFAIR' AS [PresentCityCode],
				NULL AS [PresentContactNo],
				'BH' AS [PresentCountryCode],
				'38456987' AS [PresentMobileNo],
				NULL AS [Reemployed],
				NULL AS [TaxNumber],
				NULL AS [TwitterAccount]

		SELECT @@ROWCOUNT as RowsInserted				

		--Check data
		SELECT * FROM kenuser.Employee a
		ORDER BY a.EmployeeNo

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE
			ROLLBACK TRAN T1
	END 


/*	Debug:

	TRUNCATE TABLE kenuser.Employee

	BEGIN TRAN T1

	DELETE FROM kenuser.Employee

	COMMIT TRAN T1

*/


