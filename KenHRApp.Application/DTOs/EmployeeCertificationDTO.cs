using KenHRApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class EmployeeCertificationDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string QualificationCode { get; set; } = null!;

        [Required(ErrorMessage = "Qualification is required")]
        [Display(Name = "Qualification")]
        public string QualificationDesc { get; set; } = null!;

        public string? StreamCode { get; set; } = null;
        public string? StreamDesc { get; set; } = null;

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(150, ErrorMessage = "Specialization can't be more than 150 characters.")]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; } = null!;

        [Required(ErrorMessage = "University is required")]
        [StringLength(150, ErrorMessage = "University can't be more than 150 characters.")]
        [Display(Name = "University")]
        public string University { get; set; } = null!;

        public string? Institute { get; set; } = null;
        public string? CountryCode { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? State { get; set; } = null;
        public string? CityTownName { get; set; } = null;

        public string FromMonthCode { get; set; } = null!;

        [Required(ErrorMessage = "From Month is required")]
        [Display(Name = "From Month")]
        public string? FromMonth { get; set; } = null;

        [Required(ErrorMessage = "From Year is required")]
        [Display(Name = "From Year")]
        [Range(1900, 9999, ErrorMessage = "From Year must be between 1900 and 9999")]
        public int FromYear { get; set; }

        public string ToMonthCode { get; set; } = null!;

        [Required(ErrorMessage = "From Month is required")]
        [Display(Name = "From Month")]
        public string? ToMonth { get; set; } = null;

        [Required(ErrorMessage = "To Year is required")]
        [Display(Name = "To Year")]
        [Range(1900, 9999, ErrorMessage = "From Year must be between 1900 and 9999")]
        public int ToYear { get; set; }

        public string PassMonthCode { get; set; } = null!;

        [Required(ErrorMessage = "Pass Month is required")]
        [Display(Name = "Pass Month")]
        public string? PassMonth { get; set; } = null;

        [Required(ErrorMessage = "Pass Year is required")]
        [Display(Name = "Pass Year")]
        [Range(1900, 9999, ErrorMessage = "Pass Year must be between 1900 and 9999")]
        public int? PassYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
