USE [JDE_PRODUCTION]
GO

/****** Object:  View [secuser].[LeaveRequisition2]    Script Date: 08/03/2026 21:08:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*****************************************************************************************************************************************************************************
*
*	View Name		:	SecUser.LeaveRequisition
*	Description		:	Retrieves all available cost centers
*
*	Created By		:	Noel G. Francisco
*	Date Created	:	10 March 2009
*
*
*	Revisions:
*	1.0				Noel				2009.03.10 13:46
*	Created
*
*	1.1				EOB					2015.02.11 10:25
*	Implemented the half day leave. Below summarizes the change done:
	- Refactored the logic in fetching data for "LeaveStartDate", "LeaveEndDate", "LeaveResumeDate" fields. Check if record exist in "LeaveRequisitionDetail" table
	- Added new fields to be returned in the query results data. These include the ff: LeaveStartDateWithTime, LeaveEndDateWithTime, LeaveResumeDateWithTime
*
*	1.2				EOB					2015.03.27 10:12
	Refactored the logic in calculating the value of the Leave Resume Date. If leave start date is equal to leave end date and half day leave flag equals 1, 2 or 3, then set the value to leave end date plus 1 day 
*
**************************************************************************************************************************************************************************************************************************/

ALTER VIEW [secuser].[LeaveRequisition2]
AS

	SELECT CASE WHEN b.LRCO IS NULL THEN (SELECT x.Company
											FROM SecUser.CostCenter AS x
											WHERE x.CostCenter = LTRIM(RTRIM(a.XXMCU)))
		ELSE b.LRCO END AS Company, a.XXY58VCRQN AS RequisitionNo,
		dbo.ConvertFromJulian(CASE
								WHEN b.LRY58VCRQD IS NULL THEN a.XXUPMJ
								ELSE b.LRY58VCRQD END) AS RequisitionDate, a.XXAN8 AS EmpNo,
		CASE
			WHEN c.LeaveEmpName IS NULL THEN (SELECT LTRIM(RTRIM(x.ABALPH))
											FROM SecUser.F0101 AS x
											WHERE a.XXAN8 = x.ABAN8)
			ELSE c.LeaveEmpName
		END AS EmpName, 
		ISNULL(c.LeaveEmpEmail, '') AS EmpEmail,
		LTRIM(RTRIM(a.XXMCU)) AS BusinessUnit, a.XXPGRD AS PayGrade, b.LRJCOD AS PositionID,
		b.LRMSTX AS MaritalStatus, b.LRSEX AS Gender, b.LRDEPT AS DepartmentCode,
		dbo.ConvertFromJulian(b.LRDSI) AS DateOfEmployment, (ISNULL(b.LRY57EPRSD, 0) / 1000) AS ServiceDuration,
		LTRIM(RTRIM(a.XXY58VCVCD)) AS LeaveType, b.LRY58MSFN AS MedicalFormNo,
		
		/**************************************************** Part of Revision No. 1.1 *************************************************/		
		--CASE WHEN ISNULL(LeaveReqDetailID, 0) > 0 
		--	THEN f.LeaveStartDate
		--	ELSE dbo.ConvertFromJulian(a.XXY58VCOFD)
		--	END AS LeaveStartDate, 

		--CASE WHEN ISNULL(LeaveReqDetailID, 0) > 0 
		--	THEN f.LeaveEndDate
		--	ELSE CASE
		--			WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
		--			ELSE dbo.ConvertFromJulian(a.XXY58VCOTD)
		--		 END
		--	END AS LeaveEndDate,

		--CASE WHEN ISNULL(LeaveReqDetailID, 0) > 0 
		--	THEN f.LeaveResumeDate
		--	ELSE CASE
		--			WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
		--			ELSE DATEADD(dd, 1, dbo.ConvertFromJulian(a.XXY58VCOTD))
		--		 END 
		--	END AS LeaveResumeDate,

		dbo.ConvertFromJulian(a.XXY58VCOFD) AS LeaveStartDate, 

		CASE
			WHEN dbo.ConvertFromJulian(a.XXY58VCOTD) < dbo.ConvertFromJulian(a.XXY58VCOFD) THEN --(Note: Check if Leave End Date < Leave Start Date. If true, then set Leave End Date = Leave Resume Date)
				CASE	
					WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
					WHEN dbo.ConvertFromJulian(a.XXY58VCOFD) = dbo.ConvertFromJulian(a.XXY58VCOTD) THEN dbo.ConvertFromJulian(a.XXY58VCOTD)
					ELSE DATEADD(dd, 1, dbo.ConvertFromJulian(a.XXY58VCOTD))
				END
			ELSE dbo.ConvertFromJulian(ISNULL(a.XXY58VCOTD, a.XXY58VCOFD))
		END AS LeaveEndDate,

		CASE		
			WHEN (dbo.ConvertFromJulian(a.XXY58VCOFD) = dbo.ConvertFromJulian(a.XXY58VCOTD) AND b.LREV02 IN ('1')) THEN DATEADD(dd, 1, dbo.ConvertFromJulian(a.XXY58VCOTD))	
			WHEN (dbo.ConvertFromJulian(a.XXY58VCOFD) = dbo.ConvertFromJulian(a.XXY58VCOTD) AND b.LREV02 IN ('2', '3')) THEN dbo.ConvertFromJulian(a.XXY58VCOTD)
			WHEN (dbo.ConvertFromJulian(a.XXY58VCOFD) < dbo.ConvertFromJulian(a.XXY58VCOTD) AND b.LREV02 IN ('2', '3')) THEN dbo.ConvertFromJulian(a.XXY58VCOTD)
			WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
			ELSE DATEADD(dd, 1, dbo.ConvertFromJulian(a.XXY58VCOTD))
		END AS LeaveResumeDate,
		/**************************************************** End of Revision No. 1.1 *************************************************/

		--dbo.ConvertFromJulian(a.XXY58VCOFD) AS LeaveStartDate,
		--CASE
		--	WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
		--	ELSE dbo.ConvertFromJulian(a.XXY58VCOTD)
		--END AS LeaveEndDate,
		--CASE
		--	WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
		--	ELSE DATEADD(dd, 1, dbo.ConvertFromJulian(a.XXY58VCOTD))
		--END AS LeaveResumeDate,

		(ISNULL(b.LRY58VCCVC, 0) / 10000) AS LeaveBalance, b.LRY58VCLVM AS LeaveBalUOM, b.LRY58TKEBL AS EntitledBalAmount,
		(ISNULL(b.LRY58VCVCT, 0) / 10000) AS CurrentLeaveBalance,
		b.LRY58VCCVM AS CurrentLeaveBalUOM, b.LRY58TKCBL AS CurrentLeaveBalAmount,
		(ISNULL(a.XXY58VCVDR, 0) / 10000) AS LeaveDuration, 
		b.LRY58VCUOM AS LeaveUOM,
		b.LRY58TKRTA AS CalculatedTicketAmount, b.LRCRCD AS CurrencyCodeFrom, ISNULL(b.LRY58TKEFG, 'N') AS ExceptionFlag,
		ISNULL(b.LRY58VCHFD, 'F') AS HalfDayLeave, ISNULL(b.LRAN81, 0) AS SubEmpNo, ISNULL(e.EmpName, '') AS SubEmpName, a.XXY58VCAFG AS ApprovalFlag,
		ISNULL(b.LRY58VCOFT, '') AS ShiftStartTime, ISNULL(b.LRY58VCOTT, '') AS ShiftEndTime,
		dbo.ConvertFromJulian(b.LRY58VCADT) AS ApprovalDate, b.LREV01 AS EverestEventPoint01,
		CASE
			WHEN ISNULL(a.XXY58VCALF, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
			ELSE dbo.ConvertFromJulian(a.XXY58VCALF)
		END AS ActualLeaveStartDate,
		CASE
			WHEN ISNULL(a.XXY58VCARD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
			ELSE dbo.ConvertFromJulian(a.XXY58VCARD)
		END AS ActualLeaveReturnDate,
		(ISNULL(a.XXY58VCALD, 0) / 10000) AS ActualLeaveDuration, b.LRY58VCAUM AS ActualLeaveUOM,
		LTRIM(RTRIM(a.XXY58VCRFG)) AS PlannedLeave, ISNULL(b.LRY58VCSPL, 'N') AS SpecialLeave, b.LRY58VCSPD AS SalaryPaid,
		b.LRY58VCTRA AS TicketRequisitionApplied, b.LRY58VCCAC AS JECreditAccount, b.LRY58VCDAC AS JEDebitAccount,
		b.LRDOC AS DocVoucherInvoiceE, b.LRDCT AS DocumentType, b.LRICU AS BatchNo, b.LRICUT AS BatchType,
		b.LRPDBA AS PayDeductBenAcctType, b.LRY58VCDBA AS PayCode,
		b.LRY58TKICU AS VRBatchNo, b.LRY58TKCUT AS VRBatchType,
		b.LRY58TKDCU AS DBatchNo, b.LRY58TKDBT AS DBatchType, b.LREV02 AS EverestEventPoint02,
		b.LRED01 AS UserDefinedDate1, b.LRED02 UserDefinedDate2,
		ISNULL(c.LeaveVisaRequired, 0) AS VisaRequired, ISNULL(CONVERT(FLOAT, b.LRUDF2), 0) AS LeavePlannedNo,
		(ISNULL(b.LRNUM1, 0) / 100) AS NoOfHolidays, (ISNULL(b.LRNUM2, 0) / 100) AS NoOfWeekends,
		ISNULL(c.LeaveInstanceID, '') AS InstanceID,
		ISNULL(CASE
			WHEN c.LeaveReqTypeID IS NULL THEN (SELECT x.ReqTypeID
													FROM SecUser.RequestType AS x
													WHERE x.ReqTypeCode = LTRIM(RTRIM(a.XXY58VCVCD)))
			ELSE c.LeaveReqTypeID
		END, 0) AS RequestTypeID,
		ISNULL(CASE
			WHEN c.LeaveReqTypeName IS NULL THEN (SELECT x.ReqTypeName
													FROM SecUser.RequestType AS x
													WHERE x.ReqTypeCode = LTRIM(RTRIM(a.XXY58VCVCD)))
			ELSE c.LeaveReqTypeName
		END, '') AS RequestTypeName, ISNULL(c.LeavePayAdv, 0) AS LeavePayAdv,
		LTRIM(RTRIM(ISNULL(c.LeaveJustification, ''))) AS Justification, LTRIM(RTRIM(ISNULL(c.LeaveRemarks, ''))) AS Remarks,
		ISNULL(c.LeaveCreatedBy, 0) AS CreatedBy, ISNULL(c.LeaveCreatedName, '') AS CreatedName,
		ISNULL(c.LeaveCreatedEmail, '') AS CreatedEmail,
		c.LeaveCreatedDate AS CreatedDate,
		ISNULL(c.LeaveModifiedBy, 0) AS ModifiedBy, a.XXUSER AS ModifiedName,
		ISNULL(c.LeaveModifiedEmail, '') AS ModifiedEmail,
		b.LRPID AS ProgramID, b.LRJOBN AS WorkstationID,
		SecUser.CombineDateTime(ISNULL(b.LRUPMT, 0), a.XXUPMJ) AS LastDateUpdated,
		ISNULL(d.UDCID, 0) AS RequestStatusID, ISNULL(d.UDCCode, '') AS RequestStatusCode,
		ISNULL(d.UDCDesc1, '') AS RequestStatusDesc,
		a.XXRMK AS Remark,
		ISNULL(c.LeaveRetError, 0) AS RetError,

		/**************************************************** Part of Revision No. 1.1 *************************************************/		
		b.LREV02 AS HalfDayLeaveFlag,
		f.LeaveStartDate AS LeaveStartDateWithTime,
		f.LeaveEndDate AS LeaveEndDateWithTime,
		f.LeaveResumeDate AS LeaveResumeDateWithTime
				
		--CASE WHEN ISNULL(b.LREV02, '') = 'S' AND ISNULL(b.LRY58VCOFT, '') <> '' 
		--	THEN DATEADD(n, DATEPART(n, b.LRY58VCOFT), DATEADD(hh, DATEPART(hh, b.LRY58VCOFT), dbo.ConvertFromJulian(a.XXY58VCOFD))) 
		--	ELSE dbo.ConvertFromJulian(a.XXY58VCOFD)
		--	END AS LeaveStartDateWithTime,

		--CASE WHEN UPPER(ISNULL(b.LREV02, '')) = 'E' AND ISNULL(b.LRY58VCOTT, '') <> '' 
		--	THEN DATEADD(n, DATEPART(n, b.LRY58VCOTT), DATEADD(hh, DATEPART(hh, b.LRY58VCOTT), dbo.ConvertFromJulian(ISNULL(a.XXY58VCOTD, a.XXY58VCOFD)))) 
		--	ELSE dbo.ConvertFromJulian(ISNULL(a.XXY58VCOTD, a.XXY58VCOFD))
		--	END AS LeaveEndDateWithTime,

		--CASE WHEN ISNULL(b.LREV02, '') IN ('S', 'E') AND ISNULL(b.LRY58VCOTT, '') <> '' 
		--	THEN DATEADD(n, DATEPART(n, b.LRY58VCOTT), DATEADD(hh, DATEPART(hh, b.LRY58VCOTT), CASE
		--													WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
		--													ELSE DATEADD(dd, 1, dbo.ConvertFromJulian(a.XXY58VCOTD))
		--												 END)) 
		--	ELSE CASE
		--			WHEN ISNULL(a.XXY58VCOTD, 0) = 0 THEN dbo.ConvertFromJulian(a.XXY58VCOFD)
		--			ELSE DATEADD(dd, 1, dbo.ConvertFromJulian(a.XXY58VCOTD))
		--		 END
		--	END AS LeaveResumeDateWithTime  
		/**************************************************** End of Revision No. 1.1 *************************************************/

	FROM SecUser.LeaveHistoryCombined AS a 
		INNER JOIN SecUser.F58LV13 AS b ON a.XXY58VCRQN = b.LRY58VCRQN AND a.XXRMK = 'Leave Taken' 
		LEFT JOIN SecUser.LeaveRequisitionWF AS c ON b.LRY58VCRQN = c.LeaveNo AND b.LRAN8 = c.LeaveEmpNo 
		LEFT JOIN SecUser.UserDefinedCode AS d ON
			(CASE
				WHEN c.LeaveReqStatusID IS NULL AND a.XXY58VCAFG IN ('A', 'N') THEN 131
				WHEN c.LeaveReqStatusID IS NULL AND a.XXY58VCAFG = 'R' THEN 58
				WHEN c.LeaveReqStatusID IS NULL AND a.XXY58VCAFG = 'C' THEN 57
				ELSE ISNULL(c.LeaveReqStatusID, 0)
			END) = d.UDCID AND d.UDCUDCGID = 9 
		LEFT JOIN SecUser.EmployeeMaster AS e ON b.LRAN81 = e.EmpNo
		LEFT JOIN secuser.LeaveRequisitionDetail f ON a.XXY58VCRQN = f.LeaveNo
GO


