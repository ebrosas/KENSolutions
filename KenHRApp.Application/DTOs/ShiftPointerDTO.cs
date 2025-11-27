using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class ShiftPointerDTO
    {
        #region Properties
        public int ShiftPointerId { get; set; }
        public int ShiftPointer { get; set; }
        public string ShiftTiming { get; set; } = null!;
        #endregion

        #region Reference Navigation
        public string ShiftPatternCode { get; set; } = null!;
        public string ShiftCode { get; set; } = null!;
        public ShiftPatternMasterDTO ShiftPatternMaster { get; set; } = null!;
        #endregion
    }
}
