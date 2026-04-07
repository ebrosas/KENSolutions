using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class RequestTypeResult
    {
        #region Properties
        public string RequestTypeCode { get; set; } = null!;
        public string RequestTypeName { get; set; } = null!;
        public string? RequestTypeDesc { get; set; } = null;
        public string? IconName { get; set; } = null;
        public int AssignedCount { get; set; }
        #endregion
    }
}
