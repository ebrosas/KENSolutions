using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Interfaces
{
    public interface IContactDetail
    {
        string OfficialEmail { get; set; } 
        string? PersonalEmail { get; set; }
        string? AlternateEmail { get; set; }
        string? OfficeLandlineNo { get; set; }
        string? ResidenceLandlineNo { get; set; }
        string? OfficeExtNo { get; set; }
        string? MobileNo { get; set; }
        string? AlternateMobileNo { get; set; }
    }
}
