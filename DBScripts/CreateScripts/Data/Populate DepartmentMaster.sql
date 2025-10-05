
	--BEGIN TRAN T1

	--INSERT INTO [kenuser].[DepartmentMaster]
 --   (
	--	[DepartmentCode]
 --       ,[DepartmentName]
 --       ,[GroupCode]
 --       --,[Description]
 --       ,[ParentDepartmentId]
 --       ,[SuperintendentEmpNo]
 --       ,[ManagerEmpNo]
 --       ,[IsActive]
 --       ,[CreatedAt]
 --       --,[UpdatedAt]
	--)

	SELECT	a.DepartmentCode,
			a.DepartmentName,
			a.GroupCode,
			a.ParentDepartmentId,
			a.Superintendent,
			a.CostCenterManager,
			1 AS IsActive,
			GETDATE() AS CreatedAt
	FROM [dbo].[DepartmentExport] a WITH (NOLOCK)

	--ROLLBACK TRAN T1
	--COMMIT TRAN T1
    

