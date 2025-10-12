using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class RecruitmentBudget
    {
        #region Properties
        public int BudgetId { get; set; }
        
        [Column(TypeName = "varchar(20)")]
        public string DepartmentCode { get; set; } = null!;

        [NotMapped]
        public string? DepartmentName { get; set; } = null;

        [Required(ErrorMessage = "Head Count Budget is required")]
        public int BudgetHeadCount { get; set; }

        public int? ActiveCount { get; set; }
        public int? ExitCount { get; set; }
        public int? RequisitionCount { get; set; }
        public int? NetGapCount { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string? Remarks { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; } = null;
        #endregion
    }
}
