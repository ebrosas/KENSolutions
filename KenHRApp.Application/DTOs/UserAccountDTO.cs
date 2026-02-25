using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class UserAccountDTO
    {
        #region Properties
        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int? EmployeeNo { get; set; }

        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime? DOJ { get; set; } = null;

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        [StringLength(50, ErrorMessage = "User ID can't be more than 50 characters.")]
        public string UserID { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Email can't be more than 50 characters.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [StringLength(40, ErrorMessage = "Password can't be more than 40 characters.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Retype Password is required")]
        [Display(Name = "Retype Password")]
        [StringLength(40, ErrorMessage = "Retype Password can't be more than 40 characters.")]
        public string RetypePassword { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [StringLength(200, ErrorMessage = "Password can't be more than 200 characters.")]
        public string PasswordHash { get; set; } = null!;

        [Display(Name = "Security Question #1")]
        [StringLength(150, ErrorMessage = "Security Question #1 can't be more than 150 characters.")]
        public string SecurityQuestion1 { get; set; } = null!;

        [Display(Name = "Security Answer #1")]
        [StringLength(50, ErrorMessage = "Security Answer #1 can't be more than 50 characters.")]
        public string SecurityAnswer1 { get; set; } = null!;

        [Display(Name = "Security Question #2")]
        [StringLength(150, ErrorMessage = "Security Question #2 can't be more than 150 characters.")]
        public string SecurityQuestion2 { get; set; } = null!;

        [Display(Name = "Security Answer #2")]
        [StringLength(50, ErrorMessage = "Security Answer #2 can't be more than 50 characters.")]
        public string SecurityAnswer2 { get; set; } = null!;

        [Display(Name = "Security Question #3")]
        [StringLength(150, ErrorMessage = "Security Question #3 can't be more than 150 characters.")]
        public string SecurityQuestion3 { get; set; } = null!;

        [Display(Name = "Security Answer #3")]
        [StringLength(50, ErrorMessage = "Security Answer #3 can't be more than 50 characters.")]
        public string SecurityAnswer3 { get; set; } = null!;
        #endregion
    }
}
