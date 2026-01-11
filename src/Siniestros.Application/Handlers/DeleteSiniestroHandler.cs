using MediatR;
using Microsoft.Extensions.Logging;
using Siniestros.Application.Commands;
using Siniestros.Application.Interfaces;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Handlers
{
    public class DeleteSiniestroHandler
        : IRequestHandler<DeleteSiniestroCommand, Result>
    {
        private readonly ISiniestroRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteSiniestroHandler> _logger;

        public DeleteSiniestroHandler(
            ISiniestroRepository repository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteSiniestroHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DeleteSiniestroCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing delete siniestro request with Id {SiniestroId}",
                request.Id
            );

            var siniestro = await _repository.GetByIdAsync(
                request.Id,
                cancellationToken
            );

            if (siniestro is null)
            {
                _logger.LogWarning(
                    "Siniestro with Id {SiniestroId} not found",
                    request.Id
                );

                return Result.Failure("Siniestro not found");
            }

            await _repository.DeleteAsync(request.Id, cancellationToken);

            // 🔥 ESTO ES LO QUE TE FALTABA
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Siniestro with Id {SiniestroId} successfully deleted",
                request.Id
            );

            return Result.Success();
        }
    }
}