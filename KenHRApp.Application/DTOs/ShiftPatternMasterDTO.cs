using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        #endregion

        #region Reference Navigations
        public List<ShiftTimingDTO> ShiftTimingList { get; set; } = new List<ShiftTimingDTO>();
        public List<ShiftPointerDTO> ShiftPointerList { get; set; } = new List<ShiftPointerDTO>();
        #endregion
    }
}
