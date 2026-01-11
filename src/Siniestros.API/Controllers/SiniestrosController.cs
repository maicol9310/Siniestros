using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siniestros.Application.Commands;
using Siniestros.Application.Queries;
using Siniestros.Contracts.DTOs;
using Siniestros.Domain.Enum;

namespace Siniestros.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireUser")]
    public class SiniestrosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SiniestrosController> _logger;

        public SiniestrosController(IMediator mediator, IMapper mapper, ILogger<SiniestrosController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSiniestroRequest request)
        {
            _logger.LogInformation("Creating siniestro in {Departamento}", request.Departamento);

            var cmd = new CreateSiniestroCommand(
                request.FechaHora,
                request.Departamento,
                request.Ciudad,
                request.Tipo,
                request.VehiculosInvolucrados,
                request.NumeroVictimas,
                request.Descripcion
            );

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to create siniestro: {Error}", result.Error);
                    return BadRequest(new { error = result.Error });
                }

                _logger.LogInformation("Siniestro created successfully: {SiniestroId}", result.Value!.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Validation failed while creating siniestro: {Errors}", errors);
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }

        // GET ALL / PAGINATED
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SiniestroFilterDto filterDto)
        {
            _logger.LogInformation("Fetching siniestros with filters: {Filter}", filterDto);

            var query = new GetSiniestrosQuery(filterDto); // <-- ahora coincide con la query
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch siniestros: {Error}", result.Error);
                return BadRequest(new { error = result.Error });
            }

            var dtos = _mapper.Map<IEnumerable<SiniestroDto>>(result.Value!.Items);
            return Ok(new
            {
                Items = dtos,
                result.Value.TotalItems,
                result.Value.Page,
                result.Value.PageSize
            });
        }


        // GET BY ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching siniestro by Id: {SiniestroId}", id);

            var query = new GetSiniestroByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Siniestro not found: {SiniestroId}", id);
                return NotFound(new { error = result.Error });
            }

            var dto = _mapper.Map<SiniestroDto>(result.Value!);
            return Ok(dto);
        }

        // UPDATE
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSiniestroRequest request)
        {
            _logger.LogInformation("Updating siniestro {SiniestroId}", id);

            var cmd = new UpdateSiniestroCommand(
                id,
                request.FechaHora,
                request.Departamento,
                request.Ciudad,
                request.Tipo,
                request.VehiculosInvolucrados,
                request.NumeroVictimas,
                request.Descripcion
            );

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to update siniestro {SiniestroId}: {Error}", id, result.Error);
                    return BadRequest(new { error = result.Error });
                }

                _logger.LogInformation("Siniestro updated successfully {SiniestroId}", id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Validation failed while updating siniestro {SiniestroId}: {Errors}", id, errors);
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }

        // DELETE
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting siniestro {SiniestroId}", id);

            var cmd = new DeleteSiniestroCommand(id);
            var result = await _mediator.Send(cmd);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to delete siniestro {SiniestroId}: {Error}", id, result.Error);
                return BadRequest(new { error = result.Error });
            }

            _logger.LogInformation("Siniestro deleted successfully {SiniestroId}", id);
            return NoContent();
        }
    }

    public record CreateSiniestroRequest(
        DateTime FechaHora,
        string Departamento,
        string Ciudad,
        TipoSiniestro Tipo,
        int VehiculosInvolucrados,
        int NumeroVictimas,
        string? Descripcion
    );

    public record UpdateSiniestroRequest(
        DateTime FechaHora,
        string Departamento,
        string Ciudad,
        TipoSiniestro Tipo,
        int VehiculosInvolucrados,
        int NumeroVictimas,
        string? Descripcion
    );
}
