using KenHRApp.Domain.Entities.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.EntityConfiguration
{
    public class WorkflowStepInstanceConfig : IEntityTypeConfiguration<WorkflowStepInstance>    
    {
        public void Configure(EntityTypeBuilder<WorkflowStepInstance> builder)
        {
            builder.HasKey(x => x.StepInstanceId)
                .HasName("PK_WorkflowStepInstance_StepInstanceId");

            builder.Property(a => a.StepInstanceId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.Property(a => a.ActionDate)
                .HasDefaultValue(DateTime.Now);

            builder.HasIndex(e => new { e.WorkflowInstanceId, e.StepDefinitionId, e.ApproverEmpNo, e.ApproverRole })
                .HasDatabaseName("IX_WorkflowStepInstance_CompoKeys")
                .IsUnique();
        }
    }
}
