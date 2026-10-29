using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TransitoTarijaAPI.Models;

[Table("accidentes")]
public class Accidente
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string CodigoCaso { get; set; } = string.Empty;

    [Required]
    public DateTime FechaHora { get; set; }

    public string? TipoAccidente { get; set; }

    [Required]
    public string Gravedad { get; set; } = "solo_danos";

    public string? ZonaBarrio { get; set; }
    public string? DireccionReferencia { get; set; }
    public string? Distrito { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? CondicionClimatica { get; set; }
    public string? EstadoCalzada { get; set; }

    public int? OficialAsignadoId { get; set; }

    [ForeignKey("OficialAsignadoId")]
    [JsonIgnore]
    public Oficial? OficialAsignado { get; set; }

    public string EstadoCaso { get; set; } = "en_investigacion";

    public string? JuzgadoOInstitucion { get; set; }
    public string? NumeroCuadernoInvestigacion { get; set; }
    public string? MotivoPase { get; set; }
    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<VehiculoInvolucrado> Vehiculos { get; set; } = [];
    public ICollection<PersonaInvolucrada> Personas { get; set; } = [];
    public ICollection<Peritaje> Peritajes { get; set; } = [];
}
