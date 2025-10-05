using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class EmployeeMaster
    {
        #region Properties
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Gender { get; set; } = null;
        public DateTime? HireDate { get; set; } = null;
        public string? EmploymentTypeCode { get; set; } = null;
        public string? EmploymentType { get; set; } = null;
        public string? ReportingManagerCode { get; set; } = null;
        public string? ReportingManager { get; set; } = null;
        public string? DepartmentCode { get; set; } = null;
        public string? DepartmentName { get; set; } = null;
        public string? EmployeeStatusCode { get; set; } = null;
        public string? EmployeeStatus { get; set; } = null;
        #endregion
    }
}
