using KenHRApp.Application.Common.Models;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Features.Employees.Interfaces;
using KenHRApp.Application.Features.Employees.Queries;
using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Features.Employees.Services
{
    public sealed class LoveEmployeeService : ILoveEmployeeService
    {
        #region Public Methods
        public async Task<PagedResult<EmployeeDTO>> GetEmployeesAsync(GetEmployeesQuery query, CancellationToken cancellationToken = default)
        {
            //var access = await _access.GetAccessContextAsync(cancellationToken).ConfigureAwait(false);
            //if (access.Scope == EmployeeAccessScope.None)
            //{
            //    return PagedResult<EmployeeDTO>.Empty(query.PageSize);
            //}

            //var source = _employees.Query();

            //if (access.Scope != EmployeeAccessScope.All)
            //{
            //    var visible = access.VisibleEmployeeIds;
            //    source = source.Where(e => visible.Contains(e.Id));
            //}

            //if (query.ManagerId is { } managerId)
            //{
            //    source = source.Where(e => e.ManagerId == managerId);
            //}

            //if (query.Statuses.Count > 0)
            //{
            //    var statuses = query.Statuses;
            //    source = source.Where(e => statuses.Contains(e.Status));
            //}

            //if (query.DepartmentId is { } departmentId)
            //{
            //    source = source.Where(e => e.DepartmentId == departmentId);
            //}

            //if (query.PositionId is { } positionId)
            //{
            //    source = source.Where(e => e.PositionId == positionId);
            //}

            //if (query.LocationId is { } locationId)
            //{
            //    source = source.Where(e => e.LocationId == locationId);
            //}

            //if (query.Status is { } status)
            //{
            //    source = source.Where(e => e.Status == status);
            //}

            //if (query.EmploymentType is { } employmentType)
            //{
            //    source = source.Where(e => e.EmploymentType == employmentType);
            //}

            //if (query.JoinedFrom is { } from)
            //{
            //    source = source.Where(e => e.JoiningDate >= from);
            //}

            //if (query.JoinedTo is { } to)
            //{
            //    source = source.Where(e => e.JoiningDate <= to);
            //}

            //if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            //{
            //    var term = query.SearchTerm.Trim();
            //    source = source.Where(e =>
            //        e.EmployeeNumber.Contains(term) ||
            //        e.Name.FirstNameEn.Contains(term) ||
            //        e.Name.LastNameEn.Contains(term) ||
            //        (e.Name.FirstNameAr != null && e.Name.FirstNameAr.Contains(term)) ||
            //        (e.Name.LastNameAr != null && e.Name.LastNameAr.Contains(term)) ||
            //        e.Contact.Email.Contains(term));
            //}

            //source = (query.SortBy?.ToLowerInvariant()) switch
            //{
            //    "employeenumber" => query.SortDescending ? source.OrderByDescending(e => e.EmployeeNumber) : source.OrderBy(e => e.EmployeeNumber),
            //    "joiningdate" => query.SortDescending ? source.OrderByDescending(e => e.JoiningDate) : source.OrderBy(e => e.JoiningDate),
            //    "status" => query.SortDescending ? source.OrderByDescending(e => e.Status) : source.OrderBy(e => e.Status),
            //    "departmentname" => query.SortDescending ? source.OrderByDescending(e => e.Department!.NameEn) : source.OrderBy(e => e.Department!.NameEn),
            //    "positiontitle" => query.SortDescending ? source.OrderByDescending(e => e.Position!.TitleEn) : source.OrderBy(e => e.Position!.TitleEn),
            //    "locationname" => query.SortDescending ? source.OrderByDescending(e => e.Location!.NameEn) : source.OrderBy(e => e.Location!.NameEn),
            //    "managername" => query.SortDescending ? source.OrderByDescending(e => e.Manager!.Name.FirstNameEn) : source.OrderBy(e => e.Manager!.Name.FirstNameEn),
            //    "fullnameen" => query.SortDescending
            //        ? source.OrderByDescending(e => e.Name.FirstNameEn).ThenByDescending(e => e.Name.LastNameEn)
            //        : source.OrderBy(e => e.Name.FirstNameEn).ThenBy(e => e.Name.LastNameEn),
            //    _ => query.SortDescending
            //        ? source.OrderByDescending(e => e.Name.FirstNameEn).ThenByDescending(e => e.Name.LastNameEn)
            //        : source.OrderBy(e => e.Name.FirstNameEn).ThenBy(e => e.Name.LastNameEn),
            //};

            //var total = await _queryExecutor.CountAsync(source, cancellationToken).ConfigureAwait(false);
            //var projected = source
            //    .Skip(query.Skip)
            //    .Take(query.PageSize)
            //    .ProjectTo<EmployeeDTO>(_mapper.ConfigurationProvider);

            //var items = await _queryExecutor.ToListAsync(projected, cancellationToken).ConfigureAwait(false);

            //return new PagedResult<EmployeeDTO>(items, total, query.Page, query.PageSize);
            return null;
        }
        #endregion
    }
}
