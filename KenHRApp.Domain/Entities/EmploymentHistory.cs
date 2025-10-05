using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class EmploymentHistory
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(150)"), Comment("Part of composite unique key index")]
        public string CompanyName { get; set; } = null!;

        [Column(TypeName = "varchar(300)")]
        public string? CompanyAddress{ get; set; } = null!;

        [Column(TypeName = "varchar(100)"), Comment("Part of composite unique key index")]
        public string Designation { get; set; } = null!;

        [Column(TypeName = "varchar(100)")]
        public string? Role { get; set; } = null;

        [Column(TypeName = "datetime"), Comment("Part of composite unique key index")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "datetime"), Comment("Part of composite unique key index")]
        public DateTime? ToDate { get; set; }

        [Precision(14, 3)]
        public decimal? LastDrawnSalary { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? SalaryTypeCode { get; set; } = null;

        [NotMapped]
        public string? SalaryType { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? SalaryCurrencyCode { get; set; } = null;

        [NotMapped]
        public string? SalaryCurrency { get; set; } = null;

        [Column(TypeName = "varchar(200)")]
        public string? ReasonOfChange { get; set; } = null;

        [Column(TypeName = "varchar(150)")]
        public string? ReportingManager { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? CompanyWebsite { get; set; } = null;
        #endregion

        #region Reference Navigations 
        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; } 

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
