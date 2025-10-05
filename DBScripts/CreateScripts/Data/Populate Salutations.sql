DECLARE @actionType		TINYINT = 0,		--(Notes: 0 = Check existing records, 1 = Insert new record)
		@isCommitTrans	BIT = 0,
		@GroupID		INT = 0 

	IF @actionType = 0
	BEGIN

		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE a.GroupID = (
			SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
			WHERE RTRIM(x.UDCGCode) = 'SALUTE'
		)
		ORDER BY a.UDCDesc1
    END 

	ELSE IF @actionType = 1
	BEGIN 

		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'SALUTE'

		BEGIN TRAN T1

		INSERT INTO [kenuser].[UserDefinedCode]
		(
			[UDCCode]
			,[UDCDesc1]
			,[GroupID]
		)
		VALUES
		('SAL-MR', 'Mr.', @GroupID),
		('SAL-MRS', 'Mrs.', @GroupID),
		('SAL-MS', 'Ms.', @GroupID),
		('SAL-MISS', 'Miss', @GroupID),
		('SAL-DR', 'Dr.', @GroupID),
		('PROF', 'Prof.', @GroupID),
		('SAL-SAL-', 'Rev.', @GroupID),
		('SAL-SIR', 'Sir', @GroupID),
		('SAL-MADAM', 'Madam', @GroupID),
		('SAL-LORD', 'Lord', @GroupID),
		('SAL-LADY', 'Lady', @GroupID),
		('SAL-MX', 'Mx.', @GroupID),
		('SAL-ENG', 'Engr.', @GroupID),
		('SAL-ATTY', 'Atty.', @GroupID),
		('SAL-HON', 'Hon.', @GroupID),
		('SAL-COL', 'Col.', @GroupID),
		('SAL-MAJ', 'Maj.', @GroupID),
		('SAL-CAPT', 'Capt.', @GroupID),
		('SAL-GEN', 'Gen.', @GroupID),
		('SAL-SGT', 'Sgt.', @GroupID),
		('SAL-CPT', 'Cpt.', @GroupID),
		('SAL-PST', 'Pastor', @GroupID),
		('SAL-OTH', 'Other', @GroupID)

		SELECT @@ROWCOUNT AS RowsInserted

		--Check
		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE a.GroupID = (
			SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
			WHERE RTRIM(x.UDCGCode) = 'SALUTE'
		)
		ORDER BY a.UDCDesc1

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE 
			ROLLBACK TRAN T1
	END 