using MediatR;
using Siniestros.Contracts.DTOs;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Queries
{
    public record GetSiniestroByIdQuery(Guid Id) : IRequest<Result<SiniestroDto>>;
}