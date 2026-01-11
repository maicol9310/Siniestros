using Microsoft.EntityFrameworkCore;
using Siniestros.Domain.Aggregates;

namespace Siniestros.Infrastructure.Persistence
{
    public class SiniestroDbContext : DbContext
    {
        public SiniestroDbContext(DbContextOptions<SiniestroDbContext> options) : base(options) { }

        public DbSet<Siniestro> Siniestros { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Configurations.SiniestroConfiguration());
        }
    }
}
