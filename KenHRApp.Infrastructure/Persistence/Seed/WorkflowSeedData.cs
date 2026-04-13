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
            if (await db.WorkflowDefinitions
                .AnyAsync(x => x.Name == "Leave Workflow"))
                return;

            var parallelGroupId = Guid.NewGuid();

            var workflow = new WorkflowDefinition
            {
                Name = "Leave Workflow",
                EntityName = "LeaveRequisitionWF",
                IsActive = true,
                Steps = new List<WorkflowStepDefinition>
            {
                // ============================================
                // STEP 1: SUPERVISOR
                // ============================================
                new WorkflowStepDefinition
                {
                    StepOrder = 1,
                    StepName = "Supervisor Approval",
                    ApprovalRole = "SUPERVISOR",
                    IsParallelGroup = false
                },

                // ============================================
                // STEP 2: PARALLEL (HR + COST CENTER MANAGER)
                // ============================================
                new WorkflowStepDefinition
                {
                    StepOrder = 2,
                    StepName = "HR Approval",
                    ApprovalRole = "HRADMIN",
                    IsParallelGroup = true,
                    ParallelGroupId = parallelGroupId,
                    RequiresAllParallel = true
                },
                new WorkflowStepDefinition
                {
                    StepOrder = 2,
                    StepName = "Cost Center Manager Approval",
                    ApprovalRole = "CCMANAGER",
                    IsParallelGroup = true,
                    ParallelGroupId = parallelGroupId,
                    RequiresAllParallel = true
                },

                // ============================================
                // STEP 3: GM (CONDITIONAL)
                // ============================================
                new WorkflowStepDefinition
                {
                    StepOrder = 3,
                    StepName = "General Manager Approval",
                    ApprovalRole = "GMOPS",
                    IsParallelGroup = false,

                    Conditions = new List<WorkflowCondition>
                    {
                        new WorkflowCondition
                        {
                            // Annual Leave > 15 days → GM approval
                            Expression = "LeaveType == \"AnnualLeave\" AND LeaveDuration > 15"
                        }
                    }
                }
            }
            };

            db.WorkflowDefinitions.Add(workflow);
            await db.SaveChangesAsync();

            // ============================================
            // ✅ AUTO-CLOSE CONDITION (ATTACHED TO STEP 2)
            // ============================================
            var step2 = await db.WorkflowStepDefinitions
                .Include(x => x.Conditions)
                .FirstAsync(x => x.StepName == "HR Approval");

            step2.Conditions.Add(new WorkflowCondition
            {
                // Sick leave → auto close (no Step 3)
                Expression = "LeaveType == \"Sick\"",
                NextStepDefinitionId = 0 // 🔥 SPECIAL FLAG → AUTO COMPLETE
            });

            await db.SaveChangesAsync();
        }
    }        
}
