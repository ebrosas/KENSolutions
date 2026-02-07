using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class LeaveRequisitionWF
    {
        #region Properties
        public long LeaveRequestId { get; set; }       // Identity column  

        [Column(TypeName = "varchar(50)")]
        public string? LeaveInstanceID { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string LeaveType { get; set; } = null!;

        public int LeaveEmpNo { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? LeaveEmpName { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? LeaveEmpEmail { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime LeaveStartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime LeaveEndDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime LeaveResumeDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? LeaveEmpCostCenter { get; set; } = null;

        [Column(TypeName = "varchar(500)")]
        public string? LeaveRemarks { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? LeaveConstraints { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string LeaveStatusCode { get; set; } = null!;

        [Column(TypeName = "char(1)")]
        public char? LeaveApprovalFlag { get; set; } 
        
        [Column(TypeName = "bit")]
        public bool? LeaveVisaRequired { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? LeavePayAdv { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? LeaveIsFTMember { get; set; } = null;

        public double? LeaveBalance { get; set; }
        public double? LeaveDuration { get; set; }
        public int? NoOfHolidays { get; set; }
        public int? NoOfWeekends { get; set; }
        
        [Column(TypeName = "char(1)")]
        public char? PlannedLeave { get; set; }

        public int? LeavePlannedNo { get; set; }

        [Column(TypeName = "char(1)")]
        public char? HalfDayLeaveFlag { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LeaveCreatedDate { get; set; }

        public int? LeaveCreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LeaveCreatedUserID { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? LeaveCreatedEmail { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LeaveUpdatedDate { get; set; }

        public int? LeaveUpdatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LeaveUpdatedUserID { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? LeaveUpdatedEmail { get; set; } = null;
        #endregion
    }
}
