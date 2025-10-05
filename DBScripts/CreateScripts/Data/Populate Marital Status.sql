DECLARE @actionType		TINYINT = 0,		--(Notes: 0 = Check existing records, 1 = Insert new record)
		@isCommitTrans	BIT = 0,
		@GroupID		INT = 0 

	IF @actionType = 0
	BEGIN

		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE a.GroupID = (
			SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
			WHERE RTRIM(x.UDCGCode) = 'MARSTAT'
		)
		ORDER BY a.UDCDesc1
    END 

	ELSE IF @actionType = 1
	BEGIN 

		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'MARSTAT'

		BEGIN TRAN T1

		INSERT INTO [kenuser].[UserDefinedCode]
		(
			[UDCCode]
			,[UDCDesc1]
			,[GroupID]
		)
		VALUES
		('MS-S', 'Single', @GroupID),
		('MS-M', 'Married', @GroupID),
		('MS-W', 'Widowed', @GroupID),
		('MS-D', 'Divorced', @GroupID),
		('MS-SP', 'Separated', @GroupID),
		('MS-E', 'Engaged', @GroupID),
		('MS-P', 'Partnered / In a Relationship', @GroupID),
		('MS-C', 'Civil Union / Domestic Partnership', @GroupID),
		('MS-O', 'Other', @GroupID);

		SELECT @@ROWCOUNT AS RowsInserted

		--Check
		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE a.GroupID = (
			SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
			WHERE RTRIM(x.UDCGCode) = 'MARSTAT'
		)
		ORDER BY a.UDCDesc1

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE 
			ROLLBACK TRAN T1
	END 