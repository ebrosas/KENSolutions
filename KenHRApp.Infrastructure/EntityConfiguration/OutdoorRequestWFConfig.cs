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
    public class OutdoorRequestWFConfig : IEntityTypeConfiguration<OutdoorRequestWF>
    {
        public void Configure(EntityTypeBuilder<OutdoorRequestWF> builder)
        {
            builder.HasKey(x => x.OutdoorId)
                .HasName("PK_OutdoorRequestWF_Id");

            builder.Property(a => a.OutdoorId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.Property(a => a.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(e => new { e.EmpNo, e.StartDate, e.EndDate, e.ROACode, e.ActionCode })
                .HasDatabaseName("IX_OutdoorRequestWF_CompoKeys")
                .IsUnique();

            // ✅ RELATIONSHIP: OutdoorRequestWF → FileAttachment
            builder.HasMany(x => x.AttachmentList)
                .WithOne()
                .HasPrincipalKey(e => e.AttachmentId)     // Map to AttachmentId alternate key of OutdoorRequestWF principal
                .HasForeignKey(c => c.AttachmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
