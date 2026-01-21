using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class HolidayDTO
    {
        #region Properties
        public int HolidayId { get; set; }
        public string HolidayDesc { get; set; } = null!;
        public DateTime HolidayDate { get; set; }
        #endregion

        #region Not Mapped Properties
        [NotMapped]
        public string HolidayDOW { get; set; } = null!;

        [NotMapped]
        public string HolidayMonth { get; set; } = null!;
        #endregion
    }
}
