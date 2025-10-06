using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class DepartmentDTO
    {
        #region Properties                
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Code is required")]
        [Display(Name = "Department Code")]
        [StringLength(20, ErrorMessage = "Department Code can't be more than 20 characters.")]
        public string DepartmentCode { get; set; } = null!;

        [Required(ErrorMessage = "Department Name is required")]
        [Display(Name = "Department Name")]
        [StringLength(120, ErrorMessage = "Department Code can't be more than 120 characters.")]
        public string DepartmentName { get; set; } = null!;

        public string? GroupCode { get; set; } = null;
        public string? GroupName { get; set; } = null;

        [Display(Name = "Description")]
        [StringLength(150, ErrorMessage = "Description can't be more than 150 characters.")]
        public string? Description { get; set; } = null;

        public int? ParentDepartmentId { get; set; }
        public string? ParentDepartmentName { get; set; } = null;
        public int? SuperintendentEmpNo { get; set; }
        public string? SuperintendentName { get; set; } = null;
        public int? ManagerEmpNo { get; set; }
        public string? ManagerName { get; set; } = null;
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? UpdatedAt { get; set; }
        #endregion

        #region Extended Properties
        public string IsActiveDesc 
        {
            get { return IsActive ? "Yes" : "No"; }
            set { }
        }

        public string DepartmentFullName
        {
            get { return $"{DepartmentCode} - {DepartmentName}"; }
        }
        #endregion
    }
}
