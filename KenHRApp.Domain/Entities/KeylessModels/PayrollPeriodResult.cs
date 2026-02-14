using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class PayrollPeriodResult
    {
        #region Properties
        public int PayrollPeriodId { get; set; }
        public int FiscalYear { get; set; }
        public int FiscalMonth { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime PayrollStartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime PayrollEndDate { get; set; }

        public bool IsActive { get; set; }
        #endregion
    }
}
