using KenHRApp.Application.Common.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class RecruitmentBudgetDTO
    {
        #region Properties
        public int BudgetId { get; set; }

        public string DepartmentCode { get; set; } = null!;

        [Required(ErrorMessage = "Department Name is required")]
        [Display(Name = "Department Name")]
        [StringLength(120, ErrorMessage = "Department Name can't be more than 120 characters.")]
        public string DepartmentName { get; set; } = null!;

        [Required(ErrorMessage = "Budget Description is required")]
        [Display(Name = "Budget Description")]
        [StringLength(200, ErrorMessage = "Budget Description can't be more than 200 characters.")]
        public string BudgetDescription { get; set; } = null!;

        [Required(ErrorMessage = "Head Count Budget is required")]
        [Display(Name = "Head Count Budget")]
        public int BudgetHeadCount { get; set; }

        [Display(Name = "Active Employees")]
        [ActiveCountValidation("BudgetHeadCount")] // ✅ custom validation
        public int ActiveCount { get; set; }

        [Display(Name = "Exit Employees")]
        public int ExitCount { get; set; }

        [Display(Name = "Active Requisition")]
        public int RequisitionCount { get; set; }

        [Display(Name = "Net Gap")]
        public int NetGapCount { get; set; }

        [Display(Name = "Net Indent")]
        public int NewIndentCount { get; set; }

        [Display(Name = "On-hold")]
        public bool OnHold { get; set; }

        [StringLength(120, ErrorMessage = "Remarks can't be more than 120 characters.")]
        public string? Remarks { get; set; } = null;

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; } = null;

        [Display(Name = "Last Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? LastUpdateDate { get; set; } = null;
        #endregion

        #region Extended Properties
        public string OnHoldDesc
        {
            get { return OnHold ? "Yes" : "No"; }
            set { }
        }
        #endregion

        #region Public Methods
        public void RecalculateNetGap()
        {
            NetGapCount = BudgetHeadCount - ((ActiveCount + RequisitionCount) - ExitCount);
        }
        #endregion
    }
}
