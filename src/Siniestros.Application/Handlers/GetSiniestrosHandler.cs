using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Siniestros.Application.Interfaces;
using Siniestros.Application.Queries;
using Siniestros.Contracts.DTOs;
using Siniestros.Domain.Aggregates;
using Siniestros.SharedKernel;

namespace Siniestros.Application.Handlers
{
    public class GetSiniestrosHandler
        : IRequestHandler<GetSiniestrosQuery, Result<PagedResult<SiniestroDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSiniestrosHandler> _logger;

        public GetSiniestrosHandler(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<GetSiniestrosHandler> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PagedResult<SiniestroDto>>> Handle(
            GetSiniestrosQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting siniestros with filters");

            var filter = new SiniestroFilter(
                request.Filter.Departamento,
                request.Filter.FechaInicio,
                request.Filter.FechaFin,
                request.Filter.Page,
                request.Filter.PageSize
            );

            var result = await _uow.Siniestro.ListAsync(filter, cancellationToken);

            var dtoItems = _mapper.Map<List<SiniestroDto>>(result.Items);

            var pagedDto = new PagedResult<SiniestroDto>(
                dtoItems,
                result.TotalItems,
                result.Page,
                result.PageSize
            );

            return Result<PagedResult<SiniestroDto>>.Success(pagedDto);
        }
    }
}
