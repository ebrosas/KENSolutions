using Microsoft.AspNetCore.Components.Forms;

namespace KenHRApp.Web.Data.DTOs
{
    public class FileModelDTO
    {
        #region Properties
        public string Name { get; set; } = null!;
        public IBrowserFile File { get; set; }
        #endregion
    }
}
