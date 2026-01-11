using MediatR;
using Siniestros.Contracts.DTOs;
using Siniestros.Domain.Enum;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Commands
{
    public record CreateSiniestroCommand(
        DateTime FechaHora,
        string Departamento,
        string Ciudad,
        TipoSiniestro Tipo,
        int VehiculosInvolucrados,
        int NumeroVictimas,
        string? Descripcion
    ) : IRequest<Result<SiniestroDto>>;
}
