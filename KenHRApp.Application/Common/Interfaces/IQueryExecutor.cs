namespace KenHRApp.Application.Common.Interfaces;

/// <summary>
/// Keeps the Application layer free of an EF Core reference while still allowing
/// asynchronous materialisation of composed <see cref="IQueryable{T}"/> pipelines.
/// Implemented in Infrastructure over EF Core's async operators.
/// </summary>
public interface IQueryExecutor
{
    Task<IReadOnlyList<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);
}
