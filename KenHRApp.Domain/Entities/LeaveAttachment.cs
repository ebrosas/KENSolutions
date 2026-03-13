using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class LeaveAttachment
    {
        #region Properties
        public Guid Id { get; private set; } = Guid.NewGuid();

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
        private LeaveAttachment() { }

        public LeaveAttachment(int leaveRequestId, string fileName, string contentType, 
            string storedFileName, long fileSize, byte[] fileData)
        {
            LeaveRequestId = leaveRequestId;
            FileName = fileName;
            ContentType = contentType;
            StoredFileName = storedFileName;
            FileSize = fileSize;
            FileData = fileData;
        }
        #endregion

        #region Reference Navation to LeaveRequisitionWF
        [Comment("Foreign key that references primary key: LeaveRequisitionWF.LeaveRequestId")]
        [Column(TypeName = "varchar(20)")]
        public long LeaveRequestId { get; set; }

        public LeaveRequisitionWF LeaveRequest { get; set; } = default!;
        #endregion
    }
}
