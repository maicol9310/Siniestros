namespace Siniestros.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ISiniestroRepository Siniestro { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
