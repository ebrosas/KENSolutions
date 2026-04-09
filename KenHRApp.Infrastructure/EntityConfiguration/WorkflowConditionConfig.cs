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
            builder.HasKey(x => x.WorkflowInstanceId)
                .HasName("PK_WorkflowCondition_WorkflowInstanceId");

            builder.Property(a => a.WorkflowInstanceId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.HasIndex(e => new { e.WorkflowDefinitionId, e.EntityId })
                .HasDatabaseName("IX_WorkflowCondition_CompoKeys")
                .IsUnique();
        }
    }
}
