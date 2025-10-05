using KenHRApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class EmployeeSkillDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string SkillName { get; set; } = null!;
        public string? LevelCode { get; set; } = null;
        public string? LevelDesc { get; set; } = null;
        public string? LastUsedMonthCode { get; set; } = null;
        public string? LastUsedMonthDesc { get; set; } = null;
        public int? LastUsedYear { get; set; }
        public string? FromMonthCode { get; set; } = null;
        public string? FromMonthDesc { get; set; } = null;
        public int? FromYear { get; set; }
        public string? ToMonthCode { get; set; } = null;
        public string? ToMonthDesc { get; set; } = null;
        public int? ToYear { get; set; }
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion
    }
}
