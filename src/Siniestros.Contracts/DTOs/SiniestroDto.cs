namespace Siniestros.Contracts.DTOs
{
    public class SiniestroDto
    {
        public Guid Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Departamento { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public int VehiculosInvolucrados { get; set; }
        public int NumeroVictimas { get; set; }
        public string? Descripcion { get; set; }
    }
}