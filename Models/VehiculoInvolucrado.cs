using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TransitoTarijaAPI.Models;

[Table("vehiculos_involucrados")]
public class VehiculoInvolucrado
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AccidenteId { get; set; }

    [ForeignKey("AccidenteId")]
    [JsonIgnore]
    public Accidente? Accidente { get; set; }

    [Required]
    public string Placa { get; set; } = string.Empty;

    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? TipoVehiculo { get; set; }
    public string? Color { get; set; }
    public string? NumeroChasis { get; set; }
    public bool SoatVigente { get; set; }
    public string? Aseguradora { get; set; }
    public string? DescripcionDanios { get; set; }
    public string? RemolcadoA { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
