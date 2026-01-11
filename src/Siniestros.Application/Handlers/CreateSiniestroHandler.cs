using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Siniestros.Application.Commands;
using Siniestros.Application.Interfaces;
using Siniestros.Contracts.DTOs;
using Siniestros.Domain.Aggregates;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Handlers
{
    public class CreateSiniestroHandler : IRequestHandler<CreateSiniestroCommand, Result<SiniestroDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSiniestroHandler> _logger;

        public CreateSiniestroHandler(IUnitOfWork uow, IMapper mapper, ILogger<CreateSiniestroHandler> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<SiniestroDto>> Handle(CreateSiniestroCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating siniestro in {Departamento} on {Fecha}", request.Departamento, request.FechaHora);

            Siniestro siniestro;
            try
            {
                siniestro = new Siniestro(
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
                _logger.LogWarning("Siniestro creation failed: {Error}", ex.Message);
                return Result<SiniestroDto>.Failure(ex.Message);
            }

            await _uow.Siniestro.AddAsync(siniestro, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<SiniestroDto>(siniestro);
            return Result<SiniestroDto>.Success(dto);
        }
    }
}
