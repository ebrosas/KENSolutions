DECLARE @shiftPatCode VARCHAR(20) = 'D8'

	SELECT * FROM kenuser.ShiftPatternChange a
	WHERE a.EmpNo = 10003632

	--SELECT a.ShiftPatternCode, * 
	--FROM kenuser.MasterShiftPatternTitle a 
	--WHERE RTRIM(a.ShiftPatternCode) = @shiftPatCode

	SELECT * FROM kenuser.MasterShiftPattern a
	WHERE a.ShiftPatternCode = @shiftPatCode

	SELECT * FROM kenuser.MasterShiftTime a
	WHERE a.ShiftPatternCode = @shiftPatCode
	