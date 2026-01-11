using Siniestros.Domain.Enum;

namespace Siniestros.Domain.Aggregates
{
    public class Siniestro
    {
        public Guid Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Departamento { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public TipoSiniestro Tipo { get; set; }
        public int VehiculosInvolucrados { get; set; }
        public int NumeroVictimas { get; set; }
        public string? Descripcion { get; set; }

        public Siniestro() { }

        public Siniestro(
            DateTime fechaHora,
            string departamento,
            string ciudad,
            TipoSiniestro tipo,
            int vehiculos,
            int victimas,
            string? descripcion)
        {
            Id = Guid.NewGuid();
            FechaHora = fechaHora;
            Departamento = departamento ?? throw new ArgumentNullException(nameof(departamento));
            Ciudad = ciudad ?? throw new ArgumentNullException(nameof(ciudad));
            Tipo = tipo;
            VehiculosInvolucrados = vehiculos;
            NumeroVictimas = victimas;
            Descripcion = descripcion;
        }

        public void Update(DateTime fechaHora,
            string departamento,
            string ciudad,
            TipoSiniestro tipo,
            int vehiculos,
            int victimas,
            string? descripcion)
        {

            FechaHora = fechaHora;
            Departamento = departamento;
            Ciudad = ciudad;
            Tipo = tipo;
            VehiculosInvolucrados = vehiculos;
            NumeroVictimas = victimas;
            Descripcion = descripcion;
        }
    }
}
