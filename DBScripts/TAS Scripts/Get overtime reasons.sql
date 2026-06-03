
	--Get overtime reasons for approval
	SELECT Code, [Description] 
	FROM tas.Master_OTReasons_JDE
	WHERE CODE IN 
	(
		'AL',
		'BD',
		'EW',
		'MS',
		'PD',
		'PH',
		'PM',
		'SD',
		'SR',
		'TR',
		'DF',
		'CAL',
		'CBD',
		'CSR',
		'CDF',
		'COMS',
		'COEW',
		'ACS',
		'CCS',
		'MA',
		'ROT'		--Rev. #1.1
	) 

	UNION
    
	--Get overtime reasons for rejection
	SELECT	Code, [Description] 
	FROM tas.Master_OTReasons_JDE a
	WHERE SUBSTRING(LTRIM(RTRIM(a.CODE)), 1, 2) IN ('RO')
	ORDER BY [Description]