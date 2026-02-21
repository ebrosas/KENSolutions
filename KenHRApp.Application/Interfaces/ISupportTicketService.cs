using KenHRApp.Application.DTOs;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface ISupportTicketService
    {
        Task<Result<int>> CreateTicketAsync(SubmitTicketDTO dto, List<FileUploadDTO> files, string webRootPath, CancellationToken cancellationToken = default);
    }
}
