using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class OutdoorResult
    {
        #region Properties
        public long OutdoorId { get; set; }       // Identity column  
        public Guid AttachmentId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public int EmpNo { get; set; }
        public string EmpName { get; set; } = null!;
        public string? CostCenter { get; set; } = null;
        public string? CostCenterName { get; set; } = null;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ROACode { get; set; } = null!;
        public string? ROADesc { get; set; } = null;
        public string ActionCode { get; set; } = null!;
        public string? ActionDesc { get; set; } = null;
        public string? DOWCode { get; set; } = null;
        public string? DOWDesc { get; set; } = null;        
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Description { get; set; } = null!;
        public int? StatusID { get; set; }
        public string StatusCode { get; set; } = null!;
        public string? StatusDesc { get; set; } = null;
        public string? StatusHandlingCode { get; set; } = null;
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedUserID { get; set; } = null;
        public string? CreatedEmail { get; set; } = null;
        public string? CreatedByName { get; set; } = null;
        public DateTime? LastUpdatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public string? LastUpdatedUserID { get; set; } = null;
        public string? LastUpdatedEmail { get; set; } = null;
        public int? ApproverNo { get; set; }
        public string? ApproverName { get; set; } = null;
        public List<OutdoorAttachment> AttachmentList { get; set; } = new();
        #endregion
    }
}
