using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class ShiftPatternChangeDTO
    {
        #region Properties
        public int ShiftPatternChangeID { get; set; }
        public int EmpNo { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndingDate { get; set; } = null;

        [Required(ErrorMessage = "Shift Roster Code is required")]
        [Display(Name = "Shift Roster Code")]
        [StringLength(20, ErrorMessage = "Shift Roster Code can't be more than 20 characters.")]
        public string ShiftPatternCode { get; set; } = null!;
        #endregion
    }
}
