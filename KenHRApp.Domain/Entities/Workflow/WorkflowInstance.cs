using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.Workflow
{
    public class WorkflowInstance
    {
        #region Properties
        public int WorkflowInstanceId { get; set; }         // Identity column  
        public int WorkflowDefinitionId { get; set; }       // Foreign Key to WorkflowDefinition
        public long EntityId { get; set; }                  // Ex. LeaveRequestId

        [Column(TypeName = "varchar(50)")]
        public string Status { get; set; } = "Running";

        public List<WorkflowStepInstance> Steps { get; set; } = new();
        #endregion
    }
}
