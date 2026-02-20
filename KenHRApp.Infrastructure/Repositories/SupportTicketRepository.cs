using KenHRApp.Domain.Entities;
using KenHRApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace KenHRApp.Infrastructure.Repositories
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        //private readonly IWebHostEnvironment _environment;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        #endregion

        //public SupportTicketRepository(
        //    AppDbContext context,
        //    IWebHostEnvironment environment)
        //{
        //    _context = context ?? throw new ArgumentNullException(nameof(context));
        //    _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        //}

        public SupportTicketRepository(
            AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateTicketAsync(
         SupportTicket dto,
         List<SupportTicketAttachment> files)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var ticket = new SupportTicket(
                dto.Subject.Trim(),
                dto.Requester.Trim(),
                dto.Description.Trim());

            //string uploadPath = Path.Combine(
            //    webRootPath,
            //    "uploads",
            //    "support");

            //if (!Directory.Exists(uploadPath))
            //    Directory.CreateDirectory(uploadPath);

            //if (files is not null && files.Any())
            //{
            //    foreach (var file in files)
            //    {
            //        if (file.Size > MaxFileSize)
            //            throw new InvalidOperationException(
            //                $"File {file.FileName} exceeds 10MB limit.");

            //        if (file.Content is null)
            //            throw new InvalidOperationException(
            //                $"File stream for {file.FileName} is null.");

            //        string storedFileName =
            //            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            //        string fullPath =
            //            Path.Combine(uploadPath, storedFileName);

            //        await using (var fileStream = new FileStream(
            //            fullPath,
            //            FileMode.Create,
            //            FileAccess.Write,
            //            FileShare.None,
            //            81920,
            //            useAsync: true))
            //        {
            //            await file.Content.CopyToAsync(fileStream);
            //        }

            //        var attachment = new SupportTicketAttachment(
            //            ticket.Id,
            //            file.FileName,
            //            storedFileName,
            //            file.ContentType,
            //            file.Size);

            //        ticket.AddAttachment(attachment);
            //    }
            //}

            await _context.SupportTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
