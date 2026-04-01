
	SELECT	a.EmployeeNo,
			b.FirstName + ' ' + b.MiddleName + ' ' + b.LastName AS EmpName,
			a.* 
	FROM [kenuser].[LeaveEntitlement] a WITH (NOLOCK)
		INNER JOIN kenuser.Employee b WITH (NOLOCK)  ON a.EmployeeNo = b.EmployeeNo