using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class ShiftPatternChangeDTO
    {
        #region Properties
        public int ShiftPatternChangeId { get; set; }

        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int EmpNo { get; set; }

        public string? EmpName { get; set; } = null;
        public string? Position { get; set; } = null;
        public string? CostCenter { get; set; } = null;
        public string? CostCenterName { get; set; } = null;

        [Required(ErrorMessage = "Effective Date is required")]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; }

        public DateTime? EndingDate { get; set; } = null;

        [Required(ErrorMessage = "Shift Roster Code is required")]
        [Display(Name = "Shift Roster Code")]
        [StringLength(20, ErrorMessage = "Shift Roster Code can't be more than 20 characters.")]
        public string ShiftPatternCode { get; set; } = null!;

        [Required(ErrorMessage = "Shift Pointer is required")]
        [Display(Name = "Shift Pointer")]
        public int ShiftPointer { get; set; }

        [Required(ErrorMessage = "Change Type is required")]
        [Display(Name = "Change Type")]
        public string ChangeTypeCode { get; set; } = null!;

        public string? ChangeTypeDesc { get; set; } = null;

        public int? CreatedByEmpNo { get; set; }
        public string? CreatedByName { get; set; } = null;
        public string? CreatedByUserID { get; set; } = null;
        public DateTime? CreatedDate { get; set; } = null;
        public DateTime? LastUpdateDate { get; set; } = null;
        public int? LastUpdateEmpNo { get; set; }
        public string? LastUpdateUserID { get; set; } = null;
        public string? LastUpdatedByName { get; set; } = null;
        public string? DepartmentCode { get; set; } = null;
        public string? DepartmentName { get; set; } = null;
        #endregion

        #region Extended Properties
        public string CostCenterFullName
        {
            get { return $"{CostCenter} - {CostCenterName}"; }
        }

        public string CreatedByFullName
        {
            get
            {
                if (CreatedByEmpNo.HasValue && !string.IsNullOrEmpty(CreatedByName))
                    return $"{CreatedByEmpNo} - {CreatedByName}";
                else
                    return string.Empty;
            }
        }

        public string LastUpdatedByFullName
        {
            get
            {
                if (LastUpdateEmpNo.HasValue && !string.IsNullOrEmpty(LastUpdatedByName))
                    return $"{LastUpdateEmpNo} - {LastUpdatedByName}";
                else
                    return string.Empty;
            }
        }

        public string DepartmentFullName
        {
            get
            {
                if (!string.IsNullOrEmpty(DepartmentCode) && !string.IsNullOrEmpty(DepartmentName))
                    return $"{DepartmentCode} - {DepartmentName}";
                else
                    return string.Empty;
            }
        }
        #endregion
    }
}
