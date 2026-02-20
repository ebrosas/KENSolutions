using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class SupportTicketAttachment
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid SupportTicketId { get; private set; }
        public string FileName { get; private set; }
        public string StoredFileName { get; private set; }
        public string ContentType { get; private set; }
        public long FileSize { get; private set; }

        private SupportTicketAttachment() { }

        public SupportTicketAttachment(
            Guid supportTicketId,
            string fileName,
            string storedFileName,
            string contentType,
            long fileSize)
        {
            SupportTicketId = supportTicketId;
            FileName = fileName;
            StoredFileName = storedFileName;
            ContentType = contentType;
            FileSize = fileSize;
        }
    }
}
