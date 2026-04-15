
    BEGIN TRAN T1

/*	Development DB

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
            '' AS Remarks, 
            10003636 AS AssigneeEmpNo, 
            'Fernando Magtanggol Poe' AS AssigneEmpName, 
            'ervinbrosas22@gmail.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID

	UNION

    SELECT  'HRHEAD' AS ApprovalGroupCode,
            'Head of HR' AS ApprovalGroupDesc, 
            '' AS Remarks, 
            10003633 AS AssigneeEmpNo, 
            'Antonina Ramirez Brosas' AS AssigneEmpName, 
            'antoneth_brosas28@yahoo.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID

	UNION

    SELECT  'GMOPS' AS ApprovalGroupCode,
            'GM Operations' AS ApprovalGroupDesc, 
            '' AS Remarks, 
            10003635 AS AssigneeEmpNo, 
            'Katriane Ramirez Brosas' AS AssigneEmpName, 
            'katriane.brosas@yahoo.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID
*/

/*	Staging DB

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
            '' AS Remarks, 
            10003636 AS AssigneeEmpNo, 
            'NAGENDRA SEETHARAM' AS AssigneEmpName, 
            'nagendra.seetharam@garmco.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID

	UNION

    SELECT  'HRHEAD' AS ApprovalGroupCode,
            'Head of HR' AS ApprovalGroupDesc, 
            '' AS Remarks, 
            10003633 AS AssigneeEmpNo, 
            'ANNE KIRSTEN RAMIREZ BROSAS' AS AssigneEmpName, 
            'kirsten.brosas@yahoo.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID

	UNION

    SELECT  'GMOPS' AS ApprovalGroupCode,
            'GM Operations' AS ApprovalGroupDesc, 
            '' AS Remarks, 
            10003635 AS AssigneeEmpNo, 
            'Abdulla Ebrahim Mohammed' AS AssigneEmpName, 
            'ervin.brosas@garmco.com' AS AssigneEmail, 
            GETDATE() AS CreatedDate, 
            10003632 AS CreatedBy, 
            'ervin' AS CreatedUserID

*/

    --Check
    SELECT * FROM [kenuser].[WorkflowApprovalRoles] a

    --ROLLBACK TRAN T1
    COMMIT TRAN T1

/*

	TRUNCATE TABLE [kenuser].[WorkflowApprovalRoles]

*/



