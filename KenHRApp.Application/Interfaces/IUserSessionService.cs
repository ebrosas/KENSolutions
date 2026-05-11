using KenHRApp.Application.DTOs;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IUserSessionService
    {
        #region Properties
        UserSessionModel? CurrentUser { get; set; }
        #endregion

        #region Public Methods
        bool IsAuthenticated();
        
        Task InitializeAsync();

        Task SetUserAsync(UserSessionModel user);

        Task ClearAsync();

        //void SetUser(UserSessionModel user);
        //void Clear();
        #endregion
    }
}
