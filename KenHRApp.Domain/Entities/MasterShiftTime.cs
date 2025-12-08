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
    public class MasterShiftTime
    {
        #region Properties
        public int ShiftTimingId { get; set; }
                
        [Column(TypeName = "varchar(10)")]
        public string ShiftCode { get; set; } = null!;

        [Column(TypeName = "varchar(200)")]
        public string ShiftDescription { get; set; } = null!;

        [Column(TypeName = "time")]
        public TimeSpan? ArrivalFrom { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan ArrivalTo { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan DepartFrom { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? DepartTo { get; set; }

        public int DurationNormal { get; set; } = 0;

        [Column(TypeName = "time")]
        public TimeSpan? RArrivalFrom { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? RArrivalTo { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? RDepartFrom { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? RDepartTo { get; set; }

        public int? DurationRamadan { get; set; } 

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

        #region Reference Navigation to MasterShiftPatternTitle
        [Comment("Foreign key that references alternate key: MasterShiftPatternTitle.ShiftPatternCode")]
        [Column(TypeName = "varchar(20)")]
        public string ShiftPatternCode { get; set; } = null!;

        public MasterShiftPatternTitle MasterShiftPatternTitle { get; set; } = null!;
        #endregion
    }
}
