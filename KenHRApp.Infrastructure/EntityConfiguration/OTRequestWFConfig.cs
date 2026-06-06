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
    public class OTRequestWFConfig : IEntityTypeConfiguration<OTRequestWF>
    {
        public void Configure(EntityTypeBuilder<OTRequestWF> builder)
        {
            builder.HasKey(x => x.ExtratimeId)
                .HasName("PK_OTRequestWF_Id");

            builder.Property(a => a.ExtratimeId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.Property(a => a.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(e => new { e.TS_AutoId, e.EmployeeNo, e.AttendanceDate, e.OTReasonCode })
                .HasDatabaseName("IX_OTRequestWF_CompoKeys")
                .IsUnique();
        }
    }
}
