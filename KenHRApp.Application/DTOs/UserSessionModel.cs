using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public sealed class UserSessionModel
    {
        #region Properties
        public Guid UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public int UserEmpNo { get; set; }

        public string CostCenter { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        #endregion

        #region Public Methods
        public bool IsAuthenticated =>
            UserId != Guid.Empty;
        #endregion
    }
}
