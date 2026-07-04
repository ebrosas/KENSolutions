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
    public class PlannedLeaveRequestConfig : IEntityTypeConfiguration<PlannedLeaveRequest>
    {
        public void Configure(EntityTypeBuilder<PlannedLeaveRequest> builder)
        {
            builder.HasKey(x => x.PlannedLeaveId)
                .HasName("PK_PlannedLeaveRequest_Id");

            builder.Property(a => a.PlannedLeaveId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1); // seed = 1, increment = 1

            builder.Property(a => a.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(e => new { e.EmpNo, e.LeaveStartDate, e.LeaveResumeDate })
                .HasDatabaseName("IX_PlannedLeaveRequest_CompoKeys")
                .IsUnique();
        }
    }
}
