using FluentValidation;
using Siniestros.Application.Commands;

public class CreateSiniestroCommandValidator : AbstractValidator<CreateSiniestroCommand>
{
    public CreateSiniestroCommandValidator()
    {
        RuleFor(x => x.Departamento).NotEmpty();
        RuleFor(x => x.Ciudad).NotEmpty();
        RuleFor(x => x.VehiculosInvolucrados).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NumeroVictimas).GreaterThanOrEqualTo(0);
    }
}
