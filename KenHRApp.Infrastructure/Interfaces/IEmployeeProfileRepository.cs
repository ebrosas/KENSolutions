using KenHRApp.Domain.Entities;

namespace KenHRApp.Infrastructure.Interfaces;

/// <summary>Persistence access for the employee child collections behind the profile tabs.</summary>
public interface IEmployeeProfileRepository
{
    IQueryable<TEntity> Query<TEntity>(Guid employeeId, bool asNoTracking = true) where TEntity : EmployeeOwnedEntity;

    Task<TEntity?> FindAsync<TEntity>(Guid employeeId, Guid id, CancellationToken cancellationToken = default)
        where TEntity : EmployeeOwnedEntity;

    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : EmployeeOwnedEntity;

    void Update<TEntity>(TEntity entity) where TEntity : EmployeeOwnedEntity;

    void Remove<TEntity>(TEntity entity) where TEntity : EmployeeOwnedEntity;

    /// <summary>Ids of every employee reporting to the given manager, directly or indirectly.</summary>
    Task<IReadOnlyCollection<Guid>> GetReportingLineAsync(Guid managerEmployeeId, CancellationToken cancellationToken = default);

    Task<Guid?> GetEmployeeIdByUserAsync(string userId, CancellationToken cancellationToken = default);
}
