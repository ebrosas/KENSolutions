using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class Qualification
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string QualificationCode { get; set; } = null!;

        [NotMapped]
        public string QualificationDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? StreamCode { get; set; } = null;

        [NotMapped]
        public string? StreamDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? SpecializationCode { get; set; } = null;
        
        [NotMapped]
        public string? SpecializationDesc { get; set; } = null;

        [Column(TypeName = "varchar(200)")]
        public string UniversityName { get; set; } = null!;

        [Column(TypeName = "varchar(100)")]
        public string? Institute { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string QualificationMode { get; set; } = null!;

        [NotMapped]
        public string QualificationModeDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? CountryCode { get; set; } = null;

        [NotMapped]
        public string? CountryDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? StateCode { get; set; } = null;

        [NotMapped]
        public string? StateDesc { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? CityTownName { get; set; } = null;
                
        [Column(TypeName = "varchar(20)")]
        public string FromMonthCode { get; set; } = null!;
        
        [NotMapped]
        public string FromMonthDesc { get; set; } = null!;

        public int FromYear { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string ToMonthCode { get; set; } = null!;
        
        [NotMapped]
        public string ToMonthDesc { get; set; } = null!;

        public int ToYear { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PassMonthCode { get; set; } = null!;

        [NotMapped]
        public string PassMonthDesc { get; set; } = null!;

        public int PassYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; }

        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
