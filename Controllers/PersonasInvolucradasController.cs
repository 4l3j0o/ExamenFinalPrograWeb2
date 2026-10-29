using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitoTarijaAPI.Data;
using TransitoTarijaAPI.DTOs;
using TransitoTarijaAPI.Models;

namespace TransitoTarijaAPI.Controllers;

[ApiController]
[Route("api/v1/personas-involucradas")]
[Authorize]
public class PersonasInvolucradasController : ControllerBase
{
    private readonly AppDbContext _db;

    public PersonasInvolucradasController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarPersonaRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Nombres) || string.IsNullOrWhiteSpace(req.Apellidos))
            return BadRequest(new { error = "Nombres y Apellidos son requeridos" });

        if (!await _db.Accidentes.AnyAsync(a => a.Id == req.AccidenteId))
            return NotFound(new { error = "Accidente no encontrado" });

        var roles = new[] { "conductor", "pasajero", "peaton", "ciclista" };
        if (req.Rol is not null && !roles.Contains(req.Rol))
            return BadRequest(new { error = $"Rol debe ser: {string.Join(", ", roles)}" });

        var persona = new PersonaInvolucrada
        {
            AccidenteId           = req.AccidenteId,
            VehiculoId            = req.VehiculoId,
            Nombres               = req.Nombres,
            Apellidos             = req.Apellidos,
            Ci                    = req.Ci,
            Telefono              = req.Telefono,
            Rol                   = req.Rol,
            NumeroLicencia        = req.NumeroLicencia,
            CategoriaLicencia     = req.CategoriaLicencia,
            GradoAlcoholemiaGral  = req.GradoAlcoholemiaGral,
            EstadoAlcoholemia     = req.EstadoAlcoholemia ?? "sobrio",
            EstadoSalud           = req.EstadoSalud,
            CentroMedicoTraslado  = req.CentroMedicoTraslado,
            DiagnosticoPreliminar = req.DiagnosticoPreliminar,
        };

        _db.PersonasInvolucradas.Add(persona);
        await _db.SaveChangesAsync();

        return CreatedAtAction(null, null,
            new { persona.Id, persona.AccidenteId, persona.Nombres, persona.Apellidos, mensaje = "Persona registrada correctamente" });
    }

    [HttpPatch("{id:int}/alcoholemia")]
    public async Task<IActionResult> ActualizarAlcoholemia(int id, [FromBody] ActualizarAlcoholemiaRequest req)
    {
        var persona = await _db.PersonasInvolucradas.FindAsync(id);
        if (persona is null)
            return NotFound(new { error = "Persona no encontrada" });

        if (req.GradoAlcoholemiaGral.HasValue) persona.GradoAlcoholemiaGral = req.GradoAlcoholemiaGral.Value;
        if (req.EstadoAlcoholemia is not null)  persona.EstadoAlcoholemia    = req.EstadoAlcoholemia;
        if (req.NumeroActaDosaje is not null)   persona.NumeroActaDosaje     = req.NumeroActaDosaje;
        if (req.Laboratorio is not null)        persona.Laboratorio          = req.Laboratorio;

        await _db.SaveChangesAsync();
        return Ok(new { mensaje = "Prueba de alcoholemia actualizada", persona });
    }
}
