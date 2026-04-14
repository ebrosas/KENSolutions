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
    public class WorkflowConditionConfig : IEntityTypeConfiguration<WorkflowCondition>  
    {
        public void Configure(EntityTypeBuilder<WorkflowCondition> builder)
        {
            builder.HasKey(x => x.ConditionId)
                .HasName("PK_WorkflowCondition_ConditionId");

            builder.Property(a => a.ConditionId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.HasIndex(e => new { e.StepDefinitionId, e.FieldName })
                .HasDatabaseName("IX_WorkflowCondition_CompoKeys")
                .IsUnique(false);

            // ✅ FK → StepDefinition (owner)
            builder.HasOne<WorkflowStepDefinition>()
                .WithMany(x => x.Conditions)
                .HasForeignKey(x => x.StepDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ SELF-REFERENCE: Next Step
            builder.HasOne<WorkflowStepDefinition>()
                .WithMany()
                .HasForeignKey(x => x.NextStepDefinitionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); // ✅ VERY IMPORTANT
        }
    }
}
