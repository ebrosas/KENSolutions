
	CREATE INDEX IX_Swipe_Emp_Date_Type
	ON kenuser.AttendanceSwipeLog (EmpNo, SwipeDate, SwipeType)
	INCLUDE (SwipeTime);

	CREATE INDEX IX_ShiftPatternChange_Emp_EffDate
	ON kenuser.ShiftPatternChange (EmpNo, EffectiveDate DESC);

	CREATE INDEX IX_MasterShiftPattern_Code_Pointer
	ON kenuser.MasterShiftPattern (ShiftPatternCode, ShiftPointer);

	CREATE INDEX IX_MasterShiftTime_Code
	ON kenuser.MasterShiftTime (ShiftPatternCode, ShiftCode);
