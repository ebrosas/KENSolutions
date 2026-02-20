using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

public partial class SubmitTicketBase : ComponentBase
{
    [Inject] private ISupportTicketService SupportTicketService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    //private readonly IWebHostEnvironment _environment;
    [Inject] private IWebHostEnvironment Environment { get; set; } = default!;

    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
    protected MudForm? Form;
    protected SubmitTicketDTO Ticket = new();
    protected bool IsSubmitting = false;
    protected List<IBrowserFile> UploadedFiles { get; set; } = new();

    //protected void OnFilesChanged(IReadOnlyList<IBrowserFile> files)
    //{
    //    UploadedFiles = files.ToList();
    //}

    //protected void OnFilesChanged(FileUploadChangeEventArgs<IBrowserFile> args)
    //{
    //    UploadedFiles = args.Files.ToList();
    //}

    //protected async Task SubmitTicketAsyncOld()
    //{
    //    await Form!.Validate();

    //    if (!Form.IsValid)
    //        return;

    //    try
    //    {
    //        string test = _environment.WebRootPath;
    //        IsSubmitting = true;


    //        await SupportTicketService.CreateTicketAsync(Ticket, UploadedFiles);

    //        Snackbar.Add("Ticket submitted successfully.", Severity.Success);

    //        Ticket = new SubmitTicketDTO();
    //        UploadedFiles.Clear();
    //        await Form.ResetAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        Snackbar.Add($"Error: {ex.Message}", Severity.Error);
    //    }
    //    finally
    //    {
    //        IsSubmitting = false;
    //    }
    //}

    protected async Task SubmitTicketAsync()
    {
        await Form!.Validate();

        if (!Form.IsValid)
            return;

        try
        {
            var fileDtos = new List<FileUploadDTO>();

            foreach (var file in UploadedFiles)
            {
                var stream = file.OpenReadStream(10 * 1024 * 1024);

                fileDtos.Add(new FileUploadDTO
                {
                    FileName = file.Name,
                    ContentType = file.ContentType,
                    Size = file.Size,
                    Content = stream
                });
            }

            await SupportTicketService.CreateTicketAsync(Ticket, fileDtos, Environment.WebRootPath);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    #region Private Methods
    private async Task CreateTicketAsync(SubmitTicketDTO dto, List<FileUploadDTO> files)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var ticket = new SupportTicket(
            dto.Subject.Trim(),
            dto.Requester.Trim(),
            dto.Description.Trim());

        string uploadPath = Path.Combine(
            Environment.WebRootPath,
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

        //await _context.SupportTickets.AddAsync(ticket);
        //await _context.SaveChangesAsync();
    }
    #endregion
}