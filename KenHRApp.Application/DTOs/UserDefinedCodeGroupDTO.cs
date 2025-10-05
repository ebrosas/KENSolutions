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
    public class UserDefinedCodeGroupDTO
    {
        #region Properties
        public int UDCGroupId { get; set; }
        public string UDCGCode { get; set; } = null!;
        public string UDCGDesc1 { get; set; } = null!;
        public string? UDCGDesc2 { get; set; } = null;
        public string? UDCGSpecialHandlingCode { get; set; } = null;
        #endregion
    }
}
