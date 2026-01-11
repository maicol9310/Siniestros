using MediatR;
using Siniestros.Domain.Enum;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Commands
{
    public record UpdateSiniestroCommand(
        Guid Id,
        DateTime FechaHora,
        string Departamento,
        string Ciudad,
        TipoSiniestro Tipo,
        int VehiculosInvolucrados,
        int NumeroVictimas,
        string? Descripcion
    ) : IRequest<Result>;
}
