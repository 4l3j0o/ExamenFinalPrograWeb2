using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitoTarijaAPI.Models;

[Table("oficiales")]
public class Oficial
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nombres { get; set; } = string.Empty;

    [Required]
    public string Apellidos { get; set; } = string.Empty;

    public string? Grado { get; set; }

    [Required]
    public string NumeroEscalafon { get; set; } = string.Empty;

    public string? Cargo { get; set; }

    public string? Unidad { get; set; }

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Accidente> AccidentesAsignados { get; set; } = [];
    public ICollection<Peritaje> Peritajes { get; set; } = [];
}
