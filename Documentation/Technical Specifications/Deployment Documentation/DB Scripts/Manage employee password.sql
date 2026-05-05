
	
	SELECT a.PasswordHash, a.EmployeeNo, a.UserID, * 
	FROM kenuser.Employee a WITH (NOLOCK)
	WHERE a.EmployeeNo IN (10003632, 10003636, 10003637)


/*	Data updates:

	BEGIN TRAN T1

	UPDATE kenuser.Employee
	SET PasswordHash = 'AQAAAAIAAYagAAAAEJXV7LhjoI2r9q4BynZlrI8rD39JN92dR8IqOhFpd7zVCdcAKs5Em9yXPq85agVzyg==',
		UserID = 'kjalal'
	WHERE EmployeeNo = 10003637

	COMMIT TRAN T1

*/