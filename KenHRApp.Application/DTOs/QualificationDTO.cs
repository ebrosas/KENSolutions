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
    public class QualificationDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string QualificationCode { get; set; } = null!;

        [Required(ErrorMessage = "Qualification is required")]
        [Display(Name = "Qualification")]
        public string QualificationDesc { get; set; } = null!;

        public string? StreamCode { get; set; } = null;
        public string? StreamDesc { get; set; } = null;
        public string? SpecializationCode { get; set; } = null;
        public string? SpecializationDesc { get; set; } = null;

        [Required(ErrorMessage = "University Name is required")]
        [StringLength(150, ErrorMessage = "University Name can't be more than 200 characters.")]
        [Display(Name = "University Name")]
        public string UniversityName { get; set; } = null!;

        public string? Institute { get; set; } = null;
        public string QualificationMode { get; set; } = null!;

        [Required(ErrorMessage = "Qualification Mode is required")]
        [Display(Name = "Qualification Mode")]
        public string QualificationModeDesc { get; set; } = null!;

        public string? CountryCode { get; set; } = null;
        public string? CountryDesc { get; set; } = null;
        public string? StateCode { get; set; } = null;
        public string? StateDesc { get; set; } = null;
        public string? CityTownName { get; set; } = null;
        public string FromMonthCode { get; set; } = null!;

        [Required(ErrorMessage = "From Month is required")]
        [Display(Name = "From Month")]
        public string FromMonthDesc { get; set; } = null!;

        [Required(ErrorMessage = "From Year is required")]
        [Display(Name = "From Year")]
        public int FromYear { get; set; }

        public string ToMonthCode { get; set; } = null!;

        [Required(ErrorMessage = "To Month is required")]
        [Display(Name = "To Month")]
        public string ToMonthDesc { get; set; } = null!;

        [Required(ErrorMessage = "To Year is required")]
        [Display(Name = "To Year")]
        public int ToYear { get; set; }

        public string PassMonthCode { get; set; } = null!;

        [Required(ErrorMessage = "Pass Month is required")]
        [Display(Name = "Pass Month")]
        public string PassMonthDesc { get; set; } = null!;

        [Required(ErrorMessage = "Pass Year is required")]
        [Display(Name = "Pass Year")]
        public int PassYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        public int EmployeeNo { get; set; }
        public int? TransactionNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
