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
        public long RegularizationRequestId { get; set; }       // Identity column  

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? AttendanceDate { get; set; }

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
        #endregion
    }
}
