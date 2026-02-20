using KenHRApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface ISupportTicketService
    {
        Task CreateTicketAsync(SubmitTicketDTO dto, List<FileUploadDTO> files, string webRootPath);
    }
}
