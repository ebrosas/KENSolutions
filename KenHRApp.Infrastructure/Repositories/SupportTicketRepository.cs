using KenHRApp.Domain.Entities;
using KenHRApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using KenHRApp.Domain.Models.Common;
using System.Net.Mail;

namespace KenHRApp.Infrastructure.Repositories
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        #endregion

        #region Constructor
        public SupportTicketRepository(
            AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Public Methods
        public async Task<Result<int>> CreateTicketAsync(SupportTicket dto, CancellationToken cancellationToken = default)
        {
            int rowsInserted = 0;

            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                var ticket = new SupportTicket(
                    dto.Subject.Trim(),
                    dto.Requester.Trim(),
                    dto.Description.Trim());

                if (dto.Attachments != null && dto.Attachments.Any())
                {
                    ticket.Attachments = dto.Attachments.Select(e => new SupportTicketAttachment
                    {
                        FileName = e.FileName,
                        StoredFileName = e.StoredFileName,
                        ContentType = e.ContentType,
                        FileSize = e.FileSize
                    }).ToList();
                }

                await _context.SupportTickets.AddAsync(ticket);

                // Save to database
                rowsInserted = await _context.SaveChangesAsync();

                return Result<int>.SuccessResult(rowsInserted);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }
        #endregion
    }
}
