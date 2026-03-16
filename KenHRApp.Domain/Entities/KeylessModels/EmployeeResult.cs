using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class EmployeeResult
    {
        #region Properties
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public string? FirstName { get; set; } = null;
        public string? MiddleName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Gender { get; set; } = null;
        public DateTime? HireDate { get; set; } = null;
        public DateTime? DOB { get; set; } = null;
        public string? ReportingManagerCode { get; set; } = null;
        public string? ReportingManager { get; set; } = null;
        public string? DepartmentCode { get; set; } = null;
        public string? DepartmentName { get; set; } = null;
        public string JobTitle { get; set; } = null!;
        public string? EmpEmail { get; set; } = null;
        public double? DILBalance { get; set; }
        public double? LeaveBalance { get; set; }
        public double? SLBalance { get; set; }
        #endregion
    }
}
