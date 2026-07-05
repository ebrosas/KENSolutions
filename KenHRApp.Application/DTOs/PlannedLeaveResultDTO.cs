using KenHRApp.Application.Common.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class PlannedLeaveResultDTO
    {
        #region Constructors
        public PlannedLeaveResultDTO()
        {
            LeaveDuration = 0;
            NoOfHolidays = 0;
            NoOfWeekends = 0;
        }

        public PlannedLeaveResultDTO(int empNo, string empName, string empEmail, string costCenter)
        {
            EmpNo = empNo;
            EmpName = empName;
            CostCenter = costCenter;
        }
        #endregion

        #region Properties
        public long PlannedLeaveId { get; set; }       // Identity column  
        public long LeaveNo { get; set; }
        public int EmpNo { get; set; }
        public string? EmpName { get; set; } = null;

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Start Date is required")]
        [LeaveDateValidation("LeaveResumeDate")] // ✅ custom validation
        public DateTime? LeaveStartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? LeaveEndDate { get; set; }

        [Display(Name = "Resume Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Resume Date is required")]
        [LeaveDateValidation("LeaveStartDate")] // ✅ custom validation
        public DateTime? LeaveResumeDate { get; set; }
        
        public string? StartDayMode { get; set; } = null;

        [Display(Name = "Mode")]
        public string? StartDayModeDesc { get; set; } = null;

        public string? EndDayMode { get; set; } = null;

        [Display(Name = "Mode")]
        public string? EndDayModeDesc { get; set; } = null;

        public string? CostCenter { get; set; } = null;
        public string? CostCenterName { get; set; } = null;

        [Label("Remarks")]
        [StringLength(500, ErrorMessage = "Remarks can't be more than 500 characters.")]
        public string? Remarks { get; set; } = null;

        [Display(Name = "Leave Duration")]
        public double? LeaveDuration { get; set; }

        [Display(Name = "No. of Holidays")]
        public int? NoOfHolidays { get; set; }

        [Display(Name = "No. of Weekends")]
        public int? NoOfWeekends { get; set; }

        public char? HalfDayLeaveFlag { get; set; }
        public int? StatusID { get; set; }
        public string StatusCode { get; set; } = null!;
        public string? StatusDesc { get; set; } = null;
        public string? StatusHandlingCode { get; set; } = null;

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; } = null;
        public string? CreatedUserID { get; set; } = null;
        public string? CreatedEmail { get; set; } = null;

        [Display(Name = "Last Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }
        public string? LastUpdatedName { get; set; } = null;
        public string? LastUpdatedUserID { get; set; } = null;
        public string? LastUpdatedEmail { get; set; } = null;
        #endregion
    }
}
