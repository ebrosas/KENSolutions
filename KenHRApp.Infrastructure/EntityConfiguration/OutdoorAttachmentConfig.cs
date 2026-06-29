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
    public class OutdoorAttachmentConfig : IEntityTypeConfiguration<OutdoorAttachment>
    {
        public void Configure(EntityTypeBuilder<OutdoorAttachment> builder)
        {
            builder.HasKey(x => x.Id)
                .HasName("PK_OutdoorAttachment_Id");

            builder.Property(x => x.RequestType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.StoredFileName)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
