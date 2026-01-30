using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class AttendanceTimesheetDTO
    {
        #region Properties
        public double TimesheetId { get; set; }

        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int EmpNo { get; set; }

        public string CostCenter { get; set; } = null!;
        public string? PayGrade { get; set; }

        [Required(ErrorMessage = "Attendance Date is required")]
        [Display(Name = "Attendance Date")]
        public DateTime AttendanceDate { get; set; }

        [Display(Name = "Time In")]
        public DateTime? TimeIn { get; set; }

        [Display(Name = "Time In")]
        public DateTime? TimeOut { get; set; }

        public DateTime? ShavedIn { get; set; }
        public DateTime? ShavedOut { get; set; }
        public string? OTType { get; set; } = null!;
        public DateTime? OTStartTime { get; set; }
        public DateTime? OTEndTime { get; set; }
        public string? ShiftPatCode { get; set; } = null!;
        public string? SchedShiftCode { get; set; } = null!;
        public string? ActualShiftCode { get; set; } = null!;
        public int? NoPayHours { get; set; }
        public string? RemarkCode { get; set; } = null!;
        public string? AbsenceReasonCode { get; set; } = null!;
        public string? AbsenceReasonColumn { get; set; } = null!;
        public string? LeaveType { get; set; } = null!;
        public string? DIL_Entitlement { get; set; } = null!;
        public string? CorrectionCode { get; set; } = null!;
        public bool? Processed { get; set; }
        public int? ProcessID { get; set; }
        public int? UploadID { get; set; }
        public bool? IsPublicHoliday { get; set; }
        public bool? IsDILHoliday { get; set; }
        public bool? IsRamadan { get; set; }
        public bool? IsMuslim { get; set; }
        public bool? IsDriver{ get; set; }
        public bool? IsDILDayWorker { get; set; }
        public bool? IsSalaryStaff { get; set; }
        public bool? IsDayWorkerOrShifter { get; set; }
        public bool? IsLiasonOfficer { get; set; }
        public bool? IsLastRow { get; set; }
        public int? DurationRequired { get; set; }
        public int? DurationWorked { get; set; }
        public int? DurationWorkedCumulative { get; set; }
        public int? NetMinutes { get; set; }
        public int? CreatedByEmpNo { get; set; }        
        public string? CreatedByUserID { get; set; } = null;
        public DateTime? CreatedDate { get; set; } = null;
        public DateTime? LastUpdateDate { get; set; } = null;
        public int? LastUpdateEmpNo { get; set; }
        public string? LastUpdateUserID { get; set; } = null;        
        #endregion
    }
}
