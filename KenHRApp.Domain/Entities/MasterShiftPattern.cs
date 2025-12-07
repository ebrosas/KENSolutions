using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class MasterShiftPattern
    {
        #region Properties
        public int ShiftPointerId { get; set; }
        public int ShiftPointer { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? ShiftDescription { get; set; } = null;
        #endregion

        #region Reference Navigation
        [Column(TypeName = "varchar(10)")]
        public string ShiftPatternCode { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string ShiftCode { get; set; } = null!;
        #endregion

        #region Reference Navigation to MasterShiftPatternTitle
        [Comment("Foreign key that references primary key: MasterShiftPatternTitle.ShiftPatternId")]
        public int ShiftPatternId { get; set; }
        public MasterShiftPatternTitle MasterShiftPatternTitle { get; set; } = null!;
        #endregion
    }
}
