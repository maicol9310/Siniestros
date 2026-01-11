public class Siniestro
{
    public Guid Id { get; private set; }
    public DateTime FechaHora { get; private set; }
    public string Departamento { get; private set; } = null!;
    public string Ciudad { get; private set; } = null!;
    public TipoSiniestro Tipo { get; private set; }
    public int VehiculosInvolucrados { get; private set; }
    public int NumeroVictimas { get; private set; }
    public string? Descripcion { get; private set; }

    private Siniestro() { } 

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
}
