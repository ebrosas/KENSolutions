using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class PayrollPeriodResultDTO
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

        #region Extended Properties
        public string PayrollPeriodKey 
        { 
            get
            {
                return $"{FiscalYear}-{FiscalMonth.ToString("D2")}";    
            }
        }

        public string PeriodDescription
        {
            get
            {
                string monthName = new DateTime(FiscalYear, FiscalMonth, 1).ToString("MMM");

                return $"{monthName} {FiscalYear} " +
                       $"({PayrollStartDate:dd/MM/yyyy} – {PayrollEndDate:dd/MM/yyyy})";
            }
        }
        #endregion
    }
}
