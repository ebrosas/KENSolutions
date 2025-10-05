using KenHRApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class UserDefinedCodeDTO: IErrorMessage
    {
        #region Properties
        public int UDCId { get; set; }
        public string UDCCode { get; set; } = null!;
        public string UDCDesc1 { get; set; } = null!;
        public string? UDCDesc2 { get; set; } = null;
        public string? UDCSpecialHandlingCode { get; set; } = null;
        public int? SequenceNo { get; set; }
        public bool IsActive { get; set; }
        public decimal? Amount { get; set; }
        public int GroupID { get; set; }
        #endregion

        #region IErrorMessage Implementation
        public bool HasError { get; set; } = false;
        public string? ErrorMessage { get; set; } = null;
        #endregion
    }
}
