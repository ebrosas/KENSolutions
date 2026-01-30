using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class AttendanceSwipeLog
    {
        #region Properties
        public long SwipeID { get; set; }       // Identity column   
        public int EmpNo { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime SwipeDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? SwipeTime { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? SwipeType { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? LocationCode { get; set; } = null;

        [NotMapped]
        public string? LocationName { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? ReaderCode { get; set; }
        
        [NotMapped]
        public string? ReaderName { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? StatusCode { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? SwipeLogDate { get; set; } = null;
        #endregion
    }
}
