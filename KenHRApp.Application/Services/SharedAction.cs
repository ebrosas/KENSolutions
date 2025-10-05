using KenHRApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class SharedAction : ISharedAction
    {
        #region Properties
        public Action? ShowHideDrawer { get; set; }
        #endregion

        #region Public Methods
        public void InvokeShowHideDrawer()
        {
            ShowHideDrawer?.Invoke();
        }
        #endregion
    }
}
