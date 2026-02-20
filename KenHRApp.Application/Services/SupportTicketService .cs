using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;

namespace KenHRApp.Application.Services
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly AppDbContext _context;
        //private readonly IWebHostEnvironment _environment;

        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public SupportTicketService(
            AppDbContext context)
            //IWebHostEnvironment environment)
        {
            _context = context;
            //_environment = environment;
        }

        //public async Task CreateTicketAsync(SubmitTicketDTO dto, List<FileUploadDTO> files)
        //{
        //    var ticket = new SupportTicket(
        //        dto.Subject.Trim(),
        //        dto.Requester.Trim(),
        //        dto.Description.Trim());

        //    string uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "support");

        //    if (!Directory.Exists(uploadPath))
        //        Directory.CreateDirectory(uploadPath);

        //    foreach (var file in files)
        //    {
        //        if (file.Size > MaxFileSize)
        //            throw new Exception($"File {file.FileName} exceeds 10MB limit.");

        //        string storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //        string fullPath = Path.Combine(uploadPath, storedFileName);

        //        await using var stream = new FileStream(fullPath, FileMode.Create);
        //        await file.OpenReadStream(MaxFileSize).CopyToAsync(stream);

        //        var attachment = new SupportTicketAttachment(
        //            ticket.Id,
        //            file.Name,
        //            storedFileName,
        //            file.ContentType,
        //            file.Size);

        //        ticket.AddAttachment(attachment);
        //    }

        //    _context.SupportTickets.Add(ticket);
        //    await _context.SaveChangesAsync();
        //}

        public async Task CreateTicketAsync(SubmitTicketDTO dto, List<FileUploadDTO> files, string webRootPath)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var ticket = new SupportTicket(
                dto.Subject.Trim(),
                dto.Requester.Trim(),
                dto.Description.Trim());

            string uploadPath = Path.Combine(
                webRootPath,
                "uploads",
                "support");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            if (files is not null && files.Any())
            {
                foreach (var file in files)
                {
                    if (file.Size > MaxFileSize)
                        throw new InvalidOperationException(
                            $"File {file.FileName} exceeds 10MB limit.");

                    if (file.Content is null)
                        throw new InvalidOperationException(
                            $"File stream for {file.FileName} is null.");

                    string storedFileName =
                        $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    string fullPath =
                        Path.Combine(uploadPath, storedFileName);

                    await using (var fileStream = new FileStream(
                        fullPath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        81920,
                        useAsync: true))
                    {
                        await file.Content.CopyToAsync(fileStream);
                    }

                    var attachment = new SupportTicketAttachment(
                        ticket.Id,
                        file.FileName,
                        storedFileName,
                        file.ContentType,
                        file.Size);

                    ticket.AddAttachment(attachment);
                }
            }

            await _context.SupportTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
