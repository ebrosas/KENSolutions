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
    public class WorkflowStepDefinitionConfig : IEntityTypeConfiguration<WorkflowStepDefinition>
    {
        public void Configure(EntityTypeBuilder<WorkflowStepDefinition> builder)
        {
            builder.HasKey(x => x.StepDefinitionId)
                .HasName("PK_WorkflowStepDefinition_StepDefinitionId");

            builder.Property(a => a.StepDefinitionId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.HasIndex(e => new { e.WorkflowDefinitionId, e.StepOrder, e.ApprovalRole })
                .HasDatabaseName("IX_WorkflowStepDefinition_CompoKeys")
                .IsUnique();

            // ✅ RELATIONSHIP: StepDefinition → Conditions
            builder.HasMany(x => x.Conditions)
                .WithOne()
                .HasForeignKey(x => x.StepDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
