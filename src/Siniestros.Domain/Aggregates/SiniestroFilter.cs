namespace Siniestros.Domain.Aggregates
{
    public record SiniestroFilter(
        string? Departamento,
        DateTime? FechaInicio,
        DateTime? FechaFin,
        int Page,
        int PageSize
    );
}
