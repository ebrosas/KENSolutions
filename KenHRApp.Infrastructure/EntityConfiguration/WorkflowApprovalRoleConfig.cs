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
    public class WorkflowApprovalRoleConfig : IEntityTypeConfiguration<WorkflowApprovalRole>
    {
        public void Configure(EntityTypeBuilder<WorkflowApprovalRole> builder)
        {
            builder.HasKey(x => x.ApprovalRoleId)
                .HasName("PK_WorkflowApprovalRole_ApprovalRoleId");

            builder.Property(a => a.ApprovalRoleId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.Property(a => a.CreatedDate)
                .HasDefaultValue(DateTime.Now);

            builder.HasIndex(e => new { e.ApprovalGroupCode })
                .HasDatabaseName("IX_WorkflowApprovalRole_CompoKeys")
                .IsUnique();
        }
    }
}
