
	--Step 1: Verify logical file names (important!)
	RESTORE FILELISTONLY
	FROM DISK = 'C:\KenHR\DB_Backup\KenHrDb.bak';

	--Step 2: Restore the database
	RESTORE DATABASE KenHrDb
	FROM DISK = 'C:\KenHR\DB_Backup\KenHrDb.bak'
	WITH
		MOVE 'KenHrDb'     TO 'C:\KenHR\DB_Backup\SqlData\KenHrDbNew.mdf',
		MOVE 'KenHrDb_log' TO 'C:\KenHR\DB_Backup\SqlData\KenHrDbNew_log.ldf',
		REPLACE,
		RECOVERY;
