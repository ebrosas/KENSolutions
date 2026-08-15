using AutoMapper;
using AutoMapper.QueryableExtensions;
using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.Common.Models;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Features.Employees.Interfaces;
using KenHRApp.Application.Features.Employees.Models;
using KenHRApp.Application.Features.Employees.Queries;
using KenHRApp.Domain.Entities;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Features.Employees.Services
{
    public sealed class LoveEmployeeService : ILoveEmployeeService
    {
        #region Fields
        private readonly IEmployeeRepository _repository;
        private readonly IEmployeeAccessService _access;
        private readonly IMapper _mapper;
        private readonly IQueryExecutor _queryExecutor;
        #endregion

        #region Constructors
        public LoveEmployeeService(
            IEmployeeRepository repository
            //IEmployeeAccessService access
            //IMapper mapper
            //IQueryExecutor queryExecutor
        )
        {
            _repository = repository;
            //_access = access;
            //_mapper = mapper;
            //_queryExecutor = queryExecutor;
        }
        #endregion

        #region Public Methods
        public async Task<PagedResult<EmployeeDTO>> GetEmployeesAsync(GetEmployeesQuery query, CancellationToken cancellationToken = default)
        {
            //var access = await _access.GetAccessContextAsync(cancellationToken).ConfigureAwait(false);
            //if (access.Scope == EmployeeAccessScope.None)
            //{
            //    return PagedResult<EmployeeDTO>.Empty(query.PageSize);
            //}

            var source = _repository.Query();

            //if (access.Scope != EmployeeAccessScope.All)
            //{
            //    var visible = access.VisibleEmployeeIds;
            //    source = source.Where(e => visible.Contains(e.Id));
            //}

            if (query.ManagerId is { } managerId)
            {
                source = source.Where(e => e.ReportingManagerCode == managerId);
            }

            //if (query.Status is { } status)
            //{
            //    source = source.Where(e => e.EmployeeStatusCode == status);
            //}

            //if (query.Statuses.Count > 0)
            //{
            //    var statuses = query.Statuses;
            //    source = source.Where(e => statuses.Contains(e.Status));
            //}

            if (query.DepartmentId is { } departmentId)
            {
                source = source.Where(e => e.DepartmentCode == departmentId);
            }

            if (query.PositionId is { } positionId)
            {
                source = source.Where(e => e.JobTitleCode == positionId);
            }

            if (query.LocationId is { } locationId)
            {
                source = source.Where(e => e.LocationCode == locationId);
            }

            if (query.Status is { } status)
            {
                source = source.Where(e => e.EmployeeStatusCode == status);
            }

            if (query.EmploymentType is { } employmentType)
            {
                source = source.Where(e => e.EmploymentType == employmentType);
            }

            if (query.JoinedFrom is { } from)
            {
                source = source.Where(e => DateOnly.FromDateTime(e.HireDate) >= from);
            }

            if (query.JoinedTo is { } to)
            {
                source = source.Where(e => DateOnly.FromDateTime(e.HireDate) <= to);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.Trim();
                source = source.Where(e =>
                    e.EmployeeNo.ToString().Contains(term) ||
                    e.FirstName.Contains(term) ||
                    e.MiddleName!.Contains(term) ||
                    e.LastName.Contains(term) ||
                    (e.FirstNameAr != null && e.FirstNameAr.Contains(term)) ||
                    (e.MiddleNameAr != null && e.MiddleNameAr.Contains(term)) ||
                    (e.LastNameAr != null && e.LastNameAr.Contains(term)) ||
                    e.OfficialEmail.Contains(term));
            }

            source = (query.SortBy?.ToLowerInvariant()) switch
            {
                "employeenumber" => query.SortDescending ? source.OrderByDescending(e => e.EmployeeNo) : source.OrderBy(e => e.EmployeeNo),
                "joiningdate" => query.SortDescending ? source.OrderByDescending(e => e.HireDate) : source.OrderBy(e => e.HireDate),
                "status" => query.SortDescending ? source.OrderByDescending(e => e.EmployeeStatusDesc) : source.OrderBy(e => e.EmployeeStatusDesc),
                "departmentname" => query.SortDescending ? source.OrderByDescending(e => e.DepartmentName) : source.OrderBy(e => e.DepartmentName),
                "positiontitle" => query.SortDescending ? source.OrderByDescending(e => e.JobTitleDesc) : source.OrderBy(e => e.JobTitleDesc),
                "locationname" => query.SortDescending ? source.OrderByDescending(e => e.LocationDesc) : source.OrderBy(e => e.LocationDesc),
                "managername" => query.SortDescending ? source.OrderByDescending(e => e.ReportingManager) : source.OrderBy(e => e.ReportingManager),
                "fullnameen" => query.SortDescending
                    ? source.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName)
                    : source.OrderBy(e => e.FirstName).ThenBy(e => e.LastName),
                _ => query.SortDescending
                    ? source.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName)
                    : source.OrderBy(e => e.FirstName).ThenBy(e => e.LastName),
            };

            int total = 0;
            var queryResult = source.ToList();
            if (queryResult != null)
                total = queryResult.Count;

            //var total = await _queryExecutor.CountAsync(source, cancellationToken).ConfigureAwait(false);

            //var projected = source
            //    .Skip(query.Skip)
            //    .Take(query.PageSize)
            //    .ProjectTo<EmployeeDTO>(_mapper.ConfigurationProvider);

            //var items = await _queryExecutor.ToListAsync(projected, cancellationToken).ConfigureAwait(false);

            var projected = source
              .Skip(query.Skip)
              .Take(query.PageSize)
              .ToList();

            List<EmployeeDTO> list = new List<EmployeeDTO>();
            foreach (var item in projected)
            {
                list.Add(new EmployeeDTO()
                {
                    EmployeeNo = item.EmployeeNo,
                    FirstName = item.FirstName
                });
            }

            return new PagedResult<EmployeeDTO>(list, total, query.Page, query.PageSize);
        }
        #endregion
    }
}
