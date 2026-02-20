using KenHRApp.Application.DTOs;
using KenHRApp.Domain.Models.Common;
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
        Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO request);
        Task<Result<bool>> UnlockAccountAsync(UnlockAccountDTO dto);
        Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDTO dto);
        #endregion
    }
}
