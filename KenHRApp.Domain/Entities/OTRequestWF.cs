using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class OTRequestWF
    {
        #region Properties
        public long ExtratimeId { get; set; }       // Identity column  
        public long TS_AutoId { get; set; }       
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public int EmployeeNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string EmployeeName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string CostCenter { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime AttendanceDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string OTReasonCode { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string ActionCode { get; set; } = null!;

        [Column(TypeName = "time")]
        public TimeSpan OTStartTime { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan OTEndTime { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? ShiftPattern { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? ShiftTiming { get; set; } = null;

        [Column(TypeName = "varchar(500)")]
        public string? Remarks { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string StatusCode { get; set; } = null!;

        public int? StatusID { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? StatusHandlingCode { get; set; } = null;

        public int? WorkDuration { get; set; } = null;
        public int? OTDuration { get; set; } = null;

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
        public string? OTReasonDesc { get; set; } = null;

        [NotMapped]
        public string ActionDescription { get; set; } = null!;
        #endregion
    }
}
