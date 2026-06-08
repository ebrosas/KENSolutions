using KenHRApp.Application.Common.Validations;
using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class RegularRequestDTO
    {
        #region Properties
        public long RegularizationId { get; set; }       // Identity column  
        public Guid AttachmentId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = null!;

        [Display(Name = "Attendance Date")]
        [DataType(DataType.Date)]
        public DateTime? AttendanceDate { get; set; }

        public string? ROACode { get; set; } = null;

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description can't be more than 500 characters.")]
        public string ROADescription { get; set; } = null!;

        public string? ActionCode { get; set; } = null;

        [Display(Name = "Select Action")]
        public string ActionDescription { get; set; } = null!;

        [Required(ErrorMessage = "Regularized In Time is required")]
        [Display(Name = "Regularized In Time")]
        public TimeSpan? RegularizedTimeIn { get; set; }

        [Required(ErrorMessage = "Regularized Out Time is required")]
        [Display(Name = "Regularized Out Time")]
        public TimeSpan? RegularizedTimeOut { get; set; }

        [Display(Name = "Shift Name")]
        public string? ShiftPattern { get; set; } = null;

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        public string RegularizedDescription { get; set; } = null!;

        [Display(Name = "Shift Description")]
        public string? ShiftDescription { get; set; } = null;

        [Display(Name = "Scheduled Timing")]
        public string? ShiftTiming { get; set; } = null;

        [Display(Name = "Actual Timing")]
        public string? ActualTiming { get; set; } = null;

        public int? WorkDuration { get; set; }
        public int? NoPayHours { get; set; }
        public string? RemarkCode { get; set; } = null;

        public string StatusCode { get; set; } = null!;
        public string? StatusDesc { get; set; } = null;

        public int? StatusID { get; set; }

        [Display(Name = "Status")]
        public string? StatusHandlingCode { get; set; } = null;

        public string CostCenter { get; set; } = null!;
        public string? CostCenterName { get; set; } = null;

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }
        public string? CreatedUserID { get; set; } = null;
        public string? CreatedEmail { get; set; } = null;
        public string? CreatedByName { get; set; } = null;

        [Display(Name = "Last Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }
        public string? LastUpdatedUserID { get; set; } = null;
        public string? LastUpdatedEmail { get; set; } = null;
        public List<FileAttachmentDTO> Files { get; set; } = new();
        public List<AttendanceSwipeDTO>? SwipeLogList { get; set; } = new();
        #endregion

        #region Extended Properties
        [Display(Name = "Total Work Hours")]
        public string TotalWorkHours
        {
            get
            {
                if (WorkDuration > 0)
                {
                    TimeSpan duration = TimeSpan.FromMinutes(Convert.ToDouble(WorkDuration));
                    return $"{(int)duration.TotalHours:00}:{duration.Minutes:00}";
                }
                else
                    return "0";
            }
        }

        [Display(Name = "Deficit Hours")]
        public string TotalDeficitHours
        {
            get
            {
                if (NoPayHours > 0)
                {
                    TimeSpan duration = TimeSpan.FromMinutes(Convert.ToDouble(NoPayHours));
                    return $"{(int)duration.TotalHours:00}:{duration.Minutes:00}";
                }
                else
                    return "0";
            }
        }

        [Display(Name = "Shift Details")]
        public string ShiftDetails
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ShiftPattern) &&
                    !string.IsNullOrWhiteSpace(ShiftTiming))
                {
                    return $"{ShiftPattern} - {ShiftTiming}";
                }
                else
                    return "";
            }
        }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName
        {
            get
            {
                if (EmployeeNo > 0 &&
                    !string.IsNullOrWhiteSpace(EmployeeName))
                {
                    return $"{EmployeeNo} - {EmployeeName}";
                }
                else
                    return "";
            }
            set { }
        }

        [Display(Name = "Department")]
        public string DepartmentFullName
        {
            get
            {
                return $"{CostCenter} - {CostCenterName}";
            }
        }

        [Display(Name = "Status")]
        public string StatusSummary
        {
            get
            {
                if (StatusHandlingCode == "Open")
                    return $"{StatusHandlingCode} - {StatusDesc}";
                else
                    return StatusDesc!;
            }
        }
        #endregion
    }
}
