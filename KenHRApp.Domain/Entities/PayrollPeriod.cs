using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class PayrollPeriod
    {
        #region Properties
        public int PayrollPeriodId { get; set; }        // Identity column   
        public int FiscalYear { get; set; }
        public int FiscalMonth { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime PayrollStartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime PayrollEndDate { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedByEmpNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedByUserID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; } = null;               

        public int? LastUpdateEmpNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdateUserID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; } = null;
        #endregion
    }
}
