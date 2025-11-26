using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class ShiftTimingDTO
    {
        #region Properties
        public int ShiftTimingId { get; set; }

        [Required(ErrorMessage = "Shift Code is required")]
        [Display(Name = "Shift Code")]
        [StringLength(10, ErrorMessage = "Shift Code can't be more than 10 characters.")]
        public string ShiftCode { get; set; } = null!;

        [Required(ErrorMessage = "Shift Description is required")]
        [Display(Name = "Shift Description")]
        [StringLength(200, ErrorMessage = "Shift Code can't be more than 200 characters.")]
        public string ShiftDescription { get; set; } = null!;

        [Display(Name = "Arrival From")]
        public TimeSpan? ArrivalFrom { get; set; }

        [Required(ErrorMessage = "Arrival To is required")]
        [Display(Name = "Arrival To")]
        public TimeSpan ArrivalTo { get; set; }

        [Required(ErrorMessage = "Depart From is required")]
        [Display(Name = "Depart From")]
        public TimeSpan DepartFrom { get; set; }
        
        [Display(Name = "Depart To")]
        public TimeSpan? DepartTo { get; set; }

        [Display(Name = "Ramadan Arrival From")]
        public TimeSpan? RArrivalFrom { get; set; }

        [Required(ErrorMessage = "Ramadan Arrival To is required")]
        [Display(Name = "Ramadan Arrival To")]
        public TimeSpan RArrivalTo { get; set; }

        [Required(ErrorMessage = "Ramadan Depart From is required")]
        [Display(Name = "Ramadan Depart From")]
        public TimeSpan RDepartFrom { get; set; }

        [Display(Name = "Ramadan Depart To")]
        public TimeSpan? RDepartTo { get; set; }

        public int? CreatedByEmpNo { get; set; }
        public string? CreatedByName { get; set; } = null;
        public string? CreatedByUserID { get; set; } = null;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdateDate { get; set; }
        public int? LastUpdateEmpNo { get; set; }
        public string? LastUpdateUserID { get; set; } = null;
        public string? LastUpdatedByName { get; set; } = null;
        #endregion

        #region Extended Properties
        public string CreatedByFullName
        {
            get
            {
                return $"({CreatedByEmpNo}) {CreatedByName}";
            }
        }

        public string LastUpdateByFullName
        {
            get
            {
                return $"({LastUpdateEmpNo}) {LastUpdatedByName}";
            }
        }

        public string ShiftTiming
        {
            get
            {
                return $"{ShiftCode} - {ShiftDescription}";
            }
        }
        #endregion

        #region Reference Navigation to Recruitment Request   
        public string ShiftPatternCode { get; set; } = null!;
        public ShiftPatternMasterDTO ShiftPatternMaster { get; set; } = null!;
        #endregion
    }
}
