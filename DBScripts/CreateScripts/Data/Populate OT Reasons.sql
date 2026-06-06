
	BEGIN TRAN T1
	
	INSERT INTO [kenuser].[UserDefinedCode]
    (
		[UDCCode]
        ,[UDCDesc1]
        ,[UDCDesc2]
        ,[UDCSpecialHandlingCode]
        ,[SequenceNo]
        ,[IsActive]
        ,[Amount]
        ,[GroupID]
	)
	SELECT	'ACS' AS UDCCode,
			'Add OT Change Shift' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			1 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'MA' AS UDCCode,
			'Add OT Manager Approval' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			2 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'OTAL' AS UDCCode,
			'Annual Leave' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			3 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'BD' AS UDCCode,
			'Break Down' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			4 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'CAL' AS UDCCode,
			'Call Out Annual Leave' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			5 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'CBD' AS UDCCode,
			'Call Out Break Down' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			6 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'COEW' AS UDCCode,
			'Call Out Extra Work' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			7 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'CDF' AS UDCCode,
			'Call Out Family Death' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			8 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'COMS' AS UDCCode,
			'Call Out Manpower Shortage' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			9 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'CSR' AS UDCCode,
			'Call Out Sick' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			10 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'CCS' AS UDCCode,
			'Change OT Change Shift' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			11 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'EW' AS UDCCode,
			'Extra Work/ Special Task' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			12 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'DF' AS UDCCode,
			'Family Death' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			13 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'SR' AS UDCCode,
			'Leave (Sick, Injury, Light Duty)' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			14 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'MS' AS UDCCode,
			'Manpower Shortage' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			15 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'ROT' AS UDCCode,
			'OT for Ramadan' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			16 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'PM' AS UDCCode,
			'Planned Maintenance' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			17 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'PD' AS UDCCode,
			'Project / Development' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			18 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'PH' AS UDCCode,
			'Public Holiday' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			19 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'SD' AS UDCCode,
			'Shutdown' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			20 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	UNION

	SELECT	'TR' AS UDCCode,
			'Training' AS UDCDesc1,
			NULL AS UDCDesc2,
			NULL AS UDCSpecialHandlingCode,
			21 AS SequenceNo,
			1 AS IsActive,
			NULL AS Amount,
			4041 AS GroupID

	SELECT @@ROWCOUNT AS RowsAffected


	ROLLBACK TRAN T1
	--COMMIT TRAN T1
	

