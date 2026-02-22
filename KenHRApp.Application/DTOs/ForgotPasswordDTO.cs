using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class ForgotPasswordDTO
    {
        #region Properties
        [Required(ErrorMessage = "User ID / Email is required")]
        [Display(Name = "User ID / Email")]
        [StringLength(50, ErrorMessage = "User ID / Email can't be more than 50 characters.")]
        public string EmployeeCode { get; set; } = null!;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Joining Date is required")]
        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; }
        #endregion
    }
}
