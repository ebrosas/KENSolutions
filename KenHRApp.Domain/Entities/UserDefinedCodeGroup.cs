using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class UserDefinedCodeGroup
    {
        #region Properties
        [Comment("Primary key of UserDefinedCodeGroup entity")]
        public int UDCGroupId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string UDCGCode { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string UDCGDesc1 { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string? UDCGDesc2 { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? UDCGSpecialHandlingCode { get; set; } = null;
        #endregion

        #region Navigation Properties
        public ICollection<UserDefinedCode> UserDefinedCodeList { get; set; } = new List<UserDefinedCode>();
        #endregion
    }
}
