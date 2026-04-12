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
                EntityName = "LeaveRequisitionWF",
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
    }
}
