using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public sealed class UserSessionService : IUserSessionService
    {
        #region Fields     
        private const string StorageKey = "HRMS_USER_SESSION";
        private readonly IJSRuntime _jsRuntime;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
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
            set { }
        }
        #endregion

        #region Constructor
        public UserSessionService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        #endregion

        #region Public Methods
        //public bool IsAuthenticated =>
        //    _currentUser is not null &&
        //    _currentUser.IsAuthenticated;

        public bool IsAuthenticated()
        {
            return _currentUser is not null && _currentUser.IsAuthenticated;
        }                

        public async Task InitializeAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (_currentUser is not null)
                    return;

                _currentUser = await _jsRuntime.InvokeAsync<UserSessionModel?>(
                    "hrmsSessionStorage.get",
                    StorageKey);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SetUserAsync(
        UserSessionModel user)
        {
            ArgumentNullException.ThrowIfNull(user);

            await _semaphore.WaitAsync();

            try
            {
                _currentUser = user;

                await _jsRuntime.InvokeVoidAsync(
                    "hrmsSessionStorage.set",
                    StorageKey,
                    user);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task ClearAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                _currentUser = null;

                await _jsRuntime.InvokeVoidAsync(
                    "hrmsSessionStorage.remove",
                    StorageKey);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        //public void SetUser(UserSessionModel user)
        //{
        //    ArgumentNullException.ThrowIfNull(user);

        //    lock (_lock)
        //    {
        //        _currentUser = user;
        //    }
        //}

        //public void Clear()
        //{
        //    lock (_lock)
        //    {
        //        _currentUser = null;
        //    }
        //}
        #endregion        
    }
}
