
	BEGIN TRAN T1

	INSERT INTO kenuser.RecruitmentBudget
    (
		[DepartmentCode]
        ,[BudgetHeadCount]
		,BudgetDescription
        ,[ActiveCount]
        ,[ExitCount]
        ,[RequisitionCount]
        ,[NetGapCount]
		,NewIndentCount
        ,[Remarks]
		,OnHold
        ,[CreatedDate]
	)
    SELECT	'7600' AS DepartmentCode,
			20 AS BudgetHeadCount, 
			'Manpower budget for ICT' AS BudgetDescription,
			9 AS ActiveCount, 
			0 AS ExitCount, 
			0 AS RequisitionCount,
			0 AS NetGapCount,
			3 AS NewIndentCount,
			'Test budget' AS Remarks, 
			0 AS OnHold,
			GETDATE() AS CreatedDate

	ROLLBACK TRAN T1
	--COMMIT TRAN T1

/*
	
	TRUNCATE TABLE kenuser.RecruitmentBudget

	SELECT * FROM kenuser.RecruitmentBudget a

*/


