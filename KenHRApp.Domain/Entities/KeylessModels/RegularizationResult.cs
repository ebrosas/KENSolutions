using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class RegularizationResult
    {
        #region Properties
        public long RegularizationId { get; set; }       // Identity column  
        public Guid AttachmentId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string CostCenter { get; set; } = null!;
        public string? CostCenterName { get; set; } = null;
        public DateTime AttendanceDate { get; set; }
        public string ROACode { get; set; } = null!;
        public string ROADesc { get; set; } = null;
        public string ActionCode { get; set; } = null!;
        public string ActionDesc { get; set; } = null;
        public TimeSpan RegularizedTimeIn { get; set; }
        public TimeSpan RegularizedTimeOut { get; set; }
        public string? ShiftPattern { get; set; } = null;
        public string RegularizedDescription { get; set; } = null!;
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
        public List<FileAttachment> AttachmentList { get; set; } = new();
        #endregion
    }
}
