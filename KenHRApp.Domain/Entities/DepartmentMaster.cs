using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class DepartmentMaster
    {
        #region Properties                
        public int DepartmentId { get; set; }

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string DepartmentCode { get; set; } = null!;

        [Column(TypeName = "varchar(120)"), Comment("Part of composite unique key index")]
        public string DepartmentName { get; set; } = null!;

        [Column(TypeName = "varchar(20)"), Comment("Part of composite unique key index")]
        public string? GroupCode { get; set; } = null;

        [NotMapped]
        public string? GroupName { get; set; } = null;

        [Column(TypeName = "varchar(150)")]
        public string? Description { get; set; } = null;

        public int? ParentDepartmentId { get; set; }

        [Comment("Part of composite unique key index")]
        public int? SuperintendentEmpNo { get; set; }

        [NotMapped]
        public string? SuperintendentName { get; set; } = null;

        [Comment("Part of composite unique key index")]
        public int? ManagerEmpNo { get; set; }

        [NotMapped]
        public string? ManagerName { get; set; } = null;

        public bool IsActive { get; set; } = true;

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        #endregion
    }
}
