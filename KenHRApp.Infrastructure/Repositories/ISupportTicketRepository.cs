using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public interface ISupportTicketRepository
    {
        Task CreateTicketAsync(
            SupportTicket dto,
            List<SupportTicketAttachment> files);
    }
}
