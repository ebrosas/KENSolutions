using KenHRApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IAuthenticationService
    {
        #region Web Methods
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<bool> UnlockAccountAsync(UnlockAccountDTO dto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDTO dto);
        #endregion
    }
}
