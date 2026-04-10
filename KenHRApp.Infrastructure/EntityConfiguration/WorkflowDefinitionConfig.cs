using KenHRApp.Domain.Entities;
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
    public class WorkflowDefinitionConfig : IEntityTypeConfiguration<WorkflowDefinition>
    {
        public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
        {
            builder.HasKey(x => x.WorkflowDefinitionId)
                .HasName("PK_WorkflowDefinition_WorkflowDefinitionId");

            builder.Property(a => a.WorkflowDefinitionId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.HasIndex(e => new { e.EntityName })
                .HasDatabaseName("IX_WorkflowDefinition_UniqueKey")
                .IsUnique(false);

            // ✅ RELATIONSHIP: WorkflowDefinition → Steps
            builder.HasMany(x => x.Steps)
                .WithOne()
                .HasForeignKey(x => x.WorkflowDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
