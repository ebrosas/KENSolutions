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
    public class RegularRequestWFConfig : IEntityTypeConfiguration<RegularRequestWF>
    {
        public void Configure(EntityTypeBuilder<RegularRequestWF> builder)
        {
            builder.HasKey(x => x.RegularizationId)
                .HasName("PK_RegularRequestWF_Id");

            builder.Property(a => a.RegularizationId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.Property(a => a.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(e => new { e.EmployeeNo, e.AttendanceDate, e.ROACode, e.ActionCode })
                .HasDatabaseName("IX_RegularRequestWF_CompoKeys")
                .IsUnique();

            // ✅ RELATIONSHIP: RegularRequestWF → FileAttachment
            builder.HasMany(x => x.AttachmentList)
                .WithOne()
                .HasPrincipalKey(e => e.AttachmentId)     // Map to AttachmentId alternate key of RegularRequestWF principal
                .HasForeignKey(c => c.AttachmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
