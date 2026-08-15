using Microsoft.EntityFrameworkCore;

namespace KenHRApp.Application.Common.Interfaces;

public sealed class EfQueryExecutor : IQueryExecutor
{
    public async Task<IReadOnlyList<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        => await query.ToListAsync(cancellationToken).ConfigureAwait(false);

    public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        => query.CountAsync(cancellationToken);

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        => query.FirstOrDefaultAsync(cancellationToken);
}
