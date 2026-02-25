using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Domain.Entities;
using KenHRApp.Web.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;
using System.Text.Json;

namespace KenHRApp.Web.Components.Pages.UserAccount
{
    public partial class SubmitTicket
    {
        #region Parameters and Injections
        [Inject] private ISupportTicketService SupportTicketService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
        [Inject] public NavigationManager Nav { get; set; }
        #endregion

        #region Fields
        private EditForm _editForm;
        private EditContext? _editContext;
        private StringBuilder _errorMessage = new StringBuilder();
        private List<string> _validationMessages = new();
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        private MudForm? Form;
        private string _webRootPath = string.Empty;
        private IBrowserFile _file = new DummyBrowserFile("test1.jpg", DateTimeOffset.Now, 0, "image/jpeg", []);

        #region Flags
        private bool _hasValidationError = false;
        private bool _showErrorAlert = false;
        private bool _disabled = false;
        private bool _btnProcessing = false;
        #endregion

        #region Objects and Collections
        private IReadOnlyList<IBrowserFile> _files;
        private List<IBrowserFile> UploadedFiles { get; set; } = new();
        private SubmitTicketDTO Ticket = new();
        #endregion

        #endregion

        #region Enums
        private enum SnackBarTypes
        {
            Normal,
            Information,
            Success,
            Warning,
            Error
        }

        private enum MessageBoxTypes
        {
            Info,
            Confirm,
            Warning,
            Error
        }
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(Ticket);
        }
        #endregion

        #region Validation Methods
        private void HandleInvalidSubmit(EditContext context)
        {
            _hasValidationError = true;
            _validationMessages = context.GetValidationMessages().ToList();
        }

        private void HandleValidSubmit(EditContext context)
        {
            try
            {
                // If we got here, model is valid
                _hasValidationError = false;
                _validationMessages.Clear();

                // Set flag to display the loading button
                _btnProcessing = true;

                // Set the overlay message
                overlayMessage = "Saving changes, please wait...";

                _ = SubmitTicketAsync(async () =>
                {
                    // Set flag to hide the loading button
                    _btnProcessing = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);
                });
            }
            catch (OperationCanceledException)
            {
                ShowNotification("Save cancelled (navigated away).", SnackBarTypes.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", SnackBarTypes.Error);
            }
        }
        #endregion

        #region Private Methods
        private void UploadFiles(InputFileChangeEventArgs e)
        {
            //If SuppressOnChangeWhenInvalid is false, perform your validations here
            //Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            //Snackbar.Add($"Selected file type: .{_model.File.Name.Split(".").Last()}", Severity.Info);

            //TODO upload the files to the server
        }

        private void RemoveFile(IBrowserFile file)
        {
            //_files.Remove(file);
            _files = _files.Where(f => f != file).ToList();
        }

        protected void GoToLogin()
        {
            Nav.NavigateTo("/login", true);
        }

        private void ShowNotification(string message, SnackBarTypes type, string position = Defaults.Classes.Position.TopCenter)
        {
            Snackbar.Clear();

            Snackbar.Configuration.PositionClass = position;
            Snackbar.Configuration.PreventDuplicates = false;
            Snackbar.Configuration.NewestOnTop = false;
            Snackbar.Configuration.ShowCloseIcon = true;
            Snackbar.Configuration.VisibleStateDuration = 5000;
            Snackbar.Configuration.HideTransitionDuration = 500;
            Snackbar.Configuration.ShowTransitionDuration = 500;
            Snackbar.Configuration.SnackbarVariant = Variant.Filled;

            switch (type)
            {
                case SnackBarTypes.Information:
                    Snackbar.Add(message, Severity.Info);
                    break;

                case SnackBarTypes.Success:
                    Snackbar.Add(message, Severity.Success);
                    break;

                case SnackBarTypes.Warning:
                    Snackbar.Add(message, Severity.Warning);
                    break;

                case SnackBarTypes.Error:
                    Snackbar.Add(message, Severity.Error);
                    break;

                default:
                    Snackbar.Add(message, Severity.Normal);
                    break;
            }
        }

        private void ShowHideError(bool value)
        {
            if (value)
            {
                _showErrorAlert = true;
            }
            else
            {
                _showErrorAlert = false;

                // Reset error messages
                _errorMessage.Clear();
            }
        }
        #endregion

        #region Database Methods
        private async Task SubmitTicketAsync(Func<Task> callback)
        {
            try
            {
                // Wait for 1 second then gives control back to the runtime
                await Task.Delay(500);

                // Reset error messages
                _errorMessage.Clear();

                // Initialize the cancellation token
                _cts = new CancellationTokenSource();

                bool isSuccess = true;
                string errorMsg = string.Empty;

                #region Initialize DTO
                var fileDtos = new List<FileUploadDTO>();

                foreach (var file in _files)
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
                #endregion

                var repoResult = await SupportTicketService.CreateTicketAsync(Ticket, fileDtos, Environment.WebRootPath, _cts.Token);
                isSuccess = repoResult.Success;
                if (!isSuccess)
                    errorMsg = repoResult.Error!;

                if (isSuccess)
                {
                    // Hide error message if any
                    ShowHideError(false);

                    // Show notification
                    ShowNotification("Support ticket has been submitted successfully!", SnackBarTypes.Success);
                }
                else
                {
                    // Set the error message
                    _errorMessage.AppendLine(errorMsg);
                    ShowHideError(true);
                }

                if (callback != null)
                {
                    // Hide the spinner overlay
                    await callback.Invoke();
                }
            }
            catch (Exception ex)
            {
                // Set the error message
                _errorMessage.Append(ex.Message.ToString());

                ShowHideError(true);
            }
        }

        private async Task CreateTicketAsync(SubmitTicketDTO dto, List<FileUploadDTO> files)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var ticket = new SupportTicket(
                dto.Subject.Trim(),
                dto.Requester.Trim(),
                dto.Description.Trim());

            string uploadPath = Path.Combine(
                _webRootPath,
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
}
