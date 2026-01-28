using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class AttendanceSwipeDTO
    {
        #region Properties
        public double SwipeID { get; set; }

        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int EmpNo { get; set; }

        [Required(ErrorMessage = "Attendance Date is required")]
        [Display(Name = "Attendance Date")]
        public DateTime AttendanceDate { get; set; }

        [Required(ErrorMessage = "Punch Time is required")]
        [Display(Name = "Punch Time")]
        public DateTime? PunchTime { get; set; }

        public DateTime? CreatedDate { get; set; } = null;
        #endregion
    }
}
