
	BEGIN TRAN T1

	INSERT INTO [kenuser].[OtherDocument]
           ([DocumentName]
           ,[DocumentTypeCode]
           ,[Description]
           ,[FileData]
           ,[FileExtension]
           ,[UploadDate]
           ,[TransactionNo]
           ,[EmployeeNo]
           ,[ContentTypeCode])
     SELECT	'Work Portfolio' AS [DocumentName]
			,'PORTFOLIO' AS [DocumentTypeCode]
			,'Summary of all Software System projects that were developed over the years.' AS [Description]
			,NULL AS [FileData]
			,'.PDF' AS [FileExtension]			
			,GETDATE() AS [UploadDate]
			,NULL AS [TransactionNo]
			,10003632 AS [EmployeeNo]
			,'CTPDF' AS [ContentType]

	--ROLLBACK TRAN T1
	COMMIT TRAN T1


