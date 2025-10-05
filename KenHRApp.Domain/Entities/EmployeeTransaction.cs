using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class EmployeeTransaction
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string ActionCode { get; set; } = null!;

        [NotMapped]
        public string ActionDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string StatusCode { get; set; } = null!;

        [NotMapped]
        public string Status { get; set; } = null!;

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string SectionCode { get; set; } = null!;

        [NotMapped]
        public string Section { get; set; } = null!;

        [Column(TypeName = "datetime"), Comment("Part of composite unique key index")]
        public DateTime? LastUpdateOn { get; set; }

        public int? CurrentlyAssignedEmpNo { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string? CurrentlyAssignedEmpName { get; set; } = null!;
        #endregion

        #region Reference Navigations
        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; } 

        [Comment("Foreign key that references employee transaction. Part of composite key.")]
        public int TransactionNo { get; set; }

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
