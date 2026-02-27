
	SELECT	a.EmployeeNo, a.EmployeeId, a.FailedLoginAttempts, a.IsLocked, a.PasswordHash, a.UserID, a.OfficialEmail, a.PersonalEmail,
			a.HireDate, a.OfficialEmail, 
			a.EmailVerificationToken, a.EmailVerificationTokenExpiry, a.IsEmailVerified,
			a.* 
	FROM [kenuser].[Employee] a
	WHERE a.EmployeeNo = 10003632

	SELECT	a.EmployeeNo, a.EmployeeId, a.FailedLoginAttempts, a.IsLocked, a.PasswordHash, a.UserID, a.OfficialEmail, a.PersonalEmail,
			a.HireDate, a.OfficialEmail, 
			a.EmailVerificationToken, a.EmailVerificationTokenExpiry, a.IsEmailVerified,
			a.* 
	FROM [kenuser].[Employee] a
	--WHERE a.EmployeeNo = 10003636

	

/*

	BEGIN TRAN T1

	UPDATE [kenuser].[Employee]
	SET UserID = 'ervin',
		PasswordHash = 'AQAAAAIAAYagAAAAEBuBc5/4KNzSH+SuzySHqeFCFNn/Pr5AMBqoQYT0OH/TmWKG8a/suptMnYJFH1yN4g=='
	WHERE EmployeeNo = 10003632

	UPDATE [kenuser].[Employee]
	SET EmployeeNo = 10003589
	WHERE EmployeeId = 1006

	COMMIT TRAN T1
	ROLLBACK TRAN T1

*/