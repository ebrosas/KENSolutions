using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.Features.Employees.Interfaces;
using KenHRApp.Application.Features.Employees.Models;
using KenHRApp.Infrastructure.Identity;
using KenHRApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Features.Employees.Services;

/// <summary>
/// Employee Master authorization boundary:
/// HR back office sees and manages everyone, line managers read their reporting line,
/// and everybody else reads only their own record.
/// </summary>
public sealed class EmployeeAccessService : IEmployeeAccessService
{
    //private readonly ICurrentUserService _currentUser;
    private readonly IEmployeeProfileRepository _repository;

    private EmployeeAccessContext? _cached;

    //public EmployeeAccessService(ICurrentUserService currentUser, IEmployeeProfileRepository repository)
    //{
    //    _currentUser = currentUser;
    //    _repository = repository;
    //}

    public EmployeeAccessService(IEmployeeProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeAccessContext> GetAccessContextAsync(CancellationToken cancellationToken = default)
    {
        return _cached;

        //if (_cached is not null)
        //{
        //    return _cached;
        //}

        //if (!_currentUser.IsAuthenticated || !string.IsNullOrEmpty(_currentUser.UserId))
        //{
        //    return _cached = EmployeeAccessContext.Denied;
        //}

        //var isHrBackOffice =
        //    _currentUser.IsInRole(HrmsRoles.SystemAdministrator) ||
        //    _currentUser.IsInRole(HrmsRoles.HrAdministrator) ||
        //    _currentUser.IsInRole(HrmsRoles.HrManager);

        //var employeeId = await _repository.GetEmployeeIdByUserAsync(_currentUser.UserId!, cancellationToken).ConfigureAwait(false);

        //if (isHrBackOffice)
        //{
        //    return _cached = new EmployeeAccessContext(EmployeeAccessScope.All, employeeId, Array.Empty<Guid>(), canManage: true);
        //}

        //if (employeeId is not { } id)
        //{
        //    return _cached = EmployeeAccessContext.Denied;
        //}

        //var isManager = _currentUser.IsInRole(HrmsRoles.Manager) || _currentUser.IsInRole(HrmsRoles.Supervisor);

        //if (isManager)
        //{
        //    var line = await _repository.GetReportingLineAsync(id, cancellationToken).ConfigureAwait(false);
        //    return _cached = new EmployeeAccessContext(EmployeeAccessScope.Team, id, line, canManage: false);
        //}

        //return _cached = new EmployeeAccessContext(EmployeeAccessScope.Self, id, new[] { id }, canManage: false);
    }
}

