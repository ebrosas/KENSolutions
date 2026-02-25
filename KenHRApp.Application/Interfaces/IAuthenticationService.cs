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
        Task<Result<int>> UnlockAccountAsync(UnlockAccountDTO dto, CancellationToken cancellationToken = default);
        Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDTO dto, CancellationToken cancellationToken = default);
        Task<Result<bool>> RegisterUserAccountAsync(UserAccountDTO dto, CancellationToken cancellationToken = default);
        #endregion
    }
}
