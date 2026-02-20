
	SELECT	a.FailedLoginAttempts, a.IsLocked, a.PasswordHash, a.UserID, a.OfficialEmail, a.PersonalEmail,
			a.* 
	FROM [kenuser].[Employee] a
	WHERE a.EmployeeNo = 10003632

	

/*

	BEGIN TRAN T1

	UPDATE [kenuser].[Employee]
	SET UserID = 'ervin',
		PasswordHash = 'AQAAAAIAAYagAAAAEBuBc5/4KNzSH+SuzySHqeFCFNn/Pr5AMBqoQYT0OH/TmWKG8a/suptMnYJFH1yN4g=='
	WHERE EmployeeNo = 10003632

	COMMIT TRAN T1

*/