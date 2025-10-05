using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Interfaces
{
    public interface IAttributeDetail
    {
        string Company { get; set; }
        string? CompanyBranch { get; set; }
        string? EducationCode { get; set; }
        string? EmployeeClassCode { get; set; }
        string JobTitleCode { get; set; }
        string PayGrade { get; set; }
        bool IsActive { get; set; }
    }
}
