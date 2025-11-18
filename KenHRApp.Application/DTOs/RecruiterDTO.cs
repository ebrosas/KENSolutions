using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class RecruiterDTO
    {
        #region Properties
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; } = null;
        public string LastName { get; set; } = null!;
        public int? WorkloadCount { get; set; }
        public string? Gender { get; set; } = null;
        #endregion
    }
}
