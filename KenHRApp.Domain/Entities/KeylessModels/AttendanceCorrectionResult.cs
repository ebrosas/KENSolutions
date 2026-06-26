using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class AttendanceCorrectionResult
    {
        #region Properties
        public string RequestTypeCode { get; set; } = null!;
        public string RequestTypeDesc { get; set; } = null!;
        public long RequestNo { get; set; }
        public DateTime? RequestDate { get; set; }
        public int OrigEmpNo { get; set; }
        public string OrigEmpName { get; set; } = null!;
        public string CostCenter { get; set; } = null!;
        public string? CostCenterName { get; set; } = null;
        public DateTime? AppliedDate { get; set; }
        public int? RequestedByNo { get; set; }
        public string? RequestedByName { get; set; } = null;
        public string? RequestDetail { get; set; } = null;
        //public int? CreatedByEmpNo { get; set; }
        public string CurrentStatus { get; set; } = null!;
        public string? StatusCode { get; set; } = null;
        public string? StatusDesc { get; set; } = null;
        public int? CurrentlyAssignedEmpNo { get; set; }
        public string? CurrentlyAssignedEmpName { get; set; } = null;
        #endregion
    }
}
