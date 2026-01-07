using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class ShiftPatternChange
    {
        #region MyRegion
        public int AutoId { get; set; }
        public int EmpNo { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string ShiftPatternCode { get; set; } = null!;

        public int ShiftPointer { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string ChangeType { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime EffectiveDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? EndingDate { get; set; } = null;

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
    }
}
