using KenHRApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IAppState
    {
        bool ShowAppDrawer { get; set; }
        bool IsAuthenticated { get; set; }
        RecruitmentRequestDTO? RecruitmentRequest { get; set; }
    }
}
