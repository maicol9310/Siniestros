namespace Siniestros.Contracts.DTOs
{
    public class SiniestroFilterDto
    {
        public string? Departamento { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
