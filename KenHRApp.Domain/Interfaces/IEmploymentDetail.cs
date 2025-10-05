using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Interfaces
{
    public interface IEmploymentDetail
    {
        int EmployeeNo { get; set; }
        string? EmployeeStatusCode { get; set; } 
        int? ReportingManagerCode { get; set; }
        string? WorkPermitID { get; set; }
        DateTime? WorkPermitExpiryDate { get; set; }
        DateTime HireDate { get; set; }
        DateTime? DateOfConfirmation { get; set; }
        DateTime? TerminationDate { get; set; }
        DateTime? DateOfSuperannuation { get; set; }
        bool? Reemployed { get; set; }
        int? OldEmployeeNo { get; set; }
        string DepartmentCode { get; set; }
        string EmploymentTypeCode { get; set; }
        string RoleCode { get; set; }
        string FirstAttendanceModeCode { get; set; }
        string? SecondAttendanceModeCode { get; set; }
        string? ThirdAttendanceModeCode { get; set; }
        int? SecondReportingManagerCode { get; set; }
    }
}
