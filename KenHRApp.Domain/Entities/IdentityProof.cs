using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class IdentityProof
    {
        #region Identity Proof Properties
        public int AutoId { get; set; }
        
        [Column(TypeName = "varchar(20)")]
        public string? PassportNumber { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? DateOfIssue { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateOfExpiry{ get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? PlaceOfIssue { get; set; } = null;
        #endregion

        #region Driving License Properties
        [Column(TypeName = "varchar(20)")]
        public string? DrivingLicenseNo { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? DLDateOfIssue { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DLDateOfExpiry { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? DLPlaceOfIssue { get; set; } = null;
        #endregion

        #region Other Document Properties
        [Column(TypeName = "varchar(50)")]
        public string? TypeOfDocument { get; set; } = null;

        [Column(TypeName = "varchar(30)")]
        public string? OtherDocNumber{ get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? OtherDocDateOfIssue { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? OtherDocDateOfExpiry { get; set; }
        #endregion

        #region National ID Properties
        [Column(TypeName = "varchar(40)")]
        public string? NationalIDNumber { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? NationalIDTypeCode { get; set; } = null;

        [NotMapped]
        public string? NationalIDTypeDesc { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? NatIDPlaceOfIssue { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? NatIDDateOfIssue { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime? NatIDDateOfExpiry { get; set; }
        #endregion

        #region Contract Detail Properties
        [Column(TypeName = "varchar(30)")]
        public string? ContractNumber { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? ContractPlaceOfIssue { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? ContractDateOfIssue { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ContractDateOfExpiry { get; set; }
        #endregion

        #region Visa Detail Properties
        [Column(TypeName = "varchar(30)")]
        public string? VisaNumber { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? VisaTypeCode { get; set; } = null;

        [NotMapped]
        public string? VisaTypeDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? VisaCountryCode { get; set; } = null;

        [NotMapped]
        public string? VisaCountryDesc { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? Profession { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? Sponsor { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? VisaDateOfIssue { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? VisaDateOfExpiry { get; set; }
        #endregion

        #region Reference Navigation 
        //[Comment("Composite foreign key that references Employee.EmployeeId")]
        //public int IPEmployeeId { get; set; }

        [Comment("Foreign key that references alternate key: Employee.EmployeeNo")]
        public int EmployeeNo { get; set; }

        [Comment("Unique ID number that is generated when a request requires approval")]
        public int? TransactionNo { get; set; }

        // Navigation back to Employee
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
