using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class OtherDocument
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(150)"), Comment("Part of composite unique key index")]
        public string DocumentName { get; set; } = null!;

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string DocumentTypeCode { get; set; } = null!;

        [NotMapped]
        public string DocumentTypeDesc { get; set; } = null!;

        [Column(TypeName = "varchar(300)")]
        public string? Description { get; set; } = null;

        public byte[]? FileData { get; set; }
        public string? FileExtension { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? ContentTypeCode { get; set; } = null;

        [NotMapped]
        public string? ContentTypeDesc { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? UploadDate { get; set; }
        #endregion

        #region Reference Navigations 
        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; } 

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
