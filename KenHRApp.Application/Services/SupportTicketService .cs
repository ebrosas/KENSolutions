using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using KenHRApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace KenHRApp.Application.Services
{
    public class SupportTicketService : ISupportTicketService
    {
        #region Fields
        private readonly ISupportTicketRepository _repository;
        //private readonly AppDbContext _context;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        #endregion

        #region Constructor
        public SupportTicketService(ISupportTicketRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<Result<int>> CreateTicketAsync(SubmitTicketDTO dto, List<FileUploadDTO> files, string webRootPath, CancellationToken cancellationToken = default)
        {
            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                // Initialize SupportTicket entity
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

                var result = await _repository.CreateTicketAsync(ticket, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save support ticket to the database due to an unknown error. Please refresh the page then try again!");
                }

                return Result<int>.SuccessResult(result.Value);

            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }
    }
}
