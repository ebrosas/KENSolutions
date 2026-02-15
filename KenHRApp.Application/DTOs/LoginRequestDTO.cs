using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class LoginRequestDTO
    {
        #region Properties
        public string EmployeeCode { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
        #endregion
    }
}
