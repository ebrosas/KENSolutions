
	BEGIN TRAN T1

	INSERT INTO [kenuser].[UserDefinedCode]
    (
        [UDCCode]
        ,[UDCDesc1]
        ,[IsActive]
        ,[GroupID]
    )

	SELECT	a.JobTitleCode,
			a.JobTitle,
			1 AS IsActive,
			(SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK) WHERE RTRIM(x.UDCGCode) = 'JOBTITLE') AS GroupID
	FROM [dbo].[JobTitle] a WITH (NOLOCK)
		

	ROLLBACK TRAN T1
	--COMMIT TRAN T1
    

