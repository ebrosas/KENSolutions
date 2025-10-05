using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Interfaces
{
    public interface IBankDetail
    {
        string? AccountTypeCode { get; set; }
        string? AccountNumber { get; set; }
        string? AccountHolderName { get; set; }
        string? BankNameCode { get; set; }
        string? BankBranchName { get; set; }
        string? IBANNumber { get; set; }
        string? TaxNumber { get; set; }
    }
}
