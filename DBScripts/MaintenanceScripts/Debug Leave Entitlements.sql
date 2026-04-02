
	SELECT	a.EmployeeNo,
			b.FirstName + ' ' + b.MiddleName + ' ' + b.LastName AS EmpName,
			a.* 
	FROM [kenuser].[LeaveEntitlement] a WITH (NOLOCK)
		INNER JOIN kenuser.Employee b WITH (NOLOCK)  ON a.EmployeeNo = b.EmployeeNo

	--Leave Renewal Types
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'RENEWTYPE')

	--Leave Entitlement Unit of Measure
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'LEAVEUOM')

	

/*

	BEGIN TRAN T1

	UPDATE kenuser.LeaveEntitlement
	SET EffectiveDate = '01/01/2026',
		ALEntitlementCount = 27.5,
		LeaveUOM = 'LVUOMDAY',
		SLEntitlementCount = 20,
		SickLeaveUOM = 'LVUOMDAY',
		ALRenewalType = 'RENEWYEARLY',
		SLRenewalType = 'RENEWYEARLY'
	WHERE EmployeeNo IN (10003632)

	UPDATE kenuser.LeaveEntitlement
	SET EffectiveDate = '03/01/2026',
		ALEntitlementCount = 25,
		LeaveUOM = 'LVUOMDAY',
		SLEntitlementCount = 15,
		SickLeaveUOM = 'LVUOMDAY',
		ALRenewalType = 'RENEWYEARLY',
		SLRenewalType = 'RENEWYEARLY'
	WHERE EmployeeNo IN (10003633)

	COMMIT TRAN T1

*/