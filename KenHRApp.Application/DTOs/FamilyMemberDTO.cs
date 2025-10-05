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
    public class FamilyMemberDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; } = null;
        public string LastName { get; set; } = null!;
        public string RelationCode { get; set; } = null!;
        public string? Relation { get; set; } = null;
        public DateTime? DOB { get; set; }
        public string? QualificationCode { get; set; } = null;
        public string? Qualification { get; set; } = null;
        public string? StreamCode { get; set; } = null;
        public string? StreamDesc { get; set; } = null;
        public string? SpecializationCode { get; set; } = null;
        public string? Specialization { get; set; } = null;
        public string? Occupation { get; set; } = null;
        public string? ContactNo { get; set; } = null;
        public string? CountryCode { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? StateCode { get; set; } = null;
        public string? State { get; set; } = null;
        public string? CityTownName { get; set; } = null;
        public string? District { get; set; } = null;
        public bool? IsDependent { get; set; } = null;
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion

        #region Extended Properties 
        public string IsDependentDesc
        {
            get
            {
                return this.IsDependent.HasValue
                    ? (this.IsDependent.Value ? "Yes" : "No")
                    : "Not Specified";
            }
        }
        #endregion
    }
}
