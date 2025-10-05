using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Interfaces
{
    public interface IPrimaryLocation
    {
        #region Properties
        string? PresentAddress { get; set; }
        string? PresentCountryCode { get; set; }
        string? PresentCity { get; set; }
        string? PresentAreaCode { get; set; }
        string? PresentContactNo { get; set; }
        string? PresentMobileNo { get; set; }
        string? PermanentAddress { get; set; }
        string? PermanentCountryCode { get; set; }
        string? PermanentCity { get; set; }
        string? PermanentAreaCode { get; set; }
        string? PermanentContactNo { get; set; }
        string? PermanentMobileNo { get; set; }
        #endregion
    }
}
