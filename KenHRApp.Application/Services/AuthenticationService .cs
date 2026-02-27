using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly IEmployeeRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IAppUrlProvider _appUrlProvider;
        #endregion

        #region Constructor
        public AuthenticationService(
            IEmployeeRepository repository,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IAppUrlProvider appUrlProvider)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _appUrlProvider = appUrlProvider;
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

        public async Task<Result<bool>> RegisterUserAccountAsync(UserAccountDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var repoResult = await _repository.GetByEmployeeNoAndHireDateAsync(dto.EmployeeNo,
                    dto.DOJ!.Value, cancellationToken);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var employee = repoResult.Value;
                if (employee == null)
                {
                    throw new Exception("Employee not found.");
                }

                // Create hash of the password if it's not null or empty, otherwise set it to null
                string? passwordHash = null;
                if (string.IsNullOrEmpty(dto.Password))
                    passwordHash = _passwordHasher.Hash(dto.Password);

                employee.RegisterAccount(
                    dto.UserID,
                    dto.Email,
                    _passwordHasher.Hash(dto.Password),
                    dto.SecurityQuestion1,
                    dto.SecurityAnswer1,
                    dto.SecurityQuestion2,
                    dto.SecurityAnswer2,
                    dto.SecurityQuestion3,
                    dto.SecurityAnswer3
                );

                // Save changes to the database
                var saveResult = await _repository.UpdateAsync(employee);
                if (!saveResult.Success)
                {
                    return Result<bool>.Failure(saveResult.Error ?? "Unknown repository error");
                }

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in LoginAsync() method.");
            }
        }               

        public async Task<Result<bool>> SendVerificationEmailAsync(
            string email, 
            int empNo, 
            DateTime doj, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var token = await GenerateEmailVerificationTokenAsync(
                    empNo, 
                    doj, 
                    cancellationToken);

                if (string.IsNullOrEmpty(token))
                    return Result<bool>.Failure("Unable to generate email verification token!");

                var verificationLink = _appUrlProvider
                    .GetEmailVerificationUrl(token);

                #region Build the email contents
                var subject = "Verify your KenHR account";

                var body = $@"
                    Hello,

                    Thank you for registering with KenHR.

                    Please verify your email by clicking the link below:

                    {verificationLink}

                    Note that the verification link is valid for 24 hours only. If you did not initiate this request, please ignore this email.

                    Regards,
                    KenHR Team";
                #endregion

                try
                {
                    await _emailService.SendAsync(email, subject, body);
                }
                catch (Exception emailErr)
                {
                    //throw;
                }

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in LoginAsync() method.");
            }
        }

        public async Task<string?> GenerateEmailVerificationTokenAsync(int empNo, DateTime doj, CancellationToken cancellationToken = default)
        {
            try
            {
                var repoResult = await _repository.GenerateEmailVerificationTokenAsync(empNo,
                    doj, cancellationToken);
                if (!repoResult.Success)
                {
                    return null;
                }

                var token = repoResult.Value;
                if (token == null)
                {
                    throw new Exception("Employee not found.");
                }
                else
                    return token.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Result<bool>> VerifyEmailTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            try
            {
                var repoResult = await _repository.VerifyEmailTokenAsync(token, cancellationToken);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }
                
                if (repoResult!.Value == null)
                {
                    throw new Exception("Unable to verify token due to unknown error in the DB!");
                }

                return Result<bool>.SuccessResult(repoResult.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in VerifyEmailTokenAsync() method.");
            }
        }
        #endregion
    }
}
