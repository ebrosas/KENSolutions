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
    public class EmergencyContactDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string ContactPerson { get; set; } = null!;
        public string RelationCode { get; set; } = null!;
        public string? Relation { get; set; } = null;
        public string MobileNo { get; set; } = null!;
        public string? LandlineNo { get; set; } = null;
        public string? Address { get; set; } = null;
        public string? CountryCode { get; set; } = null;
        public string? CountryDesc { get; set; } = null;
        public string? City { get; set; } = null;
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
