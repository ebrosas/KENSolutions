using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskServices.Helpers
{
    public class DataService
    {
        #region Public Methods
        //public async Task<Result<List<ApprovalRequestResult>>> GetApprovalRequestAsync(
        //    byte searchType,
        //    int? empNo,
        //    string? requestType)
        //{
        //    List<ApprovalRequestResult> requestTypeList = new();

        //    try
        //    {
        //        var model = await _db.Set<ApprovalRequestResult>()
        //            //.FromSqlRaw("EXEC kenuser.Pr_GetDashboardPendingRequest @empNo = {0}, @requestType = {1}",
        //            .FromSqlRaw("EXEC kenuser.Pr_GetDashboardStatistics @searchType = {0}, @empNo = {1}, @requestType = {2}",
        //            searchType,
        //            empNo!,
        //            requestType!)
        //            .ToListAsync();
        //        if (model != null && model.Any())
        //        {
        //            requestTypeList = model.Select(e => new ApprovalRequestResult
        //            {
        //                RequestNo = e.RequestNo,
        //                RequestTypeCode = e.RequestTypeCode,
        //                RequestTypeDesc = e.RequestTypeDesc,
        //                AppliedDate = e.AppliedDate,
        //                RequestedByNo = e.RequestedByNo,
        //                RequestedByName = e.RequestedByName,
        //                Detail = e.Detail,
        //                ApprovalRole = e.ApprovalRole,
        //                CurrentStatus = e.CurrentStatus,
        //                ApproverNo = e.ApproverNo,
        //                ApproverName = e.ApproverName,
        //                PendingDays = e.PendingDays,
        //                StepInstanceId = e.StepInstanceId,
        //                CreatedByEmpNo = e.CreatedByEmpNo,
        //                Remarks = e.Remarks
        //            }).ToList();
        //        }

        //        return Result<List<ApprovalRequestResult>>.SuccessResult(requestTypeList);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<List<ApprovalRequestResult>>.Failure($"Database error: {ex.Message}");
        //    }
        //}
        #endregion
    }
}
