using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class UserDefinedCode
    {
        #region Properties
        [Comment("Primary key of UserDefinedCode entity")]
        public int UDCId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string UDCCode { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string UDCDesc1 { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string? UDCDesc2 { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? UDCSpecialHandlingCode { get; set; } = null;

        public int? SequenceNo { get; set; }

        public bool IsActive { get; set; }

        [Precision(13, 3)]
        public decimal? Amount { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int GroupID { get; set; }

        // Navigation back to Employee
        public UserDefinedCodeGroup UserDefinedCodeGroup { get; set; } = null!;
        #endregion
    }
}
