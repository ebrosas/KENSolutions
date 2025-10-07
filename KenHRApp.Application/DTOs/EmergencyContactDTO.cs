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
    public class EmergencyContactDTO
    {
        #region Properties
        public int AutoId { get; set; }

        [Required(ErrorMessage = "Contact Person is required")]
        [Display(Name = "Contact Person")]
        [StringLength(200, ErrorMessage = "Contact Person can't be more than 200 characters.")]
        public string ContactPerson { get; set; } = null!;

        public string RelationCode { get; set; } = null!;

        [Required(ErrorMessage = "Relationship is required")]
        [Display(Name = "Relationship")]
        public string? Relation { get; set; } = null;

        [Required(ErrorMessage = "Mobile No. is required")]
        [Display(Name = "Mobile No.")]
        [StringLength(20, ErrorMessage = "Mobile No. can't be more than 20 characters.")]
        public string MobileNo { get; set; } = null!;

        [Display(Name = "Landline No.")]
        [StringLength(20, ErrorMessage = "Landline No. can't be more than 20 characters.")]
        public string? LandlineNo { get; set; } = null;

        [Display(Name = "Address")]
        [StringLength(300, ErrorMessage = "Address can't be more than 300 characters.")]
        public string? Address { get; set; } = null;

        public string? CountryCode { get; set; } = null;

        [Display(Name = "Country")]
        public string? CountryDesc { get; set; } = null;

        [Display(Name = "City")]
        [StringLength(100, ErrorMessage = "City can't be more than 100 characters.")]
        public string? City { get; set; } = null;
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
