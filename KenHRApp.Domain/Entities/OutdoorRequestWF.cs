using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class OutdoorRequestWF
    {
        #region Properties
        public long OutdoorId { get; set; }       // Identity column  
        public Guid AttachmentId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public int EmpNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string EmpName { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string ROACode { get; set; } = null!;
                
        public string? DOWCode { get; set; } = null;

        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string ActionCode { get; set; } = null!;

        [Column(TypeName = "time")]
        public TimeSpan? StartTime { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? EndTime { get; set; }

        public int? StatusID { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string StatusCode { get; set; } = null!;
        
        [Column(TypeName = "varchar(20)")]
        public string? StatusHandlingCode { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? CostCenter { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUserID { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? CreatedEmail { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedUserID { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedEmail { get; set; } = null;
        #endregion

        #region Unmapped Properties
        [NotMapped]
        public string? ROADesc { get; set; } = null;

        [NotMapped]
        public string? DOWDesc { get; set; } = null;

        [NotMapped]
        public string ActionDesc { get; set; } = null!;

        [NotMapped]
        public string? StatusDesc { get; set; } = null;

        [NotMapped]
        public string? CostCenterName { get; set; } = null;

        [NotMapped]
        public string? CreatedByName { get; set; } = null;

        [NotMapped]
        public int? ApproverNo { get; set; }

        [NotMapped]
        public string? ApproverName { get; set; } = null;
        #endregion

        #region Public Methods
        public void AddAttachment(FileAttachment attachment)
        {
            var outdoorAttachment = new OutdoorAttachment(
                            attachment.AttachmentId,
                            attachment.RequestType,
                            attachment.FileName,
                            attachment.ContentType,
                            attachment.StoredFileName,
                            attachment.FileSize);

            AttachmentList.Add(outdoorAttachment);
        }
        #endregion

        #region Reference Navigations
        public List<OutdoorAttachment> AttachmentList { get; set; } = new();
        #endregion
    }
}
