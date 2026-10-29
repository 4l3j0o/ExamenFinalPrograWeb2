using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TransitoTarijaAPI.Models;

[Table("personas_involucradas")]
public class PersonaInvolucrada
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AccidenteId { get; set; }

    [ForeignKey("AccidenteId")]
    [JsonIgnore]
    public Accidente? Accidente { get; set; }

    public int? VehiculoId { get; set; }

    [ForeignKey("VehiculoId")]
    [JsonIgnore]
    public VehiculoInvolucrado? Vehiculo { get; set; }

    [Required]
    public string Nombres { get; set; } = string.Empty;

    [Required]
    public string Apellidos { get; set; } = string.Empty;

    public string? Ci { get; set; }
    public string? Telefono { get; set; }
    public string? Rol { get; set; }
    public string? NumeroLicencia { get; set; }
    public string? CategoriaLicencia { get; set; }
    public double GradoAlcoholemiaGral { get; set; } = 0.0;
    public string EstadoAlcoholemia { get; set; } = "sobrio";
    public string? NumeroActaDosaje { get; set; }
    public string? Laboratorio { get; set; }
    public string? EstadoSalud { get; set; }
    public string? CentroMedicoTraslado { get; set; }
    public string? DiagnosticoPreliminar { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
