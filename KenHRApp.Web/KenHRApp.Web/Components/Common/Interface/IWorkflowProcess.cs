namespace KenHRApp.Web.Components.Common.Interface
{
    public interface IWorkflowProcess
    {
        #region Abstract Methods
        Task InitializeWorkflowAsync(long requisitionNo, int originatorEmpNo);
        #endregion
    }
}
