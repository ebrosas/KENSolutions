using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class FamilyMember
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(50)"), Comment("Part of composite unique key index")]
        public string FirstName { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string? MiddleName { get; set; } = null;

        [Column(TypeName = "varchar(50)"), Comment("Part of composite unique key index")]
        public string LastName { get; set; } = null!;

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string RelationCode { get; set; } = null!;

        [NotMapped]
        public string? Relation { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? DOB { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? QualificationCode { get; set; } = null;

        [NotMapped]
        public string? Qualification { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? StreamCode { get; set; } = null;

        [NotMapped]
        public string? StreamDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? SpecializationCode { get; set; } = null;

        [NotMapped]
        public string? Specialization { get; set; } = null;

        [Column(TypeName = "varchar(120)")]
        public string? Occupation { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? ContactNo { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? CountryCode { get; set; } = null;

        [NotMapped]
        public string? Country { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? StateCode { get; set; } = null;

        [NotMapped]
        public string? State { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? CityTownName { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? District { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? IsDependent { get; set; } = null;
        #endregion

        #region Reference Navigation to Employee   
        //public ICollection<FamilyVisa> FamilyVisaList { get; set; } = new List<FamilyVisa>();

        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; } 

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
