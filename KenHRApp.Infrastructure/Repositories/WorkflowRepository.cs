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
        private async Task CreateNextStepInstance(WorkflowInstance instance, WorkflowStepDefinition stepDef)
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

            await _notify.SendPendingApprovalAsync(step);
        }

        public async Task<int> StartWorkflowAsync(string entityName, long entityId)
        {
            var definition = await _db.WorkflowDefinitions
                .Include(x => x.Steps)
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

            return instance.WorkflowInstanceId;
        }

        public async Task ApproveStepAsync(int stepInstanceId, int userId, string? comments)
        {
            var step = await _db.WorkflowStepInstances
                .Include(x => x.WorkflowInstance)
                .FirstAsync(x => x.StepInstanceId == stepInstanceId);

            step.Status = "Approved";
            step.ActionDate = DateTime.UtcNow;
            step.Comments = comments;

            await _db.SaveChangesAsync();

            await TryAdvanceWorkflow(step.WorkflowInstance);
        }

        private async Task TryAdvanceWorkflow(WorkflowInstance instance)
        {
            var steps = await _db.WorkflowStepInstances
                .Where(x => x.WorkflowInstanceId == instance.WorkflowInstanceId)
                .ToListAsync();

            var current = steps.Where(x => x.Status == "Pending").ToList();

            if (current.Any())
            {
                // if parallel group - wait for all
                var groupId = current.First().StepDefinition.ParallelGroupId;

                if (groupId != null)
                {
                    bool allApproved = current.All(x => x.Status == "Approved");

                    if (!allApproved) return;
                }
                else
                {
                    // sequential step, only 1 approver needed
                    if (current.First().Status != "Approved")
                        return;
                }
            }

            // evaluate next step based on conditions
            await AdvanceNextStep(instance);
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
        #endregion
    }
}
