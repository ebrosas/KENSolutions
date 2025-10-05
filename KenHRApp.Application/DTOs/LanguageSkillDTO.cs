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
    public class LanguageSkillDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string LanguageCode { get; set; } = null!;
        public string? LanguageDesc { get; set; } = null;
        public bool? CanWrite { get; set; } = null;
        public bool? CanSpeak { get; set; } = null;
        public bool? CanRead { get; set; } = null;
        public bool? MotherTongue { get; set; } = null;
        #endregion

        #region Reference Navigation to Employee   
        public int? TransactionNo { get; set; }
        public int EmployeeNo { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion

        #region Extended Properties
        public string CanReadDescription 
        { 
            get
            {
                return this.CanRead.HasValue
                    ? (this.CanRead.Value ? "Yes" : "No")
                    : "Not Specified";  
            }
        }

        public string CanSpeakDescription
        {
            get
            {
                return this.CanSpeak.HasValue
                    ? (this.CanSpeak.Value ? "Yes" : "No")
                    : "Not Specified";
            }
        }

        public string CanWriteDescription
        {
            get
            {
                return this.CanWrite.HasValue
                    ? (this.CanWrite.Value ? "Yes" : "No")
                    : "Not Specified";
            }
        }

        public string MotherTongueDescription
        {
            get
            {
                return this.MotherTongue.HasValue
                    ? (this.MotherTongue.Value ? "Yes" : "No")
                    : "Not Specified";
            }
        }
        #endregion
    }
}
