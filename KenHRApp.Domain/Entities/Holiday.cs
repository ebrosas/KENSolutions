using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class Holiday
    {
        #region Properties
        public int HolidayId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string HolidayDesc { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime HolidayDate { get; set; }

        [Column(TypeName = "tinyint")]
        public byte HolidayType { get; set; }

        public int? CreatedByEmpNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedByName { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? CreatedByUserID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; } = null;

        public int? LastUpdateEmpNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdateUserID { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedByName { get; set; } = null;
        #endregion

        #region Not Mapped Properties
        [NotMapped]
        public string? HolidayTypeDesc { get; set; } = null;
        #endregion
    }
}
