using KenHRApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.EntityConfiguration
{
    public class RequestApprovalConfig : IEntityTypeConfiguration<RequestApproval>
    {
        public void Configure(EntityTypeBuilder<RequestApproval> builder)
        {
            builder.HasKey(x => x.ApprovalId)
                .HasName("PK_RequestApproval_ApprovalID");

            builder.Property(a => a.ApprovalId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.Property(a => a.CreatedDate)
                .HasDefaultValue(DateTime.Now);

            builder.HasIndex(e => new { e.RequestTypeCode, e.RequisitionNo })
                .HasDatabaseName("IX_RequestApproval_CompoKeys")
                .IsUnique(false);
        }
    }
}
