using MediatR;
using Siniestros.Contracts.DTOs;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Commands
{
    public record LoginCommand(string Username, string Password) : IRequest<Result<AuthDto>>;
}