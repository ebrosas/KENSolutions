
	BEGIN TRAN T1

	INSERT INTO kenuser.LeaveRequisitionWF
           ([LeaveInstanceID]
           ,[LeaveType]
           ,[LeaveEmpNo]
           ,[LeaveEmpName]
           ,[LeaveEmpEmail]
           ,[LeaveStartDate]
           ,[LeaveEndDate]
           ,[LeaveResumeDate]
           ,[LeaveEmpCostCenter]
           ,[LeaveRemarks]
           ,[LeaveConstraints]
           ,[LeaveStatusCode]
           ,[LeaveApprovalFlag]
           ,[LeaveVisaRequired]
           ,[LeavePayAdv]
           ,[LeaveIsFTMember]
           ,[LeaveBalance]
           ,[LeaveDuration]
           ,[NoOfHolidays]
           ,[NoOfWeekends]
           ,[PlannedLeave]
           ,[LeavePlannedNo]
           ,[HalfDayLeaveFlag]
           ,[LeaveCreatedDate]
           ,[LeaveCreatedBy]
           ,[LeaveCreatedUserID]
           ,[LeaveCreatedEmail])
	--SELECT	NULL AS LeaveInstanceID,
	--		'AL' AS LeaveType,
	--		10003632 AS LeaveEmpNo,
	--		'ERVIN OLINAS BROSAS' AS LeaveEmpName,
	--		'ervin.brosas@garmco.com' AS LeaveEmpEmail,
	--		'03/01/2026' AS LeaveStartDate,
	--		'03/14/2026' AS LeaveEndDate,
	--		'03/15/2026' AS LeaveResumeDate,
	--		'7600' AS LeaveEmpCostCenter,
	--		'Short leave with family' AS LeaveRemarks,
	--		0 AS LeaveConstraints,
	--		'05' AS LeaveStatusCode,
	--		'N' AS LeaveApprovalFlag,
	--		0 AS LeaveVisaRequired,
	--		0 AS LeavePayAdv,
	--		0 AS LeaveIsFTMember,
	--		56 AS LeaveBalance,
	--		10 AS LeaveDuration,
	--		0 AS NoOfHolidays,
	--		4 AS NoOfWeekends,
	--		'N' AS PlannedLeave,
	--		NULL AS LeavePlannedNo,
	--		NULL AS HalfDayLeaveFlag,
	--		GETDATE() AS LeaveCreatedDate,
	--		10003632 AS LeaveCreatedBy,
	--		'ervin' AS LeaveCreatedUserID,
	--		'ervin.brosas@garmco.com' AS LeaveCreatedEmail

	SELECT	NULL AS LeaveInstanceID,
			'AL' AS LeaveType,
			10003632 AS LeaveEmpNo,
			'ERVIN OLINAS BROSAS' AS LeaveEmpName,
			'ervin.brosas@garmco.com' AS LeaveEmpEmail,
			'2026-01-04 00:00:00.000' AS LeaveStartDate,
			'2026-01-04 00:00:00.000' AS LeaveEndDate,
			'2026-01-04 00:00:00.000' AS LeaveResumeDate,
			'7600' AS LeaveEmpCostCenter,
			'Send my wife to hospital due to severe abdominal pain' AS LeaveRemarks,
			0 AS LeaveConstraints,
			'05' AS LeaveStatusCode,
			'N' AS LeaveApprovalFlag,
			0 AS LeaveVisaRequired,
			0 AS LeavePayAdv,
			0 AS LeaveIsFTMember,
			35.5 AS LeaveBalance,
			0.5 AS LeaveDuration,
			0 AS NoOfHolidays,
			0 AS NoOfWeekends,
			'N' AS PlannedLeave,
			NULL AS LeavePlannedNo,
			2 AS HalfDayLeaveFlag,			--Half Day on Leave Resume Date
			GETDATE() AS LeaveCreatedDate,
			10003632 AS LeaveCreatedBy,
			'ervin' AS LeaveCreatedUserID,
			'ervin.brosas@garmco.com' AS LeaveCreatedEmail

	UNION
    
	SELECT	NULL AS LeaveInstanceID,
			'AL' AS LeaveType,
			10003632 AS LeaveEmpNo,
			'ERVIN OLINAS BROSAS' AS LeaveEmpName,
			'ervin.brosas@garmco.com' AS LeaveEmpEmail,
			'2026-02-03 00:00:00.000' AS LeaveStartDate,
			'2026-02-03 00:00:00.000' AS LeaveEndDate,
			'2026-02-04 00:00:00.000' AS LeaveResumeDate,
			'7600' AS LeaveEmpCostCenter,
			'Need to bring my little daughter to hospital because shes sick for 2 days now and not getting better.' AS LeaveRemarks,
			0 AS LeaveConstraints,
			'05' AS LeaveStatusCode,
			'N' AS LeaveApprovalFlag,
			0 AS LeaveVisaRequired,
			0 AS LeavePayAdv,
			0 AS LeaveIsFTMember,
			35.5 AS LeaveBalance,
			0.5 AS LeaveDuration,
			0 AS NoOfHolidays,
			0 AS NoOfWeekends,
			'N' AS PlannedLeave,
			NULL AS LeavePlannedNo,
			1 AS HalfDayLeaveFlag,				--Half Day on Leave Start Date
			GETDATE() AS LeaveCreatedDate,
			10003632 AS LeaveCreatedBy,
			'ervin' AS LeaveCreatedUserID,
			'ervin.brosas@garmco.com' AS LeaveCreatedEmail

	ROLLBACK TRAN T1
	--COMMIT TRAN T1

