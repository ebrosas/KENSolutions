
	--Request Types
	SELECT a.* 
	FROM kenuser.UserDefinedCode a WITH (NOLOCK)
	WHERE a.GroupID = (SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'REQTYPE')
	ORDER BY a.UDCDesc1

/*	Set attendance related request types

	BEGIN TRAN T1

	UPDATE kenuser.UserDefinedCode
	SET UDCSpecialHandlingCode = 'ATTENDANCE'
	WHERE RTRIM(UDCCode) IN ('RTYPEOT', 'RTYPEREGULAR', 'RTYPEOUTDOOR')

	UPDATE kenuser.UserDefinedCode
	SET UDCSpecialHandlingCode = NULL
	WHERE RTRIM(UDCCode) IN ('RTYPELEAVE')

	COMMIT TRAN T1

*/

/*	Change Overtime to Extra Time

	BEGIN TRAN T1

	UPDATE kenuser.UserDefinedCode
	SET UDCDesc1 = 'Extra Time'
	WHERE RTRIM(UDCCode) IN ('RTYPEOT')

	COMMIT TRAN T1

*/