using KenHRApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class AppState : IAppState
    {
        #region Properties
        public bool ShowAppDrawer { get; set; } = false;
        #endregion
    }
}
