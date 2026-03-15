namespace KenHRApp.Web.Components.Common.Interface
{
    public interface IPageAuthorization
    {
        #region Abstract Properties
        string UserName { get; set; }
        int UserEmpNo { get; set; }
        string? UserID { get; set; }
        string? UserEmail { get; set; }
        string? UserCostCenter { get; set; }
        #endregion

        #region Abstract Methods
        void GoToLogin();
        #endregion
    }
}
