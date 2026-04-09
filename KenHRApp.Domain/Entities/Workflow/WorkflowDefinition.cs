using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.Workflow
{
    public class WorkflowDefinition
    {
        #region Properties
        public int WorkflowDefinitionId { get; set; }       // Identity column  

        [Column(TypeName = "varchar(150)")]
        public string Name { get; set; } = default!;

        [Column(TypeName = "varchar(100)")]
        public string EntityName { get; set; } = default!;

        [Column(TypeName = "bit")]
        public bool IsActive { get; set; }

        public List<WorkflowStepDefinition> Steps { get; set; } = new();
        #endregion
    }
}
