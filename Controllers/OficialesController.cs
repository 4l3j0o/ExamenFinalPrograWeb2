using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransitoTarijaAPI.Data;
using TransitoTarijaAPI.DTOs;
using TransitoTarijaAPI.Models;
using BC = BCrypt.Net.BCrypt;

namespace TransitoTarijaAPI.Controllers;

[ApiController]
[Route("api/v1/oficiales")]
[Authorize]
public class OficialesController : ControllerBase
{
    private readonly AppDbContext _db;

    public OficialesController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarOficialRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Nombres) ||
            string.IsNullOrWhiteSpace(req.Apellidos) ||
            string.IsNullOrWhiteSpace(req.NumeroEscalafon))
            return BadRequest(new { error = "Nombres, Apellidos y NumeroEscalafon son requeridos" });

        bool existe = _db.Oficiales.Any(o => o.NumeroEscalafon == req.NumeroEscalafon);
        if (existe)
            return Conflict(new { error = "El número de escalafón ya existe" });

        var rawPass  = req.Password ?? $"{req.NumeroEscalafon}@Transito2026";
        var passHash = BC.HashPassword(rawPass);

        var oficial = new Oficial
        {
            Nombres         = req.Nombres,
            Apellidos       = req.Apellidos,
            Grado           = req.Grado,
            NumeroEscalafon = req.NumeroEscalafon,
            Cargo           = req.Cargo,
            Unidad          = req.Unidad,
            Activo          = req.Activo,
            PasswordHash    = passHash,
        };

        _db.Oficiales.Add(oficial);
        await _db.SaveChangesAsync();

        return CreatedAtAction(null, null, new
        {
            oficial.Id,
            oficial.Nombres,
            oficial.Apellidos,
            oficial.Grado,
            oficial.NumeroEscalafon,
            oficial.Cargo,
            oficial.Unidad,
            oficial.Activo,
            passwordTemporal = req.Password is null ? rawPass : null,
            mensaje = "Oficial registrado correctamente"
        });
    }
}
