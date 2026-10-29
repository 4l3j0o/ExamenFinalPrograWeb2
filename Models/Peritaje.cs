using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TransitoTarijaAPI.Models;

[Table("peritajes")]
public class Peritaje
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AccidenteId { get; set; }

    [ForeignKey("AccidenteId")]
    [JsonIgnore]
    public Accidente? Accidente { get; set; }

    public int? PeritoId { get; set; }

    [ForeignKey("PeritoId")]
    [JsonIgnore]
    public Oficial? Perito { get; set; }

    public DateOnly? FechaPeritaje { get; set; }
    public double? VelocidadEstimadaImpactoKmh { get; set; }
    public double? HuellaFrenadoMetros { get; set; }
    public string? FactorDeterminante { get; set; }
    public string? Conclusiones { get; set; }
    public string? CroquisUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
