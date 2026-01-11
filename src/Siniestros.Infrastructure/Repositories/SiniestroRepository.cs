using Microsoft.EntityFrameworkCore;
using Siniestros.Application.Interfaces;
using Siniestros.Domain.Aggregates;
using Siniestros.Infrastructure.Persistence;
using Siniestros.SharedKernel;

namespace Siniestros.Infrastructure.Repositories
{
    public class SiniestroRepository : ISiniestroRepository
    {
        private readonly SiniestroDbContext _context;

        public SiniestroRepository(SiniestroDbContext context)
        {
            _context = context;
        }

        public async Task<Siniestro?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Siniestros
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task AddAsync(Siniestro siniestro, CancellationToken ct)
        {
            await _context.Siniestros.AddAsync(siniestro, ct);
        }

        public void Update(Siniestro siniestro)
        {
            _context.Siniestros.Update(siniestro);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Siniestros
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity != null)
                _context.Siniestros.Remove(entity);
        }

        public async Task<PagedResult<Siniestro>> ListAsync(
            SiniestroFilter filter,
            CancellationToken ct = default)
        {
            var query = _context.Siniestros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Departamento))
                query = query.Where(x => x.Departamento == filter.Departamento);

            if (filter.FechaInicio.HasValue)
                query = query.Where(x => x.FechaHora >= filter.FechaInicio.Value);

            if (filter.FechaFin.HasValue)
                query = query.Where(x => x.FechaHora <= filter.FechaFin.Value);

            var totalItems = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.FechaHora)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(ct);

            return new PagedResult<Siniestro>(
                items,
                totalItems,
                filter.Page,
                filter.PageSize
            );
        }
    }
}