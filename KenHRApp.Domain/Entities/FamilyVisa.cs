using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class FamilyVisa
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string CountryCode { get; set; } = null!;

        [NotMapped]
        public string? Country { get; set; } = null;

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string VisaTypeCode { get; set; } = null!;

        [NotMapped]
        public string VisaType { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string Profession { get; set; } = null!;

        [Column(TypeName = "datetime"), Comment("Part of composite unique key index")]
        public DateTime? IssueDate { get; set; }

        [Column(TypeName = "datetime"), Comment("Part of composite unique key index")]
        public DateTime? ExpiryDate { get; set; }

        [NotMapped]
        public string FamilyMemberName { get; set; } = null!;
        #endregion

        #region Reference Navigations 
        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; } 
                
        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;

        [Comment("Foreign key that references primary key: FamilyMember.AutoId")]
        public int FamilyId { get; set; }

        // Navigation back to FamilyMember
        //public FamilyMember FamilyMember { get; set; } = null!;
        #endregion
    }
}
