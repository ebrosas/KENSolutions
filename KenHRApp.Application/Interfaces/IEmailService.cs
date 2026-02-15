using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(
            string to,
            string subject,
            string body,
            bool isHtml = false,
            CancellationToken cancellationToken = default);
    }
}
