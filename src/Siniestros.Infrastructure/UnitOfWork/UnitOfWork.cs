using Siniestros.Application.Interfaces;
using Siniestros.Infrastructure.Persistence;


namespace Siniestros.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SiniestroDbContext _db;

        public ISiniestroRepository Siniestro { get; }

        public UnitOfWork(
            SiniestroDbContext db,
            ISiniestroRepository siniestroRepository)
        {
            _db = db;
            Siniestro = siniestroRepository;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);

        public void Dispose() => _db.Dispose();
    }
}
