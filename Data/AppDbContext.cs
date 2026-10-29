using Microsoft.EntityFrameworkCore;
using TransitoTarijaAPI.Models;

namespace TransitoTarijaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Oficial> Oficiales { get; set; }
    public DbSet<Accidente> Accidentes { get; set; }
    public DbSet<VehiculoInvolucrado> VehiculosInvolucrados { get; set; }
    public DbSet<PersonaInvolucrada> PersonasInvolucradas { get; set; }
    public DbSet<Peritaje> Peritajes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Accidente>()
            .Property(a => a.Gravedad)
            .HasDefaultValue("solo_danos");

        modelBuilder.Entity<Accidente>()
            .Property(a => a.EstadoCaso)
            .HasDefaultValue("en_investigacion");

        modelBuilder.Entity<Accidente>()
            .HasIndex(a => a.CodigoCaso)
            .IsUnique();

        modelBuilder.Entity<Oficial>()
            .HasIndex(o => o.NumeroEscalafon)
            .IsUnique();

        modelBuilder.Entity<VehiculoInvolucrado>()
            .HasOne(v => v.Accidente)
            .WithMany(a => a.Vehiculos)
            .HasForeignKey(v => v.AccidenteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PersonaInvolucrada>()
            .HasOne(p => p.Accidente)
            .WithMany(a => a.Personas)
            .HasForeignKey(p => p.AccidenteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Peritaje>()
            .HasOne(p => p.Accidente)
            .WithMany(a => a.Peritajes)
            .HasForeignKey(p => p.AccidenteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Accidente>()
            .HasOne(a => a.OficialAsignado)
            .WithMany(o => o.AccidentesAsignados)
            .HasForeignKey(a => a.OficialAsignadoId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Peritaje>()
            .HasOne(p => p.Perito)
            .WithMany(o => o.Peritajes)
            .HasForeignKey(p => p.PeritoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
