using KenHRApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class OtherDocumentDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string DocumentName { get; set; } = null!;
        public string DocumentTypeCode { get; set; } = null!;
        public string DocumentTypeDesc { get; set; } = null!;
        public string? Description { get; set; } = null;
        public byte[]? FileData { get; set; }
        public string? FileExtension { get; set; } = null;
        public string? ContentTypeCode { get; set; } = null;
        public string? ContentTypeDesc { get; set; } = null;
        public DateTime? UploadDate { get; set; }
        #endregion

        #region Reference Navigations 
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
