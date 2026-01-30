using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class AttendanceTimesheet
    {
        #region Properties
        public double TimesheetId { get; set; }

        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int EmpNo { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CostCenter { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? PayGrade { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime AttendanceDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TimeIn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TimeOut { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ShavedIn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ShavedOut { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? OTType { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime? OTStartTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? OTEndTime { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? ShiftPatCode { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? SchedShiftCode { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? ActualShiftCode { get; set; } = null!;

        public int? NoPayHours { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? RemarkCode { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? AbsenceReasonCode { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? AbsenceReasonColumn { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? LeaveType { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? DIL_Entitlement { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? CorrectionCode { get; set; } = null!;

        [Column(TypeName = "bit")]
        public bool? Processed { get; set; }

        public int? ProcessID { get; set; }
        public int? UploadID { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsPublicHoliday { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsDILHoliday { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsRamadan { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsMuslim { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsDriver { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsDILDayWorker { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsSalaryStaff { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsDayWorkerOrShifter { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsLiasonOfficer { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsLastRow { get; set; }
        public int? DurationRequired { get; set; }
        public int? DurationWorked { get; set; }
        public int? DurationWorkedCumulative { get; set; }
        public int? NetMinutes { get; set; }
        public int? CreatedByEmpNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedByUserID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; } = null;

        public int? LastUpdateEmpNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdateUserID { get; set; } = null;
        #endregion
    }
}
