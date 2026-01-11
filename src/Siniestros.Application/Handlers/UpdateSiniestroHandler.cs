using MediatR;
using Microsoft.Extensions.Logging;
using Siniestros.Application.Commands;
using Siniestros.Application.Interfaces;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Handlers
{
    public class UpdateSiniestroHandler : IRequestHandler<UpdateSiniestroCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UpdateSiniestroHandler> _logger;

        public UpdateSiniestroHandler(IUnitOfWork uow, ILogger<UpdateSiniestroHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateSiniestroCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating siniestro {Id}", request.Id);

            var siniestro = await _uow.Siniestro.GetByIdAsync(request.Id, cancellationToken);
            if (siniestro == null)
            {
                _logger.LogWarning("Siniestro {Id} not found", request.Id);
                return Result.Failure("Siniestro not found");
            }

            try
            {
                siniestro.Update(
                    request.FechaHora,
                    request.Departamento,
                    request.Ciudad,
                    request.Tipo,
                    request.VehiculosInvolucrados,
                    request.NumeroVictimas,
                    request.Descripcion
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Failed updating siniestro: {Error}", ex.Message);
                return Result.Failure(ex.Message);
            }

            _uow.Siniestro.Update(siniestro);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Siniestro {Id} updated successfully", request.Id);
            return Result.Success();
        }
    }
}
