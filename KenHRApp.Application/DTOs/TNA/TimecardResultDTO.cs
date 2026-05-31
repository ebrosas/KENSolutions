using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs.TNA
{
    public class TimecardResultDTO
    {
        #region Properties
        public long AutoId { get; set; }       // Identity column  
        public string CostCenter { get; set; } = null!;
        public string? CostCenterName { get; set; } = null;
        public int EmpNo { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string? Position { get; set; } = null;
        public DateTime? AttendanceDate { get; set; } = null;
        public string? DOW { get; set; } = null;
        public string? ShiftPatCode { get; set; } = null;
        public string? SchedShiftCode { get; set; } = null;
        public string? ShiftDescription { get; set; } = null;
        public string? ShiftTiming { get; set; }
        public DateTime? FirstTimeIn { get; set; } = null;
        public DateTime? LastTimeOut { get; set; } = null;
        public int? DurationRequired { get; set; }
        public int? WorkDuration { get; set; }
        public int? NoPayHours { get; set; }
        public string? RemarkCode { get; set; } = null;
        public string? LeaveType { get; set; } = null;
        public string? AbsenceReasonCode { get; set; } = null;
        public string? ROADesc { get; set; } = null;
        public string? AttendanceStatus { get; set; } = null;
        public string? AttendanceRemarks { get; set; } = null;
        public int? OTDuration { get; set; }
        public string? OTStatus { get; set; } = null;
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
                    return "";
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
                    return "";
            }
        }

        [Display(Name = "Overtime Duration")]
        public string TotalOTDuration
        {
            get
            {
                if (OTDuration > 0)
                {
                    TimeSpan duration = TimeSpan.FromMinutes(Convert.ToDouble(OTDuration));
                    return $"{(int)duration.TotalHours:00}:{duration.Minutes:00}";
                }
                else
                    return "";
            }
        }

        [Display(Name = "Scheduled Shift")]
        public string ShiftDetails
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SchedShiftCode) &&
                    !string.IsNullOrWhiteSpace(ShiftTiming))
                {
                    return $"{ShiftPatCode} - {ShiftTiming}";
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
                if (EmpNo > 0 &&
                    !string.IsNullOrWhiteSpace(EmployeeName))
                {
                    return $"{EmpNo} - {EmployeeName}";
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
        #endregion
    }
}
