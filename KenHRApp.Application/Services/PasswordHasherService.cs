using KenHRApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KenHRApp.Application.Services
{
    public class PasswordHasherService : IPasswordHasher
    {
        #region Fields
        private readonly PasswordHasher<object> _hasher;
        #endregion


        #region Constructors
        public PasswordHasherService()
        {
            _hasher = new PasswordHasher<object>();
        }
        #endregion

        #region Public Methods
        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            return _hasher.HashPassword(null!, password);
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("Hashed password cannot be empty.");

            var result = _hasher.VerifyHashedPassword(
                null!,
                hashedPassword,
                providedPassword);

            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
        #endregion
    }
}
