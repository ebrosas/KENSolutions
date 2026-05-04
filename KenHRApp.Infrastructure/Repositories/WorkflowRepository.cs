using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Entities.Workflow;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NCalc;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KenHRApp.Infrastructure.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        #region Fields
        private readonly AppDbContext _db;

        private enum WorkflowStatus
        {
            Running,
            Pending,
            InProgress,
            Approved,
            Rejected,
            Cancelled,
            Skipped,
            Completed
        }

        private enum WorkflowRequestType
        {
            NotDefined,
            LeaveRequisition,
            Overtime,
            Regularization,
            TravelRequest,
            ExpenseClaim,
            RecruitmentOffer
        }
        #endregion

        #region Constructors                
        public WorkflowRepository(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Properties
        public List<int> ApproverList { get; set; } = new();
        #endregion

        #region Private Methods
        private async Task<int> InsertApprovalHistoryAsync(
            int stepInstanceId,
            int approverEmpNo,
            string approverUserId,
            bool isApproved,
            bool isHold,
            string approverRemarks,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Input parameters
                var parameters = new[]
                {
                    new SqlParameter("@actionType", SqlDbType.TinyInt) { Value = 1 },
                    new SqlParameter("@stepInstanceId", SqlDbType.Int) { Value = stepInstanceId },
                    new SqlParameter("@approverEmpNo", SqlDbType.Int) { Value = approverEmpNo },
                    new SqlParameter("@approverUserID", SqlDbType.VarChar, 50) { Value = approverUserId ?? (object)DBNull.Value },
                    new SqlParameter("@isApproved", SqlDbType.Bit) { Value = isApproved },
                    new SqlParameter("@isHold", SqlDbType.Bit) { Value = isHold },
                    new SqlParameter("@approverRemarks", SqlDbType.VarChar, 500)
                    {
                        Value = string.IsNullOrWhiteSpace(approverRemarks)
                            ? DBNull.Value
                            : approverRemarks
                    },

                    // Output parameter
                    new SqlParameter("@affectedRow", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    }
                };

                // Execute stored procedure
                await _db.Database.ExecuteSqlRawAsync(
                    @"EXEC kenuser.Pr_ApprovalHistory_CRUD 
                    @actionType,
                    @stepInstanceId,
                    @approverEmpNo,
                    @approverUserID,
                    @isApproved,
                    @isHold,
                    @approverRemarks,
                    @affectedRow OUTPUT",
                    parameters,
                    cancellationToken);

                // Return output value
                return (int)(parameters.Last().Value ?? 0);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Database error occurred while inserting approval history.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error occurred while inserting approval history. {ex.Message}");
            }
        }
        #endregion

        #region Workflow Methods       
        public async Task<Result<List<int>>> StartWorkflowAsync(string entityName, long entityId)
        {
            WorkflowRequestType requestType = WorkflowRequestType.NotDefined;
            object requestEntity = null;

            try
            {
                var definition = await _db.WorkflowDefinitions
                    .Include(x => x.Steps.OrderBy(s => s.StepOrder))
                    .FirstAsync(x => x.EntityName == entityName && x.IsActive);

                var instance = new WorkflowInstance
                {
                    WorkflowDefinitionId = definition.WorkflowDefinitionId,
                    EntityId = entityId,
                    //Status = "Running"
                    Status = WorkflowStatus.Running.ToString()
                };

                _db.WorkflowInstances.Add(instance);
                await _db.SaveChangesAsync();

                #region Get the request entity
                var currentWF = _db.WorkflowDefinitions
                            .Where(w => w.WorkflowDefinitionId == instance.WorkflowDefinitionId)
                            .FirstOrDefault();
                if (currentWF != null)
                {
                    switch (currentWF.EntityName.ToUpper())
                    {
                        case "RTYPELEAVE":
                            requestType = WorkflowRequestType.LeaveRequisition;
                            break;
                    }
                }

                if (requestType == WorkflowRequestType.LeaveRequisition)
                {
                    var entity = await _db.LeaveRequisitionWFs
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.LeaveRequestId == instance.EntityId);
                    if (entity != null)
                    {
                        // Set the request entity
                        requestEntity = entity;
                    }
                }
                #endregion

                await CreateNextStepInstance(instance, definition.Steps.First(), requestEntity);
                return Result<List<int>>.SuccessResult(this.ApproverList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<int>>.Failure($"Workflow Engine Error: {ex.Message}");
            }
        }

        public async Task<Result<List<int>?>> ApproveStepAsync(
            int stepInstanceId, 
            int approverEmpNo, 
            string? approverUserID, 
            string? comments)
        {
            try
            {
                var step = await _db.WorkflowStepInstances
                    .Include(x => x.WorkflowInstance)
                    .ThenInclude(w => w.Steps)
                    .FirstAsync(x => x.StepInstanceId == stepInstanceId);

                if (step == null)
                    throw new InvalidOperationException($"StepInstanceId {stepInstanceId} not found.");

                if (step.Status != WorkflowStatus.Pending.ToString())   
                    throw new InvalidOperationException("This request is already processed.");

                if (step.ApproverEmpNo != approverEmpNo)
                    throw new UnauthorizedAccessException("You are not allowed to approve this request.");

                step.Status = WorkflowStatus.Approved.ToString();
                step.ActionDate = DateTime.UtcNow;
                step.Comments = comments;
                step.ApproverUserID = approverUserID;

                // Save to database
                await _db.SaveChangesAsync();

                // Insert approval history
                await InsertApprovalHistoryAsync(step.StepInstanceId, step.ApproverEmpNo, step.ApproverUserID!, isApproved: true, isHold: false, approverRemarks: comments ?? string.Empty);

                // Set the next approver(s) to the list before advancing the workflow
                List<int> approverList = await AdvanceWorkflow(step.WorkflowInstance);
                if (approverList.Any())
                    return Result<List<int>?>.SuccessResult(approverList);
                else
                    return Result<List<int>?>.SuccessResult(null);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<int>?>.Failure($"Workflow Engine Error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RejectStepAsync(int stepInstanceId, int approverEmpNo, string? approverUserID, string comments)
        {
            try
            {
                var step = await _db.WorkflowStepInstances
                    .Include(x => x.WorkflowInstance)
                    .FirstAsync(x => x.StepInstanceId == stepInstanceId);

                if (step == null)
                    throw new InvalidOperationException($"StepInstanceId {stepInstanceId} not found.");

                if (step.Status != WorkflowStatus.Pending.ToString())
                    throw new InvalidOperationException("This request is already processed.");

                if (step.ApproverEmpNo != approverEmpNo)
                    throw new UnauthorizedAccessException("You are not allowed to reject this request.");

                step.Status = WorkflowStatus.Rejected.ToString();
                step.ActionDate = DateTime.UtcNow;
                step.Comments = comments;
                step.ApproverUserID = approverUserID;

                step.WorkflowInstance.Status = WorkflowStatus.Rejected.ToString();
                await _db.SaveChangesAsync();

                //await _notify.SendRejectionAsync(step.WorkflowInstance.EntityId);

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<bool>.Failure($"Workflow Engine Error: {ex.Message}");
            }
            
        }

        private async Task<int> CreateNextStepInstance(
            WorkflowInstance instance, 
            WorkflowStepDefinition stepDef,
            object? entity)
        {
            int assigneeEmpNo = 0;

            try
            {
                // Clear the approvers collection
                this.ApproverList.Clear();

                var approver = await _db.WorkflowApprovalRoles.FirstAsync(x => x.ApprovalGroupCode == stepDef.ApprovalRole);

                if (approver.GroupType == 1)
                {
                    #region Get the Supervior of the Originator employee
                    if (entity is LeaveRequisitionWF leave)
                    {
                        var employeeInfo = await _db.Employees.FirstAsync(x => x.EmployeeNo == leave.LeaveEmpNo);
                        if (employeeInfo != null && employeeInfo.ReportingManagerCode.HasValue)
                        {
                            assigneeEmpNo = Convert.ToInt32(employeeInfo.ReportingManagerCode);
                        }
                    }
                    #endregion
                }
                else if (approver.GroupType == 2)
                {
                    #region Get the Superintendent of the Originator employee
                    if (entity is LeaveRequisitionWF leave)
                    {
                        var departmentInfo = await _db.DepartmentMasters.FirstAsync(x => x.DepartmentCode == leave.LeaveEmpCostCenter);
                        if (departmentInfo != null && departmentInfo.SuperintendentEmpNo.HasValue)
                        {
                            assigneeEmpNo = Convert.ToInt32(departmentInfo.SuperintendentEmpNo);
                        }
                    }
                    #endregion
                }
                else if (approver.GroupType == 3)
                {
                    #region Get the Cost Center Manager of the Originator employee
                    if (entity is LeaveRequisitionWF leave)
                    {
                        var departmentInfo = await _db.DepartmentMasters.FirstAsync(x => x.DepartmentCode == leave.LeaveEmpCostCenter);
                        if (departmentInfo != null && departmentInfo.ManagerEmpNo.HasValue)
                        {
                            assigneeEmpNo = Convert.ToInt32(departmentInfo.ManagerEmpNo);
                        }
                    }
                    #endregion
                }
                else
                {
                    assigneeEmpNo = approver.AssigneeEmpNo;
                }

                // Store approver to the collection
                this.ApproverList.Add(assigneeEmpNo);

                var step = new WorkflowStepInstance
                {
                    WorkflowInstanceId = instance.WorkflowInstanceId,
                    StepDefinitionId = stepDef.StepDefinitionId,
                    ApproverEmpNo = assigneeEmpNo,
                    ApproverRole = stepDef.ApprovalRole,
                    Status = WorkflowStatus.Pending.ToString()
                };

                _db.WorkflowStepInstances.Add(step);
                await _db.SaveChangesAsync();

                return assigneeEmpNo;

                //await _notify.SendPendingApprovalAsync(step);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task AdvanceWorkflowV1(WorkflowInstance instance)
        {
            try
            {
                var definition = await _db.WorkflowDefinitions
                .Include(x => x.Steps)
                .FirstAsync(x => x.WorkflowDefinitionId == instance.WorkflowDefinitionId);

                var latest = instance.Steps
                    .OrderByDescending(x => x.StepInstanceId)
                    .FirstOrDefault();

                if (latest == null)
                    throw new InvalidOperationException("No workflow steps found.");

                // ✅ Ensure StepDefinition is loaded
                await _db.Entry(latest)
                    .Reference(x => x.StepDefinition)
                    .LoadAsync();

                // Get next step(s)
                var nextStep = definition.Steps
                    .FirstOrDefault(x => x.StepOrder == latest.StepDefinition.StepOrder + 1);

                if (nextStep == null)
                {
                    instance.Status = WorkflowStatus.Completed.ToString();
                    await _db.SaveChangesAsync();

                    //await _notify.SendCompletedAsync(instance.EntityId);
                    return;
                }

                await CreateNextStepInstance(instance, nextStep, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task AdvanceWorkflowV2(WorkflowInstance instance)
        {
            try
            {
                // Load instance with steps + step definitions
                var dbInstance = await _db.WorkflowInstances
                    .Include(x => x.Steps)
                        .ThenInclude(s => s.StepDefinition)
                    .FirstAsync(x => x.WorkflowInstanceId == instance.WorkflowInstanceId);

                var definition = await _db.WorkflowDefinitions
                    .Include(x => x.Steps)
                    .FirstAsync(x => x.WorkflowDefinitionId == dbInstance.WorkflowDefinitionId);

                var latest = dbInstance.Steps
                    .OrderByDescending(x => x.StepInstanceId)
                    .FirstOrDefault();

                if (latest == null)
                    throw new InvalidOperationException("No workflow steps found.");

                var currentStepDef = latest.StepDefinition;

                // 🔥 GET ALL STEP INSTANCES FOR CURRENT STEP (IMPORTANT FOR PARALLEL)
                var currentStepInstances = dbInstance.Steps
                    .Where(x => x.StepDefinitionId == currentStepDef.StepDefinitionId)
                    .ToList();

                // ============================================
                // ✅ PARALLEL APPROVAL CHECK
                // ============================================
                if (currentStepDef.IsParallelGroup)
                {
                    if (currentStepDef.RequiresAllParallel)
                    {
                        // ✔ ALL must approve
                        bool allApproved = currentStepInstances
                            .All(x => x.Status == WorkflowStatus.Approved.ToString());

                        if (!allApproved)
                        {
                            // ⛔ WAIT — do not advance yet
                            return;
                        }
                    }
                    else
                    {
                        // ✔ ANY ONE can approve
                        bool anyApproved = currentStepInstances
                            .Any(x => x.Status == WorkflowStatus.Approved.ToString());

                        if (!anyApproved)
                        {
                            // ⛔ WAIT — no approval yet
                            return;
                        }

                        // OPTIONAL: auto-close remaining pending approvals
                        foreach (var pending in currentStepInstances
                            .Where(x => x.Status == WorkflowStatus.Pending.ToString()))
                        {
                            pending.Status = WorkflowStatus.Skipped.ToString();
                            pending.ActionDate = DateTime.UtcNow;
                        }
                    }
                }
                else
                {
                    // ============================================
                    // ✅ SEQUENTIAL STEP (DEFAULT)
                    // ============================================
                    if (latest.Status != WorkflowStatus.Approved.ToString())
                    {
                        // ⛔ Not approved yet → do not proceed
                        return;
                    }
                }

                // ============================================
                // ✅ MOVE TO NEXT STEP
                // ============================================
                var nextStepOrder = currentStepDef.StepOrder + 1;

                var nextSteps = definition.Steps
                    .Where(x => x.StepOrder == nextStepOrder)
                    .ToList();

                if (nextSteps == null || !nextSteps.Any())
                {
                    // ✅ WORKFLOW COMPLETED
                    dbInstance.Status = WorkflowStatus.Completed.ToString();
                    await _db.SaveChangesAsync();

                    // await _notify.SendCompletedAsync(dbInstance.EntityId);
                    return;
                }

                // ============================================
                // ✅ CREATE NEXT STEP INSTANCES
                // ============================================
                foreach (var nextStep in nextSteps)
                {
                    await CreateNextStepInstance(dbInstance, nextStep, null);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<int>> AdvanceWorkflow(WorkflowInstance instance)
        {
            WorkflowRequestType requestType = WorkflowRequestType.NotDefined;
            List<int> approverList = new();

            try
            {
                object requestEntity = null;
                var dbInstance = await _db.WorkflowInstances
                    .Include(x => x.Steps)
                        .ThenInclude(s => s.StepDefinition)
                            .ThenInclude(sd => sd.Conditions)
                    .FirstAsync(x => x.WorkflowInstanceId == instance.WorkflowInstanceId);

                var definition = await _db.WorkflowDefinitions
                    .Include(x => x.Steps)
                    .FirstAsync(x => x.WorkflowDefinitionId == dbInstance.WorkflowDefinitionId);

                var latest = dbInstance.Steps
                    .OrderByDescending(x => x.StepInstanceId)
                    .FirstOrDefault();

                if (latest == null)
                    throw new InvalidOperationException("No workflow steps found.");

                var currentStepDef = latest.StepDefinition;

                var currentStepInstances = dbInstance.Steps
                    .Where(x => x.StepDefinitionId == currentStepDef.StepDefinitionId)
                    .ToList();

                #region PARALLEL HANDLING (GROUP-BASED)
                if (currentStepDef.IsParallelGroup && currentStepDef.ParallelGroupId != null)
                {
                    var groupSteps = dbInstance.Steps
                        .Where(x => x.StepDefinition.ParallelGroupId == currentStepDef.ParallelGroupId)
                        .ToList();

                    if (currentStepDef.RequiresAllParallel)
                    {
                        bool allApproved = groupSteps.All(x => x.Status == WorkflowStatus.Approved.ToString());

                        if (!allApproved)
                            return approverList;
                    }
                    else
                    {
                        bool anyApproved = groupSteps.Any(x => x.Status == WorkflowStatus.Approved.ToString());

                        if (!anyApproved)
                            return approverList;

                        // Auto-skip remaining
                        foreach (var pending in groupSteps.Where(x => x.Status == WorkflowStatus.Pending.ToString()))
                        {
                            pending.Status = WorkflowStatus.Skipped.ToString();
                            pending.ActionDate = DateTime.UtcNow;
                        }
                    }
                }
                else
                {
                    // Sequential
                    if (latest.Status != WorkflowStatus.Approved.ToString())
                        return approverList;
                }
                #endregion

                #region Get the request entity
                var currentWF = _db.WorkflowDefinitions
                                   .Where(w => w.WorkflowDefinitionId == dbInstance.WorkflowDefinitionId)
                                   .FirstOrDefault();
                if (currentWF != null)
                {
                    switch (currentWF.EntityName.ToUpper())
                    {
                        case "RTYPELEAVE":
                            requestType = WorkflowRequestType.LeaveRequisition;
                            break;
                    }
                }

                if (requestType == WorkflowRequestType.LeaveRequisition)
                {
                    var entity = await _db.LeaveRequisitionWFs
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.LeaveRequestId == dbInstance.EntityId);
                    if (entity != null)
                    {
                        // Set the request entity
                        requestEntity = entity;
                    }
                }
                #endregion

                #region MULTI-CONDITIONAL ROUTING
                var nextStepIds = new List<int>();
                foreach (var condition in currentStepDef.Conditions)
                {
                    if (requestEntity != null)
                    {
                        if (EvaluateCondition(condition, requestEntity))
                        {
                            nextStepIds.Add(condition.NextStepDefinitionId ?? 0);
                        }
                    }
                }
                #endregion

                // Fallback to StepOrder if no condition matched
                if (!nextStepIds.Any())
                {
                    var nextOrder = currentStepDef.StepOrder + 1;

                    nextStepIds = definition.Steps
                        .Where(x => x.StepOrder == nextOrder)
                        .Select(x => x.StepDefinitionId)
                        .ToList();
                }

                if (nextStepIds == null || !nextStepIds.Any())
                {
                    dbInstance.Status = WorkflowStatus.Completed.ToString();
                    await _db.SaveChangesAsync();

                    // await _notify.SendCompletedAsync(dbInstance.EntityId);
                    return approverList;
                }

                // ============================================
                // ✅ LOAD NEXT STEPS
                // ============================================
                var nextSteps = definition.Steps
                    .Where(x => nextStepIds.Contains(x.StepDefinitionId))
                    .ToList();

                // ============================================
                // ✅ CREATE NEXT STEP INSTANCES
                // ============================================
                int newApproverNo = 0;
                foreach (var nextStep in nextSteps)
                {
                    newApproverNo = await CreateNextStepInstance(dbInstance, nextStep, requestEntity);
                    if (newApproverNo > 0 && !approverList.Contains(newApproverNo) )
                    {
                        approverList.Add(newApproverNo);
                    }
                }

                return approverList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool EvaluateConditionOld(WorkflowCondition condition, long entityId, WorkflowRequestType requestType)
        {
            try
            {
                //dynamic entity = null;

                //if (requestType == WorkflowRequestType.LeaveRequisition)
                //{
                    
                //}

                var entity = _db.LeaveRequisitionWFs
                        .AsNoTracking()
                        .FirstOrDefault(x => x.LeaveRequestId == entityId);

                if (entity == null)
                    return false;

                // Case-insensitive property lookup
                var property = entity.GetType()
                    .GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(condition.FieldName, StringComparison.OrdinalIgnoreCase));

                if (property == null)
                    throw new InvalidOperationException($"Field '{condition.FieldName}' not found.");

                var actualValue = property.GetValue(entity);

                if (actualValue == null)
                    return false;

                string actualString = actualValue.ToString() ?? string.Empty;
                string expectedString = condition.CompareValue ?? string.Empty;

                // ============================================
                // ✅ NUMERIC PARSING (FIXED)
                // ============================================
                bool actualIsNumber = double.TryParse(actualString, out double actualNumber);
                bool expectedIsNumber = double.TryParse(expectedString, out double expectedNumber);

                bool isNumeric = actualIsNumber && expectedIsNumber;

                // ============================================
                // ✅ DATE PARSING
                // ============================================
                bool actualIsDate = DateTime.TryParse(actualString, out DateTime actualDate);
                bool expectedIsDate = DateTime.TryParse(expectedString, out DateTime expectedDate);

                bool isDate = actualIsDate && expectedIsDate;

                var op = condition.Operator.ToLower();

                // ============================================
                // ✅ DATE COMPARISON
                // ============================================
                if (isDate)
                {
                    return op switch
                    {
                        ">" => actualDate > expectedDate,
                        "<" => actualDate < expectedDate,
                        ">=" => actualDate >= expectedDate,
                        "<=" => actualDate <= expectedDate,
                        "=" => actualDate == expectedDate,
                        "!=" => actualDate != expectedDate,
                        _ => false
                    };
                }

                // ============================================
                // ✅ NUMERIC COMPARISON
                // ============================================
                if (isNumeric)
                {
                    return op switch
                    {
                        ">" => actualNumber > expectedNumber,
                        "<" => actualNumber < expectedNumber,
                        ">=" => actualNumber >= expectedNumber,
                        "<=" => actualNumber <= expectedNumber,
                        "=" => actualNumber == expectedNumber,
                        "!=" => actualNumber != expectedNumber,
                        _ => false
                    };
                }

                // ============================================
                // ✅ STRING COMPARISON
                // ============================================
                return op switch
                {
                    "=" => string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),

                    "!=" => !string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),

                    "contains" => actualString.Contains(expectedString, StringComparison.OrdinalIgnoreCase),

                    "startswith" => actualString.StartsWith(expectedString, StringComparison.OrdinalIgnoreCase),

                    "endswith" => actualString.EndsWith(expectedString, StringComparison.OrdinalIgnoreCase),

                    _ => throw new NotSupportedException($"Operator '{condition.Operator}' is not supported.")
                };
            }
            catch (Exception)
            {
                // TODO: log error in production
                return false;
            }
        }        

        private bool EvaluateConditionHasBug(WorkflowCondition condition, object entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(condition.Expression))
                    return false;

                // ============================================
                // ✅ NORMALIZE EXPRESSION (FIX HERE)
                // ============================================
                var normalizedExpression = NormalizeExpression(condition.Expression);

                if (string.IsNullOrWhiteSpace(normalizedExpression))
                    return false;

                var exp = new Expression(normalizedExpression, EvaluateOptions.IgnoreCase);
                //var exp = new Expression(condition.Expression, EvaluateOptions.IgnoreCase);

                // ============================================
                // ✅ MAP ENTITY PROPERTIES TO EXPRESSION
                // ============================================
                var properties = entity.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(entity);

                    exp.Parameters[prop.Name] = value ?? DBNull.Value;
                }

                // ============================================
                // ✅ OPTIONAL: CUSTOM FUNCTIONS
                // ============================================
                exp.EvaluateFunction += (name, args) =>
                {
                    if (name.Equals("IsNullOrEmpty", StringComparison.OrdinalIgnoreCase))
                    {
                        var val = args.Parameters[0].Evaluate();
                        args.Result = val == null || string.IsNullOrEmpty(val.ToString());
                    }
                };

                // ============================================
                // ✅ EXECUTE EXPRESSION
                // ============================================
                var result = exp.Evaluate();

                return result is bool b && b;
            }
            catch (Exception ex)
            {
                // TODO: log error
                //_logger.LogError(ex, "Expression evaluation failed");
                return false;
            }
        }

        private bool EvaluateConditionNew(WorkflowCondition condition, object entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(condition.Expression))
                    return false;

                var normalizedExpression = NormalizeExpression(condition.Expression);

                if (string.IsNullOrWhiteSpace(normalizedExpression))
                    return false;

                var exp = new Expression(normalizedExpression, EvaluateOptions.IgnoreCase);

                var properties = entity.GetType().GetProperties();

                // ============================================
                // ✅ MAP PROPERTIES
                // ============================================
                //foreach (var prop in properties)
                //{
                //    var value = prop.GetValue(entity);
                //    exp.Parameters[prop.Name] = value ?? DBNull.Value;
                //}

                // ============================================
                // ✅ HANDLE MISSING PARAMETERS (CRITICAL FIX)
                // ============================================
                exp.EvaluateParameter += (name, args) =>
                {
                    //var prop = properties
                    //    .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                    var propertyDict = properties.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
                    if (propertyDict.TryGetValue(name, out var prop))
                    {
                        if (prop != null)
                        {
                            var val = prop.GetValue(entity);
                            args.Result = val ?? DBNull.Value;
                        }
                        else
                        {
                            // 🔥 Prevent crash — treat missing fields as NULL
                            args.Result = DBNull.Value;
                        }
                    }

                    //if (prop != null)
                    //{
                    //    var val = prop.GetValue(entity);
                    //    args.Result = val ?? DBNull.Value;
                    //}
                    //else
                    //{
                    //     🔥 Prevent crash — treat missing fields as NULL
                    //    args.Result = DBNull.Value;
                    //}
                };

                // ============================================
                // ✅ CUSTOM FUNCTIONS
                // ============================================
                exp.EvaluateFunction += (name, args) =>
                {
                    if (name.Equals("IsNullOrEmpty", StringComparison.OrdinalIgnoreCase))
                    {
                        var val = args.Parameters[0].Evaluate();
                        args.Result = val == null || string.IsNullOrEmpty(val?.ToString());
                    }
                };

                // ============================================
                // ✅ EXECUTE
                // ============================================
                var result = exp.Evaluate();

                return result is bool b && b;
            }
            catch (Exception ex)
            {
                // TODO: log properly
                //_logger.LogError(ex, $"Expression failed: {condition.Expression}");
                return false;
            }
        }

        private bool EvaluateConditionClassic(WorkflowCondition condition, object entity)
        {
            try
            {
                if (entity == null)
                    return false;

                // Case-insensitive property lookup
                var property = entity.GetType()
                    .GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(condition.FieldName, StringComparison.OrdinalIgnoreCase));

                if (property == null)
                    throw new InvalidOperationException($"Field '{condition.FieldName}' not found.");

                var actualValue = property.GetValue(entity);

                if (actualValue == null)
                    return false;

                string actualString = actualValue.ToString() ?? string.Empty;
                string expectedString = condition.CompareValue ?? string.Empty;

                // ============================================
                // ✅ NUMERIC PARSING (FIXED)
                // ============================================
                bool actualIsNumber = double.TryParse(actualString, out double actualNumber);
                bool expectedIsNumber = double.TryParse(expectedString, out double expectedNumber);

                bool isNumeric = actualIsNumber && expectedIsNumber;

                // ============================================
                // ✅ DATE PARSING
                // ============================================
                bool actualIsDate = DateTime.TryParse(actualString, out DateTime actualDate);
                bool expectedIsDate = DateTime.TryParse(expectedString, out DateTime expectedDate);

                bool isDate = actualIsDate && expectedIsDate;

                var op = condition.Operator.ToLower();

                // ============================================
                // ✅ DATE COMPARISON
                // ============================================
                if (isDate)
                {
                    return op switch
                    {
                        ">" => actualDate > expectedDate,
                        "<" => actualDate < expectedDate,
                        ">=" => actualDate >= expectedDate,
                        "<=" => actualDate <= expectedDate,
                        "=" => actualDate == expectedDate,
                        "!=" => actualDate != expectedDate,
                        _ => false
                    };
                }

                // ============================================
                // ✅ NUMERIC COMPARISON
                // ============================================
                if (isNumeric)
                {
                    return op switch
                    {
                        ">" => actualNumber > expectedNumber,
                        "<" => actualNumber < expectedNumber,
                        ">=" => actualNumber >= expectedNumber,
                        "<=" => actualNumber <= expectedNumber,
                        "=" => actualNumber == expectedNumber,
                        "!=" => actualNumber != expectedNumber,
                        _ => false
                    };
                }

                // ============================================
                // ✅ STRING COMPARISON
                // ============================================
                return op switch
                {
                    "=" => string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),

                    "!=" => !string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),

                    "contains" => actualString.Contains(expectedString, StringComparison.OrdinalIgnoreCase),

                    "startswith" => actualString.StartsWith(expectedString, StringComparison.OrdinalIgnoreCase),

                    "endswith" => actualString.EndsWith(expectedString, StringComparison.OrdinalIgnoreCase),

                    _ => throw new NotSupportedException($"Operator '{condition.Operator}' is not supported.")
                };
            }
            catch (Exception)
            {
                // TODO: log error in production
                return false;
            }
        }

        private bool EvaluateCondition(WorkflowCondition condition, object entity)
        {
            try
            {
                if (entity == null)
                    return false;

                // ============================================
                // ✅ USE EXPRESSION FIRST (NEW ENGINE)
                // ============================================
                if (!string.IsNullOrWhiteSpace(condition.Expression))
                {
                    var normalized = NormalizeExpression(condition.Expression);

                    if (string.IsNullOrWhiteSpace(normalized))
                        return false;

                    // ============================================
                    // ✅ CAST TO STRONG TYPE 
                    // ============================================
                    if (entity is LeaveRequisitionWF leave)
                    {
                        var queryable = new List<LeaveRequisitionWF> { leave }.AsQueryable();

                        return queryable.Any(normalized);
                    }

                    //throw new InvalidOperationException("Unsupported entity type.");

                    //// Convert single object → IQueryable
                    //var queryable = new[] { entity }.AsQueryable();

                    //// Execute dynamic LINQ
                    //return queryable.Any(normalized);
                }

                // ============================================
                // ✅ FALLBACK TO FIELD-BASED (LEGACY SUPPORT)
                // ============================================
                var property = entity.GetType()
                    .GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(condition.FieldName, StringComparison.OrdinalIgnoreCase));

                if (property == null)
                    throw new InvalidOperationException($"Field '{condition.FieldName}' not found.");

                var actualValue = property.GetValue(entity);

                if (actualValue == null)
                    return false;

                string actualString = actualValue.ToString() ?? string.Empty;
                string expectedString = condition.CompareValue ?? string.Empty;

                var op = condition.Operator.ToLower();

                return op switch
                {
                    "=" => string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),
                    "!=" => !string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),
                    "contains" => actualString.Contains(expectedString, StringComparison.OrdinalIgnoreCase),
                    "startswith" => actualString.StartsWith(expectedString, StringComparison.OrdinalIgnoreCase),
                    "endswith" => actualString.EndsWith(expectedString, StringComparison.OrdinalIgnoreCase),
                    _ => throw new NotSupportedException($"Operator '{condition.Operator}' is not supported.")
                };
            }
            catch (Exception ex)
            {
                // TODO: log error properly
                return false;
            }
        }

        private string NormalizeExpressionClassic(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return expression;

            // Replace logical operators safely (case-insensitive)
            expression = Regex.Replace(expression, @"\bAND\b", "&&", RegexOptions.IgnoreCase);
            expression = Regex.Replace(expression, @"\bOR\b", "||", RegexOptions.IgnoreCase);

            return expression;
        }

        private string NormalizeExpression(string expression)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(expression))
                    return expression;

                // Convert SQL-style to C# style
                expression = Regex.Replace(expression, @"\bAND\b", "&&", RegexOptions.IgnoreCase);
                expression = Regex.Replace(expression, @"\bOR\b", "||", RegexOptions.IgnoreCase);

                // Convert "=" to "==" safely
                expression = Regex.Replace(expression, @"(?<![=!<>])=(?!=)", "==");

                // Convert "<>" to "!="
                expression = expression.Replace("<>", "!=");

                return expression;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Database Access methods     
        public async Task<Result<List<RequestTypeResult>>> GetPendingRequestAsync(
            int empNo,
            string? requestType,
            byte? periodType,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<RequestTypeResult> requestTypeList = new();

            try
            {
                var model = await _db.Set<RequestTypeResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetPendingRequest @empNo = {0}, @requestType = {1}, @periodType = {2}, @startDate = {3}, @endDate = {4}",
                    empNo,
                    requestType!,
                    periodType!,
                    startDate!,
                    endDate!)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    requestTypeList = model.Select(e => new RequestTypeResult
                    {
                        RequestTypeCode = e.RequestTypeCode,
                        RequestTypeName = e.RequestTypeName,
                        RequestTypeDesc = e.RequestTypeDesc,
                        IconName = e.IconName,
                        AssignedCount = e.AssignedCount
                    }).ToList();
                }

                return Result<List<RequestTypeResult>>.SuccessResult(requestTypeList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<RequestTypeResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<Employee?>> GetEmployeeInfoAsync(
            int? empNo, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (empNo == null || empNo == 0)
                    throw new ArgumentException(
                        "Employee No. must be provided.",
                        nameof(empNo));

                Employee? employee = await _db.Employees
                    .FirstOrDefaultAsync(e =>
                        e.EmployeeNo == empNo,
                        cancellationToken);

                return Result<Employee?>.SuccessResult(employee);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<Employee?>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<Employee?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<WorkflowDetailResult>>> GetWorkflowStatusAsync(
            string workflowTypeCode,
            long requestNo)
        {
            List<WorkflowDetailResult> requestTypeList = new();

            try
            {
                var model = await _db.Set<WorkflowDetailResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetWorkflowStatus @workflowTypeCode = {0}, @requestNo = {1}",
                    workflowTypeCode,
                    requestNo)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    requestTypeList = model.Select(e => new WorkflowDetailResult
                    {
                        RequestNo = e.RequestNo,
                        WorkflowType = e.WorkflowType,
                        WorkflowStatus = e.WorkflowStatus,
                        ActivityID = e.ActivityID,
                        ActivityName = e.ActivityName,
                        ActivityOrder = e.ActivityOrder,
                        ActivityStatus = e.ActivityStatus,
                        ApproverNo = e.ApproverNo,
                        ApproverName = e.ApproverName
                    }).ToList();
                }

                return Result<List<WorkflowDetailResult>>.SuccessResult(requestTypeList);
            }
            catch (Exception ex)
            {
                return Result<List<WorkflowDetailResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<ApprovalRequestResult>>> GetApprovalRequestAsync(
            byte searchType,
            int? empNo,
            string? requestType)
        {
            List<ApprovalRequestResult> requestTypeList = new();

            try
            {
                var model = await _db.Set<ApprovalRequestResult>()
                    //.FromSqlRaw("EXEC kenuser.Pr_GetDashboardPendingRequest @empNo = {0}, @requestType = {1}",
                    .FromSqlRaw("EXEC kenuser.Pr_GetDashboardStatistics @searchType = {0}, @empNo = {1}, @requestType = {2}",
                    searchType,
                    empNo!,
                    requestType!)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    requestTypeList = model.Select(e => new ApprovalRequestResult
                    {
                        RequestNo = e.RequestNo,
                        RequestTypeCode = e.RequestTypeCode,
                        RequestTypeDesc = e.RequestTypeDesc,
                        AppliedDate = e.AppliedDate,
                        RequestedByNo = e.RequestedByNo,
                        RequestedByName = e.RequestedByName,
                        Detail = e.Detail,
                        ApprovalRole = e.ApprovalRole,
                        CurrentStatus = e.CurrentStatus,
                        ApproverNo = e.ApproverNo,
                        ApproverName = e.ApproverName,
                        PendingDays = e.PendingDays,
                        StepInstanceId = e.StepInstanceId,
                        CreatedByEmpNo = e.CreatedByEmpNo
                    }).ToList();
                }

                return Result<List<ApprovalRequestResult>>.SuccessResult(requestTypeList);
            }
            catch (Exception ex)
            {
                return Result<List<ApprovalRequestResult>>.Failure($"Database error: {ex.Message}");
            }
        }                
        #endregion
    }
}
