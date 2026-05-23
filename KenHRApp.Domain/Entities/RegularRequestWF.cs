using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class RegularRequestWF
    {
        #region Properties
        public long RegularizationId { get; set; }       // Identity column  
        public Guid AttachmentId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public int EmployeeNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string EmployeeName { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime AttendanceDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string ROACode { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string ActionCode { get; set; } = null!;

        [Column(TypeName = "time")]
        public TimeSpan RegularizedTimeIn { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan RegularizedTimeOut { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? ShiftPattern { get; set; } = null;

        [Column(TypeName = "varchar(500)")]
        public string RegularizedDescription { get; set; } = null!;

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
        public string? ROADescription { get; set; } = null;

        [NotMapped]
        public string ActionDescription { get; set; } = null!;

        [NotMapped]
        public string? ShiftTiming { get; set; } = null;

        [NotMapped]
        public string? ActualTiming { get; set; } = null;

        [NotMapped]
        public int WorkDuration { get; set; }

        [NotMapped]
        public int NoPayHours { get; set; }
        #endregion

        #region Public Methods
        public void AddAttachment(FileAttachment attachment)
        {
            AttachmentList.Add(attachment);
        }
        #endregion

        #region Reference Navigations
        public List<FileAttachment> AttachmentList { get; set; } = new();
        #endregion
    }
}
