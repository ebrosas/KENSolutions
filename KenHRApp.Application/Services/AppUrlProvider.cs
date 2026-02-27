using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public sealed class AppUrlProvider : IAppUrlProvider
    {
        private readonly AppSettings _settings;

        public AppUrlProvider(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GetEmailVerificationUrl(string token)
        {
            var baseUrl = _settings.BaseUrl.TrimEnd('/');

            return $"{baseUrl}/UserAccount/VerifyEmail?token={Uri.EscapeDataString(token)}";
        }
    }
}
