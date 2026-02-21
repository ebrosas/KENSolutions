using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class SupportTicketAttachment
    {
        #region Properties
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid SupportTicketId { get; private set; }

        [Column(TypeName = "varchar(100)")]
        public string FileName { get; set; } = null!;

        [Column(TypeName = "varchar(250)")]
        public string StoredFileName { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string ContentType { get; set; } = null!;

        public long FileSize { get; set; }
        #endregion

        #region Constructors
        public SupportTicketAttachment() { }

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
        #endregion
    }
}
