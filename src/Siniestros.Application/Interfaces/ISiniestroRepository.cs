using Siniestros.Domain.Aggregates;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Interfaces
{
    public interface ISiniestroRepository
    {
        Task<Siniestro?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Siniestro siniestro, CancellationToken ct);
        void Update(Siniestro siniestro);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task<PagedResult<Siniestro>> ListAsync(SiniestroFilter filter,CancellationToken ct = default);
    }
}
