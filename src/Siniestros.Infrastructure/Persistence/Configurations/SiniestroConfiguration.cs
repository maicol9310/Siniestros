using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Siniestros.Domain.Aggregates;

namespace Siniestros.Infrastructure.Persistence.Configurations
{
    public class SiniestroConfiguration : IEntityTypeConfiguration<Siniestro>
    {
        public void Configure(EntityTypeBuilder<Siniestro> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FechaHora)
                .IsRequired();

            builder.Property(s => s.Departamento)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Ciudad)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Tipo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.VehiculosInvolucrados)
                .IsRequired();

            builder.Property(s => s.NumeroVictimas)
                .IsRequired();

            builder.Property(s => s.Descripcion)
                .HasMaxLength(500);
        }
    }
}
