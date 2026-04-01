using KenHRApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class LeaveEntitlementDTO
    {
        #region Properties
        public int LeaveEntitlementId { get; set; }     // Identity column  

        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int EmployeeNo { get; set; }

        [Required(ErrorMessage = "Employee Name is required")]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; } = null!;

        [Display(Name = "Effective Date")]
        [DataType(DataType.Date)]
        public DateTime? EffectiveDate { get; set; }

        [Required(ErrorMessage = "Leave Entitlement is required")]
        [Display(Name = "Leave Entitlement")]
        public double ALEntitlementCount { get; set; }

        [Display(Name = "Sick Leave Entitlement")]
        public double? SLEntitlementCount { get; set; }

        public string? ALRenewalType { get; set; } = null;

        [Display(Name = "Renewal Type")]
        [StringLength(150, ErrorMessage = "Renewal Type can't be more than 150 characters.")]
        public string? ALRenewalTypeDesc { get; set; } = null;

        public string? SLRenewalType { get; set; } = null;

        [Display(Name = "SL Renewal Type")]
        [StringLength(150, ErrorMessage = "Sick Leave Renewal Type can't be more than 150 characters.")]
        public string? SLRenewalTypeDesc { get; set; } = null;

        public string LeaveUOM { get; set; } = null!;

        [Required(ErrorMessage = "Leave Unit of Measure is required")]
        [Display(Name = "Leave Unit of Measure")]
        [StringLength(150, ErrorMessage = "Leave Unit of Measure can't be more than 150 characters.")]
        public string LeaveUOMDesc { get; set; } = null!;

        public string? SickLeaveUOM { get; set; } = null;

        [Display(Name = "Sick Leave Unit of Measure")]
        [StringLength(150, ErrorMessage = "Sick Leave Unit of Measure can't be more than 150 characters.")]
        public string? SickLeaveUOMDesc { get; set; } = null;

        [Display(Name = "Leave Balance")]
        public double? LeaveBalance { get; set; }

        [Display(Name = "Sick Leave Balance")]
        public double? SLBalance { get; set; }

        [Display(Name = "DIL Balance")]
        public double? DILBalance { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        public int? LeaveCreatedBy { get; set; }

        public string? CreatedUserID { get; set; } = null;

        [Display(Name = "Last Update Date")]
        [DataType(DataType.Date)]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        public string? LastUpdatedUserID { get; set; } = null;
        #endregion
    }
}
