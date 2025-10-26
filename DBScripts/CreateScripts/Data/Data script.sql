
	--Check all EF migrations
	SELECT * FROM [dbo].[__EFMigrationsHistory]

	SELECT * FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
	ORDER BY a.UDCGDesc1

	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = 8

	--Get Employee Status
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'EMPSTATUS')

	--Get Employment Type
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'QUALIFACTIONMODE')

	--Get Countries
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'COUNTRY')
	
	--Get Salutations
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'SALUTE')

	--Get Attendance Modes
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'ATTENDANCEMODE')

	--General
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'STREAMTYPE')

	SELECT a.EmployeeId, a.EmployeeNo, 
		a.EmploymentTypeCode, a.RoleCode, a.JobTitleCode, a.FirstAttendanceModeCode, a.SecondAttendanceModeCode, a.ThirdAttendanceModeCode,
		a.SecondReportingManagerCode,
		a.* 
	FROM kenuser.Employee a
	WHERE a.EmployeeNo = 10003633

	SELECT * FROM kenuser.UserDefinedCode a
	ORDER BY a.UDCDesc1

	SELECT a.PlaceOfIssue, * 
	FROM [kenuser].[IdentityProof] a WITH (NOLOCK)
	WHERE a.EmployeeNo = 10003632

	SELECT  * 
	FROM [kenuser].[EmergencyContact] a WITH (NOLOCK)
	WHERE a.EmployeeNo = 10003632

	SELECT * FROM kenuser.DepartmentMaster a

	SELECT * FROM [kenuser].[EmploymentHistory] a WHERE a.EmployeeNo = 10003632	
	SELECT * FROM [kenuser].[OtherDocument] a WHERE a.EmployeeNo = 10003632	
	SELECT * FROM kenuser.FamilyMember a WHERE a.EmployeeNo = 10003632
	SELECT * FROM kenuser.FamilyVisa a WHERE a.EmployeeNo = 10003632

	SELECT * FROM [kenuser].[LanguageSkill] a WHERE a.EmployeeNo = 10003632

	SELECT * FROM [kenuser].EmployeeCertification a WHERE a.EmployeeNo = 10003632
	
	SELECT * FROM [kenuser].[EmployeeSkill] a WHERE a.EmployeeNo = 10003632

	SELECT a.EmployeeNo, * FROM [kenuser].[IdentityProof] a

	SELECT * FROM kenuser.EmergencyContact a WITH (NOLOCK)

	SELECT * FROM kenuser.Qualification a WITH (NOLOCK)

	SELECT * from [kenuser].[DepartmentMaster] a
	ORDER BY a.DepartmentName

	SELECT * FROM kenuser.RecruitmentBudget a

	

/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.Employee
	SET DepartmentCode = '7600',
		EmployeeStatusCode = 'STATACTIVE',
		EmploymentTypeCode = 'ETYPPERMANENT'
	WHERE EmployeeNo IN (10003632)

	UPDATE kenuser.Employee
	SET DepartmentCode = '7250',
		EmployeeStatusCode = 'STATNEWJOIN',
		EmploymentTypeCode = 'ETYPFULLTIME'
	WHERE EmployeeNo IN (10003633)
	
	UPDATE kenuser.Employee
	SET RoleCode = 'RTEMPLOYEE',
		JobTitleCode = '000620',
		FirstAttendanceModeCode = 'BIOMETRIC'
	WHERE EmployeeNo = 10003632

	UPDATE kenuser.Employee
	SET RoleCode = 'RTEMPLOYEE',
		JobTitleCode = '000586',
		FirstAttendanceModeCode = 'PUNCHINOUT'
	WHERE EmployeeNo = 10003633

	DELETE FROM [dbo].[__EFMigrationsHistory]
	WHERE MigrationID = '20250913143228_UpdateFamilyMember'

	COMMIT TRAN T1

*/

/*	Data deletion

	TRUNCATE TABLE [kenuser].[IdentityProof]

*/