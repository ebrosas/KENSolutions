using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class FileAttachment
    {
        #region Properties        
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AttachmentId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string RequestType { get; set; } = null!;

        [Column(TypeName = "varchar(100)")]
        public string FileName { get; set; } = null!;

        [Column(TypeName = "varchar(250)")]
        public string StoredFileName { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string ContentType { get; set; } = null!;

        public long FileSize { get; set; }

        public byte[]? FileData { get; set; }
        #endregion

        #region Constructors
        public FileAttachment() { }

        public FileAttachment(
            Guid attachmentId, 
            string fileName, 
            string contentType, 
            string storedFileName, 
            long fileSize)
        {
            AttachmentId = attachmentId;
            FileName = fileName;
            ContentType = contentType;
            StoredFileName = storedFileName;
            FileSize = fileSize;
        }
        #endregion
    }
}
