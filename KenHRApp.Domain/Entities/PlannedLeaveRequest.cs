using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class PlannedLeaveRequest
    {
        #region Constructors
        public PlannedLeaveRequest() { }
        #endregion

        #region Enums
        public enum LeaveDayMode
        {
            FullDay = 1,
            FirstHalf = 2,
            SecondHalf = 3
        }
        #endregion

        #region Properties
        public long PlannedLeaveId { get; set; }       // Identity column  
        public int EmpNo { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? EmpName { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime LeaveStartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime LeaveEndDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime LeaveResumeDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? CostCenter { get; set; } = null;

        [Column(TypeName = "varchar(500)")]
        public string? Remarks { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? StartDayMode { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? EndDayMode { get; set; } = null;

        public double? LeaveDuration { get; set; }
        public int? NoOfHolidays { get; set; }
        public int? NoOfWeekends { get; set; }
        
        [Column(TypeName = "char(1)")]
        public char? HalfDayLeaveFlag { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? CreatedByName { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUserID { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? CreatedEmail { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? LastUpdatedName { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedUserID { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedEmail { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string StatusCode { get; set; } = null!;
        public int? StatusID { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? StatusHandlingCode { get; set; } = null;
        #endregion
    }
}
