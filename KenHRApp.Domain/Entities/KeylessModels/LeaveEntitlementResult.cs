using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class LeaveEntitlementResult
    {
        #region Properties
        public int LeaveEntitlementId { get; set; }     
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateTime EffectiveDate { get; set; }
        public double ALEntitlementCount { get; set; }
        public double? SLEntitlementCount { get; set; }
        public string? ALRenewalType { get; set; } = null;
        public string? ALRenewalTypeDesc { get; set; } = null;
        public string? SLRenewalType { get; set; } = null;
        public string? SLRenewalTypeDesc { get; set; } = null;
        public string LeaveUOM { get; set; } = null!;
        public string LeaveUOMDesc { get; set; } = null!;
        public string? SickLeaveUOM { get; set; } = null;
        public string? SickLeaveUOMDesc { get; set; } = null;
        public double? LeaveBalance { get; set; }
        public double? SLBalance { get; set; }
        public double? DILBalance { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LeaveCreatedBy { get; set; }
        public string? CreatedUserID { get; set; } = null;
        public DateTime? LastUpdatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public string? LastUpdatedUserID { get; set; } = null;
        public string? DepartmentCode { get; set; } = null;
        public string? DepartmentName { get; set; } = null;
        #endregion
    }
}
