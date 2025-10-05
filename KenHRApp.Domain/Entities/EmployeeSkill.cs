using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class EmployeeSkill
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string SkillName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? LevelCode { get; set; } = null;

        [NotMapped]
        public string? LevelDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? LastUsedMonthCode { get; set; } = null;

        [NotMapped]
        public string? LastUsedMonthDesc { get; set; } = null;

        public int? LastUsedYear { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? FromMonthCode { get; set; } = null;

        [NotMapped]
        public string? FromMonthDesc { get; set; } = null;

        public int? FromYear { get; set; } 

        [Column(TypeName = "varchar(20)")]
        public string? ToMonthCode { get; set; } = null;

        [NotMapped]
        public string? ToMonthDesc { get; set; } = null;

        public int? ToYear { get; set; }
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
