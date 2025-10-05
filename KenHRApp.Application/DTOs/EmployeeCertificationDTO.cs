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
    public class EmployeeCertificationDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string QualificationCode { get; set; } = null!;
        public string QualificationDesc { get; set; } = null!;
        public string? StreamCode { get; set; } = null;
        public string? StreamDesc { get; set; } = null;
        public string Specialization { get; set; } = null!;
        public string University { get; set; } = null!;
        public string? Institute { get; set; } = null;
        public string? CountryCode { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? State { get; set; } = null;
        public string? CityTownName { get; set; } = null;
        public string FromMonthCode { get; set; } = null!;
        public string? FromMonth { get; set; } = null;
        public int FromYear { get; set; }
        public string ToMonthCode { get; set; } = null!;
        public string? ToMonth { get; set; } = null;
        public int ToYear { get; set; }
        public string PassMonthCode { get; set; } = null!;
        public string? PassMonth { get; set; } = null;
        public int? PassYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
