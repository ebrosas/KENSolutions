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
    public class FamilyVisaDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public int FamilyId { get; set; }

        [Required(ErrorMessage = "Family Member is required")]
        [Display(Name = "Family Member")]
        public string FamilyMemberName { get; set; } = null!;

        public string CountryCode { get; set; } = null!;

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public string Country { get; set; } = null!;

        public string VisaTypeCode { get; set; } = null!;

        [Required(ErrorMessage = "Visa Type is required")]
        [Display(Name = "Visa Type")]
        public string VisaType { get; set; } = null!;

        [Required(ErrorMessage = "Profession is required")]
        [StringLength(150, ErrorMessage = "Profession can't be more than 150 characters.")]
        [Display(Name = "Profession")]
        public string Profession { get; set; } = null!;

        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        #endregion

        #region Reference Navigations 
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        public FamilyMember FamilyMember { get; set; } = null!;
        #endregion
    }
}
