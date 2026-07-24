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
    public class EmployeeSkillDTO
    {
        #region Properties
        public int AutoId { get; set; }

        [Required(ErrorMessage = "Skill Name is required")]
        [Display(Name = "Skill Name")]
        [StringLength(50, ErrorMessage = "Skill Name can't be more than 50 characters.")]
        public string SkillName { get; set; } = null!;

        public string? LevelCode { get; set; } = null;
        public string? LevelDesc { get; set; } = null;
        public string? LastUsedMonthCode { get; set; } = null;
        public string? LastUsedMonthDesc { get; set; } = null;

        [Display(Name = "Last User Year")]
        [Range(1900, 9999, ErrorMessage = "Last Used Year must be between 1900 and 9999")]
        public int? LastUsedYear { get; set; }

        public string? FromMonthCode { get; set; } = null;
        public string? FromMonthDesc { get; set; } = null;

        [Display(Name = "From Year")]
        [Range(1900, 9999, ErrorMessage = "From Year must be between 1900 and 9999")]
        public int? FromYear { get; set; }

        public string? ToMonthCode { get; set; } = null;
        public string? ToMonthDesc { get; set; } = null;

        [Display(Name = "To Year")]
        [Range(1900, 9999, ErrorMessage = "To Year must be between 1900 and 9999")]
        public int? ToYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
