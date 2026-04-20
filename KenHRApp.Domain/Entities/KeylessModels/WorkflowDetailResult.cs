using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class WorkflowDetailResult
    {
        #region Properties
        public long RequestNo { get; set; }
        public string WorkflowType { get; set; } = null!;
        public string WorkflowStatus { get; set; } = null!;
        public int ActivityID { get; set; }
        public string ActivityName { get; set; } = null!;
        public int ActivityOrder { get; set; }
        public string ActivityStatus { get; set; } = null!;
        public int ApproverNo { get; set; }
        public string? ApproverName { get; set; } = null;
        #endregion
    }
}
