using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.Workflow
{
    public class WorkflowCondition
    {
        #region Properties
        public int ConditionId { get; set; }            // Identity column  
        public int StepDefinitionId { get; set; }       

        [Column(TypeName = "varchar(100)")]
        public string FieldName { get; set; } = default!;

        [Column(TypeName = "varchar(20)")]
        public string Operator { get; set; } = default!;

        [Column(TypeName = "varchar(50)")]
        public string CompareValue { get; set; } = default!;

        public int NextStepDefinitionId { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Expression { get; set; }
        #endregion
    }
}
