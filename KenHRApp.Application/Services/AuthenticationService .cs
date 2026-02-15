using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly IEmployeeRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        #endregion

        #region Constructor
        public AuthenticationService(
            IEmployeeRepository repository,
            IPasswordHasher passwordHasher,
            IEmailService emailService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }
        #endregion

        #region Public Methods
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var employee = await _repository
                .GetByEmployeeCodeOrEmailAsync(request.EmployeeCode);

            if (employee == null)
            {
                return new LoginResponseDTO
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid credentials."
                };
            }

            bool isLocked = false;
            if (bool.TryParse(employee.IsLocked.ToString(), out var locked) && locked)
            {
                isLocked = true;
            }

            if (isLocked)
            {
                return new LoginResponseDTO
                {
                    IsSuccess = false,
                    ErrorMessage = "Account is locked."
                };
            }

            var isValid = _passwordHasher.Verify(
                employee.PasswordHash!,
                request.Password);

            if (!isValid)
            {
                //employee.IncrementFailedAttempts();
                await _repository.UpdateAsync(employee);

                return new LoginResponseDTO
                {
                    IsSuccess = false,
                    FailedAttempts = employee.FailedLoginAttempts,
                    ErrorMessage =
                        $"Invalid credentials. Attempt {employee.FailedLoginAttempts}/3"
                };
            }

            //employee.ResetFailedAttempts();
            await _repository.UpdateAsync(employee);

            return new LoginResponseDTO
            {
                IsSuccess = true
            };
        }

        public async Task<bool> UnlockAccountAsync(UnlockAccountDTO dto)
        {
            var employee = await _repository
                .GetByEmployeeCodeOrEmailAsync(dto.EmployeeCode);

            if (employee == null)
                return false;

            if (employee.DOB!.Value.Date != dto.DateOfBirth.Date ||
                employee.HireDate.Date != dto.DateOfJoining.Date)
                return false;

            //employee.UnlockAccount();
            await _repository.UpdateAsync(employee);

            await _emailService.SendAsync(
                employee.OfficialEmail,
                "Security Code",
                "<p>Your security code is: <b>123456</b></p>",
                true);

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDTO dto)
        {
            var employee = await _repository
                .GetByEmployeeCodeOrEmailAsync(dto.EmployeeCode);

            if (employee == null)
                return false;

            var tempPassword = Guid.NewGuid()
                                   .ToString("N")[..8];

            //employee.ChangePassword(
            //    _passwordHasher.Hash(tempPassword));

            await _repository.UpdateAsync(employee);

            await _emailService.SendAsync(
                employee.OfficialEmail,
                "Temporary Password",
                $"<p>Your temporary password is: <b>{tempPassword}</b></p>",
                true);

            return true;
        }
        #endregion
    }
}
