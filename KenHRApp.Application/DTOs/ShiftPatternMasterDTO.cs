using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class ShiftPatternMasterDTO
    {
        #region Properties
        public int ShiftPatternId { get; set; }

        [Required(ErrorMessage = "Shift Roster Code is required")]
        [Display(Name = "Shift Roster Code")]
        [StringLength(20, ErrorMessage = "Shift Roster Code can't be more than 20 characters.")]
        public string ShiftPatternCode { get; set; } = null!;

        [Display(Name = "Shift Roster Description")]
        [StringLength(300, ErrorMessage = "Shift Roster Description can't be more than 300 characters.")]
        public string? ShiftPatternDescription { get; set; } = null;

        public bool IsActive { get; set; }
        public bool? IsDayShift { get; set; }
        public bool? IsFlexiTime { get; set; }
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
        public string IsActiveDesc
        {
            get { return IsActive ? "Yes" : "No"; }
            set { }
        }

        public string IsFlexiTimeDesc
        {
            get { return IsFlexiTime!.Value ? "Yes" : "No"; }
            set { }
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
        #endregion

        #region Reference Navigations
        public List<ShiftTimingDTO> ShiftTimingList { get; set; } = new List<ShiftTimingDTO>();
        public List<ShiftPointerDTO> ShiftPointerList { get; set; } = new List<ShiftPointerDTO>();
        #endregion
    }
}
