using KenHRApp.Application.Common.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs.TNA
{
    public class OutdoorRequestDTO
    {
        #region Properties
        public long OutdoorId { get; set; }       // Identity column  
        public Guid AttachmentId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public int EmpNo { get; set; }
        public string EmpName { get; set; } = null!;

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "End Date is required")]
        public DateTime? EndDate { get; set; }

        public string? ROACode { get; set; } = null;

        [Display(Name = "Outdoor Type")]
        [Required(ErrorMessage = "Outdoor Type is required")]
        public string ROADesc { get; set; } = null!;

        public string? DOWCode { get; set; } = null;

        [Display(Name = "Day of Week")]
        public string? DOWDesc { get; set; } = null;

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can't be more than 500 characters.")]
        public string Description { get; set; } = null!;

        public string? ActionCode { get; set; } = null;

        [Display(Name = "Select Action")]
        [Required(ErrorMessage = "Action is required")]
        public string ActionDesc { get; set; } = null!;

        [Display(Name = "Start Time")]
        [OutdoorTimeValidation("EndTime")] // ✅ custom validation
        public TimeSpan? StartTime { get; set; }

        [Display(Name = "End Time")]
        [OutdoorTimeValidation("StartTime")] // ✅ custom validation
        public TimeSpan? EndTime { get; set; }

        public int? StatusID { get; set; }
        public string StatusCode { get; set; } = null!;
        public string? StatusDesc { get; set; } = null;

        [Display(Name = "Status")]
        public string? StatusHandlingCode { get; set; } = null;

        public string? CostCenter { get; set; } = null;
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
        public int? ApproverNo { get; set; }
        public string? ApproverName { get; set; } = null;

        public List<FileAttachmentDTO> Files { get; set; } = new();
        #endregion

        #region Extended Properties
        [Display(Name = "Employee Name")]
        public string EmployeeFullName
        {
            get
            {
                if (EmpNo > 0 &&
                    !string.IsNullOrWhiteSpace(EmpName))
                {
                    return $"{EmpNo} - {EmpName}";
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

        [Display(Name = "Current Approver")]
        public string CurrentApprover
        {
            get
            {
                if (ApproverNo > 0 &&
                    !string.IsNullOrWhiteSpace(ApproverName))
                {
                    return $"{ApproverNo} - {ApproverName}";
                }
                else
                    return "";
            }
            set { }
        }
        #endregion
    }
}
