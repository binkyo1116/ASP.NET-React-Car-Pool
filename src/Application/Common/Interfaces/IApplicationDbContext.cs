namespace Carpool.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Event> Events { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
