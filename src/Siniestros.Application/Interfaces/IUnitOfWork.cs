namespace Siniestros.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ISiniestroRepository Siniestro { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
