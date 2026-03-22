using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class LeaveAttachmentDTO
    {
        #region Properties
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LeaveAttachmentId { get; set; }
        public long LeaveRequestId { get; set; }

        public string FileName { get; set; } = null!;

        public string StoredFileName { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public long FileSize { get; set; }

        public byte[]? FileData { get; set; }
        #endregion
    }
}
