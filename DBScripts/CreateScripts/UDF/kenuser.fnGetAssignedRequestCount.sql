/************************************************************************************************************************************************************************
*	Revision History
*
*	Name: tas.fnGetAssignedRequestCount
*	Description: This function is used to get the total late count of an employee on a given period
*
*	Date:			Author:		Rev.#:		Comments:
*	10/02/2026		Ervin		1.0			Created
*	14/05/2026		Ervin		1.1			Moified the logic for fetching the number of assigned request
*
**************************************************************************************************************************************************************************/

ALTER FUNCTION kenuser.fnGetAssignedRequestCount
(
	@requestTypeCode	VARCHAR(20),
    @empNo				INT,
    @startDate			DATETIME,
    @endDate			DATETIME
)
RETURNS INT
AS
BEGIN

    DECLARE @assignedCount INT;

	--Validate parameters
	IF ISNULL(@startDate, '') = '' OR CAST(@startDate AS DATETIME) = CAST('' AS DATETIME)
		SET @startDate = NULL

	IF ISNULL(@endDate, '') = '' OR CAST(@endDate AS DATETIME) = CAST('' AS DATETIME)
		SET @endDate = NULL

	--SELECT @assignedCount = COUNT(a.ApprovalId)
	--FROM kenuser.RequestApprovals a WITH (NOLOCK)
	--WHERE a.AssignedEmpNo = @empNo
	--	AND RTRIM(a.RequestTypeCode) = @requestTypeCode
	--	AND 
	--	(
	--		(a.CreatedDate BETWEEN @startDate AND @endDate AND @startDate IS NOT NULL AND @endDate IS NOT NULL)
	--		OR (@startDate IS NULL AND @endDate IS NULL)
	--	)

	--Start of Rev. #1.1
	SELECT @assignedCount = COUNT(c.EntityId) 
	FROM kenuser.WorkflowStepDefinitions a WITH (NOLOCK)
		INNER JOIN kenuser.WorkflowDefinitions b WITH (NOLOCK) ON a.WorkflowDefinitionId = b.WorkflowDefinitionId
		INNER JOIN kenuser.WorkflowInstances c WITH (NOLOCK) ON b.WorkflowDefinitionId = c.WorkflowDefinitionId
		CROSS APPLY
		(
			SELECT RTRIM(x.UDCDesc1) AS RequestTypeDesc  
			FROM kenuser.UserDefinedCode x WITH (NOLOCK)
			WHERE x.GroupID = (SELECT UDCGroupId FROM kenuser.UserDefinedCodeGroup WITH (NOLOCK) WHERE RTRIM(UDCGCode) = 'REQTYPE')
				AND RTRIM(x.UDCCode) = RTRIM(b.EntityName)
		) udc 
		OUTER APPLY
		(
			SELECT	x.[Status] as ActivityStatus,
					x.ApproverEmpNo AS ApproverNo,
					RTRIM(ISNULL(y.FirstName, '')) + ' ' + RTRIM(ISNULL(y.MiddleName, '')) + ' ' + RTRIM(ISNULL(y.LastName, '')) AS ApproverName,
					x.ActionDate,
					x.StepInstanceId		--Rev. #1.1
			FROM kenuser.WorkflowStepInstances x WITH (NOLOCK)
				LEFT JOIN kenuser.Employee y WITH (NOLOCK) ON x.ApproverEmpNo = y.EmployeeNo
			WHERE x.WorkflowInstanceId = c.WorkflowInstanceId
				and x.StepDefinitionId = a.StepDefinitionId
		) d
	WHERE RTRIM(d.ActivityStatus) = 'Pending'
		AND d.ApproverNo = @empNo
		AND RTRIM(b.EntityName) = @requestTypeCode
		AND 
		(
			(CONVERT(DATETIME, CONVERT(VARCHAR, d.ActionDate, 12)) BETWEEN @startDate AND @endDate AND @startDate IS NOT NULL AND @endDate IS NOT NULL)
			OR (@startDate IS NULL AND @endDate IS NULL)
		)
	--End of Rev. #1.1

    RETURN @assignedCount;
END

GO

/*	Test:

PARAMETERS:
	@requestTypeCode	VARCHAR(20),
    @empNo				INT,
    @startDate			DATETIME,
    @endDate			DATETIME

	SELECT  kenuser.fnGetAssignedRequestCount('RTYPELEAVE', 10003635, '', '')
	SELECT  kenuser.fnGetAssignedRequestCount('RTYPERECRUIT', 10003632, '', '')
	SELECT  kenuser.fnGetAssignedRequestCount(10003632, '02/01/2026', '02/28/2026', NULL)

*/