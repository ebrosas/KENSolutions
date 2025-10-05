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
    public class EmployeeTransactionDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string ActionCode { get; set; } = null!;
        public string ActionDesc { get; set; } = null!;
        public string StatusCode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string SectionCode { get; set; } = null!;
        public string Section { get; set; } = null!;
        public DateTime? LastUpdateOn { get; set; }
        public int? CurrentlyAssignedEmpNo { get; set; }
        public string? CurrentlyAssignedEmpName { get; set; } = null!;
        #endregion

        #region Reference Navigations
        public int EmployeeNo { get; set; }
        public int TransactionNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion

        #region Extended Propertie
        public string CurrentAssignedEmployee 
        { 
            get
            {
                if (CurrentlyAssignedEmpNo > 0 && !string.IsNullOrEmpty(CurrentlyAssignedEmpName))
                    return $"{CurrentlyAssignedEmpNo} - {CurrentlyAssignedEmpName}";
                else
                    return string.Empty;
            }   
        }
        #endregion
    }
}
