using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public interface ISupportTicketRepository
    {
        Task<Result<int>> CreateTicketAsync(SupportTicket dto, CancellationToken cancellationToken = default);
    }
}
