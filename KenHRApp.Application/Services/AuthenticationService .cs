using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Models.Common;
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
        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                var repoResult = await _repository.GetByUserIDOrEmailAsync(request.EmployeeCode);
                if (!repoResult.Success)
                {
                    return Result<LoginResponseDTO>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var employee = repoResult.Value;
                if (employee == null)
                {
                    return Result<LoginResponseDTO>.Failure("Invalid credentials.");
                }

                bool isLocked = false;
                if (bool.TryParse(employee.IsLocked.ToString(), out var locked) && locked)
                {
                    isLocked = true;
                }

                if (isLocked)
                {
                    return Result<LoginResponseDTO>.Failure("Account is locked.");
                }

                //string testHashPwd = _passwordHasher.Hash("Garmco#1017");

                //var isValid = _passwordHasher.Verify(
                //    testHashPwd,
                //    request.Password);

                var isValid = _passwordHasher.Verify(
                    employee.PasswordHash!,
                    request.Password);

                if (!isValid)
                {
                    employee.IncrementFailedAttempts();
                    await _repository.UpdateAsync(employee);
                    return Result<LoginResponseDTO>.Failure($"Invalid credentials. Attempt {employee.FailedLoginAttempts}/3");
                }

                employee.ResetFailedAttempts();
                await _repository.UpdateAsync(employee);

                return Result<LoginResponseDTO>.SuccessResult(new LoginResponseDTO
                {
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return Result<LoginResponseDTO>.Failure(ex.Message.ToString() ?? "Unknown error in LoginAsync() method.");
            }
        }

        private async Task<Result<bool>> UnlockAccountAsyncOld(UnlockAccountDTO dto)
        {
            try
            {
                var repoResult = await _repository.GetByUserIDOrEmailAsync(dto.EmployeeCode);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var employee = repoResult.Value;
                if (employee == null)
                {
                    throw new Exception("Unable to find a matching employee in the database with the specified Date of Birth and Joining Date.");
                }

                if (employee.DOB!.Value.Date != dto.DateOfBirth.Date ||
                    employee.HireDate.Date != dto.DateOfJoining.Date)
                {
                    throw new Exception("Either the specified Date of Birth or Joining Date does not matched with the record in the database.");
                }

                employee.UnlockAccount();
                await _repository.UpdateAsync(employee);

                await _emailService.SendAsync(
                    employee.OfficialEmail,
                    "Security Code",
                    "<p>Your security code is: <b>123456</b></p>",
                    true);

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in LoginAsync() method.");
            }
        }

        public async Task<Result<int>> UnlockAccountAsync(UnlockAccountDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                var result = await _repository.UnlockUserAccountAsync(dto.EmployeeCode, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to unlock user account due to an unknown error. Please refresh the page then try again!");
                }

                //if (!repoResult.Success)
                //{
                //    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                //}

                //var employee = repoResult.Value;
                //if (employee == null)
                //{
                //    throw new Exception("Unable to find a matching employee in the database with the specified Date of Birth and Joining Date.");
                //}

                //if (employee.DOB!.Value.Date != dto.DateOfBirth.Date ||
                //    employee.HireDate.Date != dto.DateOfJoining.Date)
                //{
                //    throw new Exception("Either the specified Date of Birth or Joining Date does not matched with the record in the database.");
                //}

                //employee.UnlockAccount();
                //await _repository.UpdateAsync(employee);

                //await _emailService.SendAsync(
                //    employee.OfficialEmail,
                //    "Security Code",
                //    "<p>Your security code is: <b>123456</b></p>",
                //    true);

                //return Result<bool>.SuccessResult(true);


                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString() ?? "Unknown error in UnlockAccountAsync() method.");
            }
        }

        public async Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var repoResult = await _repository.GetByEmployeeCodeAndHireDateAsync(dto.EmployeeCode, 
                    dto.DateOfJoining!.Value, cancellationToken);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var employee = repoResult.Value;
                if (employee == null)
                {
                    throw new Exception("Employee not found.");
                }

                var tempPassword = Guid.NewGuid()
                                       .ToString("N")[..8];

                employee.ChangePassword(_passwordHasher.Hash(tempPassword));

                await _repository.UpdateAsync(employee);

                try
                {
                    await _emailService.SendAsync(
                    employee.OfficialEmail,
                    "Temporary Password",
                    $"<p>Your temporary password is: <b>{tempPassword}</b></p>",
                    true);
                }
                catch (Exception emailErr)
                {
                    
                }

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in LoginAsync() method.");
            }
        }
        #endregion
    }
}
