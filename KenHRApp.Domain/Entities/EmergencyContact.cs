using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class EmergencyContact
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ContactPerson { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string RelationCode { get; set; } = null!;

        [NotMapped]
        public string? Relation { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string MobileNo { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? LandlineNo { get; set; } = null;

        [Column(TypeName = "varchar(300)")]
        public string? Address { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? CountryCode { get; set; } = null;

        [NotMapped]
        public string? CountryDesc { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? City { get; set; } = null;
        #endregion

        #region Reference Navigation to Employee   
        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; }

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
