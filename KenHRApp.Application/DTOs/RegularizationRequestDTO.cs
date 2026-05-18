using KenHRApp.Application.Common.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class RegularizationRequestDTO
    {
        #region Properties
        public long RegularizedRequestId { get; set; }       // Identity column  

        [Display(Name = "Attendance Date")]
        [DataType(DataType.Date)]
        public DateTime? AttendanceDate { get; set; }

        public string? ReasonCode { get; set; } = null;

        [Display(Name = "Reason")]
        public string ReasonDescription { get; set; } = null!;

        [Required(ErrorMessage = "Regularized In Time is required")]
        [Display(Name = "Regularized In Time")]
        public TimeSpan? RegularizedTimeIn { get; set; }

        [Required(ErrorMessage = "Regularized Out Time is required")]
        [Display(Name = "Regularized Out Time")]
        public TimeSpan? RegularizedTimeOut { get; set; }

        [Display(Name = "Shift Name")]
        public string? ShiftPattern { get; set; } = null;

        [Display(Name = "Description")]
        public string? Description { get; set; } = null;

        [Display(Name = "Shift Description")]
        public string? ShiftDescription { get; set; } = null;

        [Display(Name = "Scheduled Timing")]
        public string? ShiftTiming { get; set; } = null;

        [Display(Name = "Actual Timing")]
        public string? ActualTiming { get; set; } = null;

        public int WorkDuration { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public string? CreatedUserID { get; set; } = null;

        public string? CreatedEmail { get; set; } = null;

        [Display(Name = "Last Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        public string? LastUpdatedUserID { get; set; } = null;

        public string? LastUpdatedEmail { get; set; } = null;

        public List<FileAttachmentDTO> Files { get; set; } = new();
        #endregion

        #region Extended Properties
        [Display(Name = "Total Work Hours")]
        public string TotalWorkHours
        {
            get
            {
                if (WorkDuration > 0)
                {
                    TimeSpan duration = TimeSpan.FromMinutes(WorkDuration);
                    return $"{(int)duration.TotalHours:00}:{duration.Minutes:00}";
                }
                else
                    return "0";
            }
        }
        #endregion
    }
}
