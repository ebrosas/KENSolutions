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
    public class LeaveAttachmentConfig : IEntityTypeConfiguration<LeaveAttachment>
    {
        public void Configure(EntityTypeBuilder<LeaveAttachment> builder)
        {
            builder.HasKey(x => x.Id)
                .HasName("PK_LeaveAttachment_Id");

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.StoredFileName)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(50);

            //builder.HasIndex(e => new { e.LeaveRequestId, e.FileName, e.ContentType })
            //    .HasDatabaseName("IX_LeaveAttachment_CompoKeys")
            //    .IsUnique();
        }
    }
}
