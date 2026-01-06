using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class EmployeeRosterDTO
    {
        #region Properties
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Gender { get; set; } = null;
        public DateTime? HireDate { get; set; } = null;
        public string? EmploymentTypeCode { get; set; } = null;
        public string? EmploymentType { get; set; } = null;
        public string? ReportingManagerCode { get; set; } = null;
        public string? ReportingManager { get; set; } = null;
        public string? DepartmentCode { get; set; } = null;
        public string? DepartmentName { get; set; } = null;
        public string? EmployeeStatusCode { get; set; } = null;
        public string? EmployeeStatus { get; set; } = null;

        [Required(ErrorMessage = "Effective Date is required")]
        [Display(Name = "Effective Date")]
        public DateTime? EffectiveDate { get; set; }

        public DateTime? EndingDate { get; set; } = null;

        [Required(ErrorMessage = "Shift Roster Code is required")]
        [Display(Name = "Shift Roster Code")]
        [StringLength(20, ErrorMessage = "Shift Roster Code can't be more than 20 characters.")]
        public string ShiftPatternCode { get; set; } = null!;

        public string? ShiftPatternDescription { get; set; } = null;

        [Required(ErrorMessage = "Shift Pointer is required")]
        [Display(Name = "Shift Pointer")]
        public int ShiftPointer { get; set; }

        public int ShiftPointerId { get; set; }

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
        #endregion

        #region Extended Properties
        public string EmployeeName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public string DepartmentFullName
        {
            get { return $"{DepartmentCode} - {DepartmentName}"; }
        }

        public string CreatedByFullName
        {
            get { return $"{CreatedByEmpNo} - {CreatedByName}"; }
        }

        public string LastUpdateByFullName
        {
            get { return $"{LastUpdateEmpNo} - {LastUpdatedByName}"; }
        }
        #endregion
    }
}
