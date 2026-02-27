using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Interfaces
{
    public interface IAppUrlProvider
    {
        string GetEmailVerificationUrl(string token);
    }
}
