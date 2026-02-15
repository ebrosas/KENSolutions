using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Interfaces
{
    public interface IAuthenticationDetail
    {
        string? UserID { get; set; }
        string? PasswordHash { get; set; }
        int FailedLoginAttempts { get; set; }
        bool IsLocked { get; set; }
    }
}
