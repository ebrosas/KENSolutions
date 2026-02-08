using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class LeaveEntitlement
    {
        #region Properties
        public int LeaveEntitlementId { get; set; }     // Identity column  

        [Comment("Part of composite unique key index")]
        public double LeaveEntitlemnt { get; set; }

        public double SickLeaveEntitlemnt { get; set; }

        [Column(TypeName = "char(1)"), Comment("Part of composite unique key index")]
        public char LeaveUOM { get; set; }

        public double? LeaveBalance { get; set; }
        public double? SLBalance { get; set; }
        public double? DILBalance { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public int? LeaveCreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUserID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedUserID { get; set; } = null;
        #endregion

        #region Reference Navigation to Employee   
        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; }

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
