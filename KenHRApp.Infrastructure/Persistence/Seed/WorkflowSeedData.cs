using KenHRApp.Domain.Entities.Workflow;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Persistence.Seed
{
    public static class WorkflowSeedData
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            //if (await db.WorkflowDefinitions.AnyAsync(x => x.Name == "Leave Workflow"))
            //    return;

            if (await db.WorkflowDefinitions
                .Include(x => x.Steps)
                .AnyAsync(x => x.Name == "Leave Workflow"))
                return;

            var workflow = new WorkflowDefinition
            {
                Name = "Leave Workflow",
                EntityName = "RTYPELEAVE",
                IsActive = true,
                Steps = new List<WorkflowStepDefinition>
            {
                new WorkflowStepDefinition
                {
                    StepOrder = 1,
                    StepName = "Supervisor Approval",
                    ApprovalRole = "SUPERVISOR",
                    IsParallelGroup = false
                },
                new WorkflowStepDefinition
                {
                    StepOrder = 2,
                    StepName = "Department Manager Approval",
                    ApprovalRole = "CCMANAGER",
                    IsParallelGroup = false
                }
            }
            };

            db.WorkflowDefinitions.Add(workflow);
            await db.SaveChangesAsync();
        }

        public static async Task SeedLeaveWorkflowAsync(AppDbContext db)
        {
            /*  Workflow Outline:
              
                Step 1 → Approval by Supervisor

                Step 2 → Parallel Group Approval
                    - Head of HR
                    - Cost Center Manager

                Step 3 → Conditional Routing
                    - If Sick Leave → Auto Close
                    - If Annual Leave + Duration > 15 → GM Operations 
             
             */
            try
            {
                if (await db.WorkflowDefinitions.AnyAsync(x => x.Name == "Leave Workflow"))
                    return;

                var parallelGroupId = Guid.NewGuid();

                var workflow = new WorkflowDefinition
                {
                    Name = "Leave Workflow",
                    EntityName = "RTYPELEAVE",      // Refers to the Request Type Code
                    IsActive = true
                };

                db.WorkflowDefinitions.Add(workflow);
                await db.SaveChangesAsync();

                // ============================================
                // STEP 1: SUPERVISOR
                // ============================================
                var step1 = new WorkflowStepDefinition
                {
                    WorkflowDefinitionId = workflow.WorkflowDefinitionId,
                    StepOrder = 1,
                    StepName = "Supervisor Approval",
                    ApprovalRole = "SUPERVISOR",
                    IsParallelGroup = false
                };

                // ============================================
                // STEP 2: PARALLEL (HR + CC MANAGER)
                // ============================================
                var step2HR = new WorkflowStepDefinition
                {
                    WorkflowDefinitionId = workflow.WorkflowDefinitionId,
                    StepOrder = 2,
                    StepName = "HR Approval",
                    ApprovalRole = "HRHEAD",
                    IsParallelGroup = true,
                    ParallelGroupId = parallelGroupId,
                    RequiresAllParallel = true
                };

                var step2CC = new WorkflowStepDefinition
                {
                    WorkflowDefinitionId = workflow.WorkflowDefinitionId,
                    StepOrder = 2,
                    StepName = "Cost Center Manager Approval",
                    ApprovalRole = "CCMANAGER",
                    IsParallelGroup = true,
                    ParallelGroupId = parallelGroupId,
                    RequiresAllParallel = true
                };

                // ============================================
                // STEP 3: GM
                // ============================================
                var step3GM = new WorkflowStepDefinition
                {
                    WorkflowDefinitionId = workflow.WorkflowDefinitionId,
                    StepOrder = 3,
                    StepName = "General Manager Approval",
                    ApprovalRole = "GMOPS",
                    IsParallelGroup = false
                };

                db.WorkflowStepDefinitions.AddRange(step1, step2HR, step2CC, step3GM);
                await db.SaveChangesAsync();

                // ============================================
                // ✅ CONDITIONS
                // ============================================

                var conditions = new List<WorkflowCondition>
                {
                    // --------------------------------------------
                    // ✅ CONDITION 1: Sick → Auto Close
                    // --------------------------------------------
                    new WorkflowCondition
                    {
                        //StepDefinitionId = step2HR.StepDefinitionId,      // attach to Step 2 - HR Approval
                        StepDefinitionId = step2CC.StepDefinitionId,        // attach to Step 2 - Cost Center Manager Approval (Notes: Attach to the last step in the parallel group to ensure both HR and CC Manager approvals are completed before evaluating this condition)
                        FieldName = "LeaveType",
                        Operator = "=",
                        CompareValue = "SL",
                        Expression = "LeaveType == \"SL\"",
                        NextStepDefinitionId = null,
                        IsTerminal = true     // 🔥 AUTO CLOSE
                    },

                    // --------------------------------------------
                    // ✅ CONDITION 2: AnnualLeave > 15 → GM
                    // --------------------------------------------
                    new WorkflowCondition
                    {
                        //StepDefinitionId = step2HR.StepDefinitionId,      // attach to Step 2 - HR Approval
                        StepDefinitionId = step2CC.StepDefinitionId,        // attach to Step 2 - Cost Center Manager Approval (Notes: Attach to the last step in the parallel group to ensure both HR and CC Manager approvals are completed before evaluating this condition)
                        FieldName = "LeaveDuration",
                        Operator = ">",
                        CompareValue = "15",
                        Expression = "LeaveType == \"AL\" AND LeaveDuration > 15",
                        NextStepDefinitionId = step3GM.StepDefinitionId
                    }
                };

                db.WorkflowConditions.AddRange(conditions);

                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }        
}
