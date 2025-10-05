using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class EmployeeCertification
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(20)"), Comment("Part of unique composite key index")]
        public string QualificationCode { get; set; } = null!;

        [NotMapped]
        public string QualificationDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? StreamCode { get; set; } = null;

        [NotMapped]
        public string? StreamDesc { get; set; } = null;

        [Column(TypeName = "varchar(150)")]
        public string Specialization { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string University { get; set; } = null!;

        [Column(TypeName = "varchar(100)")]
        public string? Institute { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? CountryCode { get; set; } = null;

        [NotMapped]
        public string? Country { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? State { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? CityTownName { get; set; } = null;

        [Column(TypeName = "varchar(20)"), Comment("Part of unique composite key index")]
        public string FromMonthCode { get; set; } = null!;

        [NotMapped]
        public string? FromMonth { get; set; } = null;

        [Comment("Part of unique composite key index")]
        public int FromYear { get; set; }

        [Column(TypeName = "varchar(20)"), Comment("Part of unique composite key index")]
        public string ToMonthCode { get; set; } = null!;

        [NotMapped]
        public string? ToMonth { get; set; } = null;

        [Comment("Part of unique composite key index")]
        public int ToYear { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PassMonthCode { get; set; } = null!;

        [NotMapped]
        public string? PassMonth { get; set; } = null;

        public int? PassYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        [Comment("Foreign key that references Employee.EmployeeNo alternate key")]
        public int EmployeeNo { get; set; }

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
