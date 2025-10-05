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
    public class QualificationDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string QualificationCode { get; set; } = null!;
        public string QualificationDesc { get; set; } = null!;
        public string? StreamCode { get; set; } = null;
        public string? StreamDesc { get; set; } = null;
        public string? SpecializationCode { get; set; } = null;
        public string? SpecializationDesc { get; set; } = null;
        public string UniversityName { get; set; } = null!;
        public string? Institute { get; set; } = null;
        public string QualificationMode { get; set; } = null!;
        public string QualificationModeDesc { get; set; } = null!;
        public string? CountryCode { get; set; } = null;
        public string? CountryDesc { get; set; } = null;
        public string? StateCode { get; set; } = null;
        public string? StateDesc { get; set; } = null;
        public string? CityTownName { get; set; } = null;
        public string FromMonthCode { get; set; } = null!;
        public string FromMonthDesc { get; set; } = null!;
        public int FromYear { get; set; }
        public string ToMonthCode { get; set; } = null!;
        public string ToMonthDesc { get; set; } = null!;
        public int ToYear { get; set; }
        public string PassMonthCode { get; set; } = null!;
        public string PassMonthDesc { get; set; } = null!;
        public int PassYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        public int EmployeeNo { get; set; }
        public int? TransactionNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
