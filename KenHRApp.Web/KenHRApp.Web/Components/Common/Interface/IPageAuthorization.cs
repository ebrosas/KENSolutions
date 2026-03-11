namespace KenHRApp.Web.Components.Common.Interface
{
    public interface IPageAuthorization
    {
        #region Abstract Properties
        string UserName { get; set; }
        public int UserEmpNo { get; set; }
        #endregion

        #region Abstract Methods
        void GoToLogin();
        #endregion
    }
}
