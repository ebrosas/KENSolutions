using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IErrorMessage
    {
        bool HasError { get; set; }
        string? ErrorMessage { get; set; }
    }
}
