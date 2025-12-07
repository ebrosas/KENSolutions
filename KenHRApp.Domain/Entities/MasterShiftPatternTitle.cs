using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class MasterShiftPatternTitle
    {
        #region Properties
        public int ShiftPatternId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string ShiftPatternCode { get; set; } = null!;

        [Column(TypeName = "varchar(300)")]
        public string? ShiftPatternDescription { get; set; } = null;

        public bool IsActive { get; set; } = true;
        public bool? IsDayShift { get; set; }
        public bool? IsFlexiTime { get; set; }

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

        #region Reference Navigations
        public List<MasterShiftTime> ShiftTimingList { get; set; } = new List<MasterShiftTime>();
        public List<MasterShiftPattern> ShiftPointerList { get; set; } = new List<MasterShiftPattern>();
        #endregion
    }
}
