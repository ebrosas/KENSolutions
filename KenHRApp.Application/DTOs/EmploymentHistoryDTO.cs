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
    public class EmploymentHistoryDTO
    {
        #region Properties
        public int AutoId { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(150, ErrorMessage = "Company Name can't be more than 150 characters.")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = null!;

        public string? CompanyAddress { get; set; } = null;

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(100, ErrorMessage = "Designation can't be more than 100 characters.")]
        [Display(Name = "Designation")]
        public string Designation { get; set; } = null!;

        public string? Role { get; set; } = null;

        [Display(Name = "Role")]
        public string? RoleDesc { get; set; } = null;

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? LastDrawnSalary { get; set; } = null;

        public string? SalaryTypeCode { get; set; } = null;

        [Display(Name = "Salary Type")]
        public string? SalaryType { get; set; } = null;

        public string? SalaryCurrencyCode { get; set; } = null;

        [Display(Name = "Salary Type")]
        public string? SalaryCurrency { get; set; } = null;

        public string? ReasonOfChange { get; set; } = null;
        public string? ReportingManager { get; set; } = null;
        public string? CompanyWebsite { get; set; } = null;
        #endregion

        #region Reference Navigations 
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
