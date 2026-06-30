
	SELECT 
		a.StatusCode, a.StatusID, b.UDCDesc1 as StatusDesc, a.[StatusHandlingCode],
		a.EmpNo, a.CostCenter, 
		a.* 
	FROM kenuser.OutdoorRequestWF a
		CROSS APPLY
		(
			SELECT y.* 
			FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
				INNER JOIN kenuser.UserDefinedCode y WITH (NOLOCK) ON x.UDCGroupId = y.GroupID
			where x.UDCGCode = 'STATUS'
				AND y.UDCCode = a.StatusCode
		) b
	where a.OutdoorId = 3

	EXEC kenuser.Pr_GetOutdoorRequestDetail 3


/*	Debug:

	BEGIN TRAN T1
	
	UPDATE [kenuser].[PayrollPeriod]
	SET IsActive = 1
	WHERE PayrollPeriodId = 18

	UPDATE [kenuser].[PayrollPeriod]
	SET IsActive = 0
	WHERE PayrollPeriodId = 16

	COMMIT TRAN T1
*/