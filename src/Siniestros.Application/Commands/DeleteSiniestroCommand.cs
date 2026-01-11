using MediatR;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Commands
{
    public record DeleteSiniestroCommand(Guid Id) : IRequest<Result>;
}
