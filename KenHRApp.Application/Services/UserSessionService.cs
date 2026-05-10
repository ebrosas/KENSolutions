using KenHRApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public sealed class UserSessionService
    {
        #region Fields                
        private readonly object _lock = new();
        private UserSessionModel? _currentUser;
        #endregion

        #region Properties
        public UserSessionModel? CurrentUser
        {
            get
            {
                lock (_lock)
                {
                    return _currentUser;
                }
            }
        }
        #endregion

        #region Public Methods
        public bool IsAuthenticated =>
            _currentUser is not null &&
            _currentUser.IsAuthenticated;

        public void SetUser(UserSessionModel user)
        {
            ArgumentNullException.ThrowIfNull(user);

            lock (_lock)
            {
                _currentUser = user;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _currentUser = null;
            }
        }
        #endregion        
    }
}
