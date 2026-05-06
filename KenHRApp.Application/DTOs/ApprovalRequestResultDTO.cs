using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class ApprovalRequestResultDTO
    {
        #region Properties
        public long RequestNo { get; set; }
        public string RequestTypeCode { get; set; } = null!;
        public string RequestTypeDesc { get; set; } = null!;
        public DateTime? AppliedDate { get; set; }
        public int? RequestedByNo { get; set; }
        public string? RequestedByName { get; set; } = null;
        public string? Detail { get; set; } = null;
        public string? ApprovalRole { get; set; } = null;
        public string CurrentStatus { get; set; } = null!;
        public int ApproverNo { get; set; }
        public string? ApproverName { get; set; } = null;
        public int? PendingDays { get; set; }
        public int? StepInstanceId { get; set; }
        public int? CreatedByEmpNo { get; set; }
        public string? Remarks { get; set; } = null;
        #endregion

        #region Extended Properties
        public string RequesterName
        {
            get
            {
                return $"{RequestedByName} (Emp.#: {RequestedByNo})";
            }
        }

        public string ApproverFullName
        {
            get
            {
                return $"{ApproverName} (Emp. #: {ApproverNo})";
            }
        }

        public string PendingSince
        {
            get
            {
                return $"{PendingDays} days";
            }
        }
        #endregion
    }
}
