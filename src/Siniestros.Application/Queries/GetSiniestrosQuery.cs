using MediatR;
using Siniestros.Contracts.DTOs;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Queries
{
    public record GetSiniestrosQuery(
        SiniestroFilterDto Filter
    ) : IRequest<Result<PagedResult<SiniestroDto>>>;
}
