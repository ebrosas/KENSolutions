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
    public class IdentityProofDTO
    {
        #region Identity Proof Properties
        public int AutoId { get; set; }
        public string? PassportNumber { get; set; } = null;
        public DateTime? DateOfIssue { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public string? PlaceOfIssue { get; set; } = null;
        #endregion

        #region Driving License Properties
        public string? DrivingLicenseNo { get; set; } = null;
        public DateTime? DLDateOfIssue { get; set; }
        public DateTime? DLDateOfExpiry { get; set; }
        public string? DLPlaceOfIssue { get; set; } = null;
        #endregion

        #region Other Document Properties
        public string? TypeOfDocument { get; set; } = null;
        public string? OtherDocNumber { get; set; } = null;
        public DateTime? OtherDocDateOfIssue { get; set; }
        public DateTime? OtherDocDateOfExpiry { get; set; }
        #endregion

        #region National ID Properties
        public string? NationalIDNumber { get; set; } = null;
        public string? NationalIDTypeCode { get; set; } = null;
        public string? NationalIDTypeDesc { get; set; } = null;
        public string? NatIDPlaceOfIssue { get; set; } = null;
        public DateTime? NatIDDateOfIssue { get; set; }
        public DateTime? NatIDDateOfExpiry { get; set; }
        #endregion

        #region Contract Detail Properties
        public string? ContractNumber { get; set; } = null;
        public string? ContractPlaceOfIssue { get; set; } = null;
        public DateTime? ContractDateOfIssue { get; set; }
        public DateTime? ContractDateOfExpiry { get; set; }
        #endregion

        #region Visa Detail Properties
        public string? VisaNumber { get; set; } = null;
        public string? VisaTypeCode { get; set; } = null;
        public string? VisaTypeDesc { get; set; } = null;
        public string? VisaCountryCode { get; set; } = null;
        public string? VisaCountryDesc { get; set; } = null;
        public string? Profession { get; set; } = null;
        public string? Sponsor { get; set; } = null;
        public DateTime? VisaDateOfIssue { get; set; }
        public DateTime? VisaDateOfExpiry { get; set; }
        #endregion

        #region Reference Navigation 
        public int EmployeeNo { get; set; }
        public int? TransactionNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
