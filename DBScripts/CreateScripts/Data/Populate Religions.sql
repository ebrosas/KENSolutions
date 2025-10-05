DECLARE @actionType		TINYINT = 0,		--(Notes: 0 = Check existing records, 1 = Insert new record)
		@isCommitTrans	BIT = 0,
		@GroupID		INT = 0 

	IF @actionType = 0
	BEGIN

		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE a.GroupID = (
			SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
			WHERE RTRIM(x.UDCGCode) = 'RELIGION'
		)
		ORDER BY a.UDCDesc1
    END 

	ELSE IF @actionType = 1
	BEGIN 

		SELECT @GroupID = a.UDCGroupId 
		FROM kenuser.UserDefinedCodeGroup a WITH (NOLOCK)
		WHERE RTRIM(a.UDCGCode) = 'RELIGION'

		BEGIN TRAN T1

		INSERT INTO [kenuser].[UserDefinedCode]
		(
			[UDCCode]
			,[UDCDesc1]
			,[GroupID]
		)
		VALUES
		('CH', 'Christianity', @GroupID),
		('CA', 'Catholicism', @GroupID),
		('PR', 'Protestantism', @GroupID),
		('OR', 'Orthodox Christianity', @GroupID),
		('IS', 'Islam', @GroupID),
		('SI', 'Sunni Islam', @GroupID),
		('SH', 'Shia Islam', @GroupID),
		('JU', 'Judaism', @GroupID),
		('HI', 'Hinduism', @GroupID),
		('BU', 'Buddhism', @GroupID),
		('CO', 'Confucianism', @GroupID),
		('TA', 'Taoism', @GroupID),
		('SHI', 'Shinto', @GroupID),
		('SIKH', 'Sikhism', @GroupID),
		('BA', 'Baha''i Faith', @GroupID),
		('JA', 'Jainism', @GroupID),
		('ZO', 'Zoroastrianism', @GroupID),
		('SP', 'Spiritism', @GroupID),
		('AN', 'Animism / Indigenous Beliefs', @GroupID),
		('NO', 'Non-religious / Secular', @GroupID),
		('AT', 'Atheism', @GroupID),
		('AG', 'Agnosticism', @GroupID),
		('OT', 'Other', @GroupID);

		SELECT @@ROWCOUNT AS RowsInserted

		--Check
		SELECT * FROM kenuser.UserDefinedCode a WITH (NOLOCK)
		WHERE a.GroupID = (
			SELECT x.UDCGroupId FROM kenuser.UserDefinedCodeGroup x WITH (NOLOCK)
			WHERE RTRIM(x.UDCGCode) = 'RELIGION'
		)
		ORDER BY a.UDCDesc1

		IF @isCommitTrans = 1
			COMMIT TRAN T1
		ELSE 
			ROLLBACK TRAN T1
	END 