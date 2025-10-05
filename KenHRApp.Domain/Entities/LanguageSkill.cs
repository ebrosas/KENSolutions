using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class LanguageSkill
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string LanguageCode { get; set; } = null!;

        [NotMapped]
        public string? LanguageDesc { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? CanWrite { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? CanSpeak { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? CanRead { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? MotherTongue { get; set; } = null;
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
