using KenHRApp.Domain.Entities;
using KenHRApp.Infrastructure.Data;
using KenHRApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KenHRApp.Infrastructure.Persistence.Repositories;

public sealed class EmployeeProfileRepository : IEmployeeProfileRepository
{
    private readonly AppDbContext _context;

    public EmployeeProfileRepository(AppDbContext context) => _context = context;

    public IQueryable<TEntity> Query<TEntity>(Guid employeeId, bool asNoTracking = true) where TEntity : EmployeeOwnedEntity
    {
        var set = asNoTracking ? _context.Set<TEntity>().AsNoTracking() : _context.Set<TEntity>();
        return set.Where(e => e.EmployeeId == employeeId);
    }

    public Task<TEntity?> FindAsync<TEntity>(Guid employeeId, Guid id, CancellationToken cancellationToken = default)
        where TEntity : EmployeeOwnedEntity
        => _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id && e.EmployeeId == employeeId, cancellationToken);

    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : EmployeeOwnedEntity
        => await _context.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);

    public void Update<TEntity>(TEntity entity) where TEntity : EmployeeOwnedEntity => _context.Set<TEntity>().Update(entity);

    public void Remove<TEntity>(TEntity entity) where TEntity : EmployeeOwnedEntity => _context.Set<TEntity>().Remove(entity);

    public async Task<IReadOnlyCollection<Guid>> GetReportingLineAsync(Guid managerEmployeeId, CancellationToken cancellationToken = default)
    {
        // Breadth-first walk of the reporting tree; depth is bounded to protect against cyclic data.
        var visited = new HashSet<Guid> { managerEmployeeId };
        var frontier = new List<Guid> { managerEmployeeId };

        for (var depth = 0; depth < 12 && frontier.Count > 0; depth++)
        {
            var next = await _context.Employees
                .AsNoTracking()
                .Where(e => frontier.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            frontier = next.Where(visited.Add).ToList();
        }

        return visited;
    }

    public async Task<Guid?> GetEmployeeIdByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _context.Employees
            .AsNoTracking()
            .Where(e => e.UserID == userId)
            .Select(e => (Guid?)e.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
}