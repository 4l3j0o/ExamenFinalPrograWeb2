using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitoTarijaAPI.Data;

namespace TransitoTarijaAPI.Controllers;

[ApiController]
[Route("api/v1/estadisticas")]
[Authorize]
public class EstadisticasController : ControllerBase
{
    private readonly AppDbContext _db;

    public EstadisticasController(AppDbContext db) => _db = db;

    [HttpGet("puntos-criticos")]
    public async Task<IActionResult> PuntosCriticos([FromQuery] int? anio)
    {
        int year = anio ?? DateTime.UtcNow.Year;

        var accidentesDelAnio = _db.Accidentes
            .Where(a => a.FechaHora.Year == year);

        var resumen = new
        {
            total      = await accidentesDelAnio.CountAsync(),
            fatales    = await accidentesDelAnio.CountAsync(a => a.Gravedad == "fatal"),
            conHeridos = await accidentesDelAnio.CountAsync(a => a.Gravedad == "con_heridos"),
            soloDanos  = await accidentesDelAnio.CountAsync(a => a.Gravedad == "solo_danos"),
        };

        var puntos = await accidentesDelAnio
            .Where(a => a.Latitud != null && a.Longitud != null)
            .GroupBy(a => new { a.Latitud, a.Longitud, a.Distrito, a.ZonaBarrio, a.DireccionReferencia })
            .Select(g => new
            {
                g.Key.Distrito,
                g.Key.ZonaBarrio,
                g.Key.DireccionReferencia,
                g.Key.Latitud,
                g.Key.Longitud,
                TotalAccidentes = g.Count(),
                Fatales = g.Count(a => a.Gravedad == "fatal"),
                ConHeridos = g.Count(a => a.Gravedad == "con_heridos"),
                SoloDanos = g.Count(a => a.Gravedad == "solo_danos"),
            })
            .OrderByDescending(p => p.TotalAccidentes)
            .ToListAsync();

        var topDistritos = await accidentesDelAnio
            .Where(a => a.Distrito != null)
            .GroupBy(a => a.Distrito)
            .Select(g => new { Distrito = g.Key, Total = g.Count() })
            .OrderByDescending(d => d.Total)
            .Take(10)
            .ToListAsync();

        return Ok(new { anio = year, resumen, topDistritos, puntos });
    }
}
