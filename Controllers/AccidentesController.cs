using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitoTarijaAPI.Data;
using TransitoTarijaAPI.DTOs;
using TransitoTarijaAPI.Models;

namespace TransitoTarijaAPI.Controllers;

[ApiController]
[Route("api/v1/accidentes")]
[Authorize]
public class AccidentesController : ControllerBase
{
    private readonly AppDbContext _db;

    public AccidentesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] string? distrito,
        [FromQuery] string? gravedad,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] int page  = 1,
        [FromQuery] int limit = 20)
    {
        var query = _db.Accidentes
            .Include(a => a.OficialAsignado)
            .AsQueryable();

        if (!string.IsNullOrEmpty(distrito))
            query = query.Where(a => a.Distrito == distrito);

        if (!string.IsNullOrEmpty(gravedad))
            query = query.Where(a => a.Gravedad == gravedad);

        if (fechaDesde.HasValue)
            query = query.Where(a => a.FechaHora >= fechaDesde.Value);

        var total = await query.CountAsync();
        var data  = await query
            .OrderByDescending(a => a.FechaHora)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(a => new
            {
                a.Id, a.CodigoCaso, a.FechaHora, a.TipoAccidente,
                a.Gravedad, a.ZonaBarrio, a.DireccionReferencia,
                a.Distrito, a.Latitud, a.Longitud,
                a.CondicionClimatica, a.EstadoCalzada,
                a.EstadoCaso, a.Observaciones, a.CreatedAt,
                OficialNombre = a.OficialAsignado != null
                    ? a.OficialAsignado.Nombres + " " + a.OficialAsignado.Apellidos
                    : null
            })
            .ToListAsync();

        return Ok(new { total, page, limit, data });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Detalle(int id)
    {
        var accidente = await _db.Accidentes
            .Include(a => a.OficialAsignado)
            .Include(a => a.Vehiculos)
            .Include(a => a.Personas)
            .Include(a => a.Peritajes)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (accidente is null)
            return NotFound(new { error = "Accidente no encontrado" });

        return Ok(new
        {
            accidente.Id, accidente.CodigoCaso, accidente.FechaHora,
            accidente.TipoAccidente, accidente.Gravedad, accidente.ZonaBarrio,
            accidente.DireccionReferencia, accidente.Distrito,
            accidente.Latitud, accidente.Longitud,
            accidente.CondicionClimatica, accidente.EstadoCalzada,
            accidente.EstadoCaso, accidente.JuzgadoOInstitucion,
            accidente.NumeroCuadernoInvestigacion, accidente.MotivoPase,
            accidente.Observaciones, accidente.CreatedAt,
            oficial = accidente.OficialAsignado is null ? null : new
            {
                accidente.OficialAsignado.Id,
                accidente.OficialAsignado.Nombres,
                accidente.OficialAsignado.Apellidos,
                accidente.OficialAsignado.Grado,
                accidente.OficialAsignado.NumeroEscalafon
            },
            vehiculos = accidente.Vehiculos,
            personas  = accidente.Personas,
            peritajes = accidente.Peritajes
        });
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarAccidenteRequest req)
    {
        var gravedades = new[] { "solo_danos", "con_heridos", "fatal" };
        if (!gravedades.Contains(req.Gravedad))
            return BadRequest(new { error = $"gravedad debe ser: {string.Join(", ", gravedades)}" });

        if (await _db.Accidentes.AnyAsync(a => a.CodigoCaso == req.CodigoCaso))
            return Conflict(new { error = "El código de caso ya existe" });

        var acc = new Accidente
        {
            CodigoCaso          = req.CodigoCaso,
            FechaHora           = req.FechaHora,
            TipoAccidente       = req.TipoAccidente,
            Gravedad            = req.Gravedad,
            ZonaBarrio          = req.ZonaBarrio,
            DireccionReferencia = req.DireccionReferencia,
            Distrito            = req.Distrito,
            Latitud             = req.Latitud,
            Longitud            = req.Longitud,
            CondicionClimatica  = req.CondicionClimatica,
            EstadoCalzada       = req.EstadoCalzada,
            OficialAsignadoId   = req.OficialAsignadoId,
            Observaciones       = req.Observaciones,
        };

        _db.Accidentes.Add(acc);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Detalle), new { id = acc.Id },
            new { acc.Id, acc.CodigoCaso, mensaje = "Accidente registrado" });
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoRequest req)
    {
        var estados = new[] { "en_investigacion", "conciliado", "remitido_fiscalia", "cerrado" };
        if (req.EstadoCaso is not null && !estados.Contains(req.EstadoCaso))
            return BadRequest(new { error = $"estadoCaso debe ser: {string.Join(", ", estados)}" });

        var acc = await _db.Accidentes.FindAsync(id);
        if (acc is null) return NotFound(new { error = "Accidente no encontrado" });

        if (req.EstadoCaso is not null)                  acc.EstadoCaso                    = req.EstadoCaso;
        if (req.JuzgadoOInstitucion is not null)         acc.JuzgadoOInstitucion           = req.JuzgadoOInstitucion;
        if (req.NumeroCuadernoInvestigacion is not null) acc.NumeroCuadernoInvestigacion   = req.NumeroCuadernoInvestigacion;
        if (req.MotivoPase is not null)                  acc.MotivoPase                    = req.MotivoPase;

        await _db.SaveChangesAsync();
        return Ok(new { mensaje = "Estado actualizado", accidente = acc });
    }

    [HttpGet("{id:int}/vehiculos")]
    public async Task<IActionResult> ListarVehiculos(int id)
    {
        if (!await _db.Accidentes.AnyAsync(a => a.Id == id))
            return NotFound(new { error = "Accidente no encontrado" });

        var vehiculos = await _db.VehiculosInvolucrados
            .Where(v => v.AccidenteId == id)
            .ToListAsync();

        return Ok(new { accidenteId = id, total = vehiculos.Count, vehiculos });
    }
}
