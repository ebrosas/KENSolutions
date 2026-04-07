
	
	SELECT	a.OTRequestNo, a.EmpNo, a.CostCenter,
			b.* 
	FROM tas.OvertimeRequest a WITH (NOLOCK)
		INNER JOIN [tas].[OvertimeWFApprovalHistory] b WITH (NOLOCK) ON a.OTRequestNo = b.OTRequestNo
	WHERE a.EmpNo = 10001883
		AND a.DT = '03/16/2026'
	ORDER BY b.AutoID ASC 

	SELECT * FROM tas.Master_BusinessUnit_JDE_V2 a
	WHERE RTRIM(a.BusinessUnit) = '3800'

	SELECT * FROM [tas].[OvertimeWFCostCenterMapping] a
	--WHERE RTRIM(a.CostCenter) = '3800'

	SELECT * FROM [tas].[OvertimeWFActivityTemplate] a WITH (NOLOCK)
	WHERE RTRIM(a.WFModuleCode) = 'OTPRODUCTN'
	ORDER BY a.SequenceNo

