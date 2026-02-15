using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class LoginResponseDTO
    {
        #region Properties
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public int FailedAttempts { get; set; }
        #endregion
    }
}
