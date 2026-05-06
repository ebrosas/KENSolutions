using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class WorkflowDetailResultDTO
    {
        #region Fields
        private readonly string CONST_PENDING = "Pending";
        private readonly string CONST_APPROVED = "Approved";
        private readonly string CONST_REJECTED = "Rejected";
        #endregion

        #region Properties
        public long RequestNo { get; set; }
        public string WorkflowType { get; set; } = null!;
        public string WorkflowStatus { get; set; } = null!;
        public int ActivityID { get; set; }
        public string ActivityName { get; set; } = null!;
        public int ActivityOrder { get; set; }
        public string? ActivityStatus { get; set; } = null!;
        public int? ApproverNo { get; set; }
        public string? ApproverName { get; set; } = null;
        #endregion

        #region Extended Properties
        public bool IsCompleted 
        { 
            get
            {
                return ActivityStatus == CONST_APPROVED ? true : false;
            }
            set { }
        }

        public bool IsCurrent
        {
            get
            {
                return ActivityStatus == CONST_PENDING ? true : false;
            }
        }

        public string ApprovalSummary 
        { 
            get
            {
                if (ActivityStatus == CONST_APPROVED)
                    return $"Approved by {ApproverName} (Emp. #: {ApproverNo})";
                else if (ActivityStatus == CONST_REJECTED)
                    return $"Rejected by {ApproverName} (Emp. #: {ApproverNo})";
                else if (ActivityStatus == CONST_PENDING)
                    return "In-progress";
                else
                    return "Pending";
            }
            set { }
        }

        public string ApprovalDetails
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (ActivityStatus == CONST_APPROVED)
                {
                    return $"Approved by {ApproverName} (Emp. #: {ApproverNo})";
                }
                else if (ActivityStatus == CONST_REJECTED)
                    return $"Rejected by {ApproverName} (Emp. #: {ApproverNo})";
                else if (ActivityStatus == CONST_PENDING)
                    return $"Pending for approval with {ApproverName} (Emp. #: {ApproverNo})";
                else
                    return string.Empty;
            }
            set { }
        }
        #endregion
    }
}
