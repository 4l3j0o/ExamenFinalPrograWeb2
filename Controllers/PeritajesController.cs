using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitoTarijaAPI.Data;
using TransitoTarijaAPI.DTOs;
using TransitoTarijaAPI.Models;

namespace TransitoTarijaAPI.Controllers;

[ApiController]
[Route("api/v1/peritajes")]
[Authorize]
public class PeritajesController : ControllerBase
{
    private readonly AppDbContext _db;

    public PeritajesController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarPeritajeRequest req)
    {
        if (!await _db.Accidentes.AnyAsync(a => a.Id == req.AccidenteId))
            return NotFound(new { error = "Accidente no encontrado" });

        var peritaje = new Peritaje
        {
            AccidenteId                 = req.AccidenteId,
            PeritoId                    = req.PeritoId,
            FechaPeritaje               = req.FechaPeritaje,
            VelocidadEstimadaImpactoKmh = req.VelocidadEstimadaImpactoKmh,
            HuellaFrenadoMetros         = req.HuellaFrenadoMetros,
            FactorDeterminante          = req.FactorDeterminante,
            Conclusiones                = req.Conclusiones,
            CroquisUrl                  = req.CroquisUrl,
        };

        _db.Peritajes.Add(peritaje);
        await _db.SaveChangesAsync();

        return CreatedAtAction(null, null,
            new { peritaje.Id, peritaje.AccidenteId, mensaje = "Peritaje registrado correctamente" });
    }
}
