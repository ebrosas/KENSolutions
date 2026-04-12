using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Entities.Workflow;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        #region Fields
        private readonly AppDbContext _db;
        #endregion

        #region Constructors                
        public WorkflowRepository(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Workflow Methods       
        public async Task<Result<int>> StartWorkflowAsync(string entityName, long entityId)
        {
            try
            {
                var definition = await _db.WorkflowDefinitions
                .Include(x => x.Steps.OrderBy(s => s.StepOrder))
                .FirstAsync(x => x.EntityName == entityName && x.IsActive);

                var instance = new WorkflowInstance
                {
                    WorkflowDefinitionId = definition.WorkflowDefinitionId,
                    EntityId = entityId,
                    Status = "Running"
                };

                _db.WorkflowInstances.Add(instance);
                await _db.SaveChangesAsync();

                //await InitializeFirstStepAsync(instance, definition);

                await CreateNextStepInstance(instance, definition.Steps.First());
                return Result<int>.SuccessResult(instance.WorkflowInstanceId);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ApproveStepAsync(int stepInstanceId, int userId, string? comments)
        {
            try
            {
                var step = await _db.WorkflowStepInstances
                .Include(x => x.WorkflowInstance)
                .ThenInclude(w => w.Steps)
                .FirstAsync(x => x.StepInstanceId == stepInstanceId);

                if (step == null)
                    throw new InvalidOperationException($"StepInstanceId {stepInstanceId} not found.");

                if (step.Status != "Pending")
                    throw new InvalidOperationException("This step is already processed.");

                if (step.ApproverEmpNo != userId)
                    throw new UnauthorizedAccessException("You are not allowed to approve this step.");

                step.Status = "Approved";
                step.ActionDate = DateTime.UtcNow;
                step.Comments = comments;

                await _db.SaveChangesAsync();

                await AdvanceWorkflow(step.WorkflowInstance);

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RejectStepAsync(int stepInstanceId, int userId, string comments)
        {
            try
            {
                var step = await _db.WorkflowStepInstances
                .Include(x => x.WorkflowInstance)
                .FirstAsync(x => x.StepInstanceId == stepInstanceId);

                step.Status = "Rejected";
                step.ActionDate = DateTime.UtcNow;
                step.Comments = comments;

                step.WorkflowInstance.Status = "Rejected";
                await _db.SaveChangesAsync();

                //await _notify.SendRejectionAsync(step.WorkflowInstance.EntityId);

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
            
        }

        //private async Task TryAdvanceWorkflow(WorkflowInstance instance)
        //{
        //    var steps = await _db.WorkflowStepInstances
        //        .Where(x => x.WorkflowInstanceId == instance.WorkflowInstanceId)
        //        .ToListAsync();

        //    var current = steps.Where(x => x.Status == "Pending").ToList();

        //    if (current.Any())
        //    {
        //        // if parallel group - wait for all
        //        var groupId = current.First().StepDefinition.ParallelGroupId;

        //        if (groupId != null)
        //        {
        //            bool allApproved = current.All(x => x.Status == "Approved");

        //            if (!allApproved) return;
        //        }
        //        else
        //        {
        //            // sequential step, only 1 approver needed
        //            if (current.First().Status != "Approved")
        //                return;
        //        }
        //    }

        //    // evaluate next step based on conditions
        //    await AdvanceNextStep(instance);
        //}

        private async Task CreateNextStepInstance(WorkflowInstance instance, WorkflowStepDefinition stepDef)
        {
            try
            {
                var approver = await _db.WorkflowApprovalRoles.FirstAsync(x => x.ApprovalGroupCode == stepDef.ApprovalRole);

                var step = new WorkflowStepInstance
                {
                    WorkflowInstanceId = instance.WorkflowInstanceId,
                    StepDefinitionId = stepDef.StepDefinitionId,
                    ApproverEmpNo = approver.AssigneeEmpNo,
                    ApproverRole = stepDef.ApprovalRole,
                    Status = "Pending"
                };

                _db.WorkflowStepInstances.Add(step);
                await _db.SaveChangesAsync();

                //await _notify.SendPendingApprovalAsync(step);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task AdvanceWorkflow(WorkflowInstance instance)
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

                var nextStep = definition.Steps
                    .FirstOrDefault(x => x.StepOrder == latest.StepDefinition.StepOrder + 1);

                if (nextStep == null)
                {
                    instance.Status = "Completed";
                    await _db.SaveChangesAsync();

                    //await _notify.SendCompletedAsync(instance.EntityId);
                    return;
                }

                await CreateNextStepInstance(instance, nextStep);
            }
            catch (Exception ex)
            {
                throw;
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
        #endregion
    }
}
