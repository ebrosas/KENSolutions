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
    public class WorkflowInstanceConfig : IEntityTypeConfiguration<WorkflowInstance>
    {
        public void Configure(EntityTypeBuilder<WorkflowInstance> builder)
        {
            builder.HasKey(x => x.WorkflowInstanceId)
                .HasName("PK_WorkflowInstance_WorkflowInstanceId");

            builder.Property(a => a.WorkflowInstanceId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.HasIndex(e => new { e.WorkflowDefinitionId, e.EntityId, e.Status })
                .HasDatabaseName("IX_WorkflowInstance_CompoKeys")
                .IsUnique();
        }
    }
}
