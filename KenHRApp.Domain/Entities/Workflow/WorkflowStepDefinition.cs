using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.Workflow
{
    public class WorkflowStepDefinition
    {
        #region Properties
        public int StepDefinitionId { get; set; }       // Identity column  
        public int WorkflowDefinitionId { get; set; }   // Foreign Key to WorkflowDefinition

        [Column(TypeName = "varchar(200)")]
        public string StepName { get; set; } = default!;

        public int StepOrder { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ApprovalRole { get; set; } = default!;

        [Column(TypeName = "bit")]
        public bool IsParallelGroup { get; set; }

        public Guid? ParallelGroupId { get; set; }

        [Column(TypeName = "bit")]
        public bool RequiresAllParallel { get; set; }

        public List<WorkflowCondition> Conditions { get; set; } = new();
        #endregion
    }
}
