using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Siniestros.Application.Interfaces;
using Siniestros.Application.Queries;
using Siniestros.Contracts.DTOs;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Handlers
{
    public class GetSiniestroByIdHandler : IRequestHandler<GetSiniestroByIdQuery, Result<SiniestroDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSiniestroByIdHandler> _logger;

        public GetSiniestroByIdHandler(IUnitOfWork uow, IMapper mapper, ILogger<GetSiniestroByIdHandler> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<SiniestroDto>> Handle(GetSiniestroByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var siniestro = await _uow.Siniestro.GetByIdAsync(request.Id, cancellationToken);
                if (siniestro == null)
                {
                    _logger.LogWarning("Siniestro not found: {Id}", request.Id);
                    return Result<SiniestroDto>.Failure("Siniestro not found");
                }

                var dto = _mapper.Map<SiniestroDto>(siniestro);
                return Result<SiniestroDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching siniestro by Id");
                return Result<SiniestroDto>.Failure(ex.Message);
            }
        }
    }
}
