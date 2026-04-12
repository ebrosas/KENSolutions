
    BEGIN TRAN T1

    INSERT INTO [kenuser].[WorkflowApprovalRoles]
           ([ApprovalGroupCode]
           ,[ApprovalGroupDesc]
           ,[Remarks]
           ,[AssigneeEmpNo]
           ,[AssigneEmpName]
           ,[AssigneEmail]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[CreatedUserID])
    SELECT  'SUPERVISOR' AS ApprovalGroupCode,
            'Immediate Supervisor' AS ApprovalGroupDesc, 
            '1st Level Approval' AS Remarks, 
            10003632 AS AssigneeEmpNo, 
            'ERVIN OLINAS BROSAS' AS AssigneEmpName, 
            'ervin.brosas@garmco.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID

    UNION

     SELECT  'CCMANAGER' AS ApprovalGroupCode,
            'Department Manager' AS ApprovalGroupDesc, 
            '2nd Level Approval' AS Remarks, 
            10003634 AS AssigneeEmpNo, 
            'ANNE KIRSTEN BROSAS' AS AssigneEmpName, 
            'kirsten.brosas@yahoo.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID

    --Check
    SELECT * FROM [kenuser].[WorkflowApprovalRoles] a

    --ROLLBACK TRAN T1
    COMMIT TRAN T1



