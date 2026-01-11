using MediatR;
using Microsoft.AspNetCore.Mvc;
using Siniestros.Application.Commands;
using Siniestros.Contracts.DTOs;
using Siniestros.SharedKernel;

namespace Siniestros.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand cmd)
        {
            try
            {
                var result = await _mediator.Send(cmd);
                return Ok(result);
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new Result<AuthDto>
                {
                    IsSuccess = false,
                    Error = string.Join(", ", errors)
                });
            }
        }
    }
}