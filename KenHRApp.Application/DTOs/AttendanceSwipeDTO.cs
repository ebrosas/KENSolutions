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
        public long SwipeID { get; set; }

        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int EmpNo { get; set; }

        [Required(ErrorMessage = "Swipe Date is required")]
        [Display(Name = "Swipe Date")]
        public DateTime SwipeDate { get; set; }

        [Required(ErrorMessage = "Swipe Time is required")]
        [Display(Name = "Swipe Time")]
        public DateTime? SwipeTime { get; set; }

        public string? SwipeType { get; set; }        
        public string? LocationCode { get; set; } = null;
        public string? LocationName { get; set; } = null;
        public string? ReaderCode { get; set; } = null;
        public string? ReaderName { get; set; } = null;
        public string? StatusCode { get; set; } = null;
        public DateTime? SwipeLogDate { get; set; } 
        #endregion
    }
}
