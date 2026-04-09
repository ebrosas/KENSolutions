using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.Workflow
{
    public class WorkflowStepInstance
    {
        #region Properties
        public int StepInstanceId { get; set; }         // Identity column  
        public int WorkflowInstanceId { get; set; }     // Foreign Key to WorkflowCondition
        public int StepDefinitionId { get; set; }
        public int ApproverEmpNo { get; set; }
        public string? ApproverUserID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ApproverRole { get; set; } = default!;

        [Column(TypeName = "varchar(50)")]
        public string Status { get; set; } = "Pending";

        public DateTime? ActionDate { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string? Comments { get; set; } = null;
        #endregion
    }
}
