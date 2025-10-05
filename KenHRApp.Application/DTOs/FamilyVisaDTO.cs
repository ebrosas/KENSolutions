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
    public class FamilyVisaDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public int FamilyId { get; set; }
        public string FamilyMemberName { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string? Country { get; set; } = null;
        public string VisaTypeCode { get; set; } = null!;
        public string VisaType { get; set; } = null!;
        public string Profession { get; set; } = null!;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        #endregion

        #region Reference Navigations 
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        public FamilyMember FamilyMember { get; set; } = null!;
        #endregion
    }
}
