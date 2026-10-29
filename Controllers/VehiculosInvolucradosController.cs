using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransitoTarijaAPI.Data;
using TransitoTarijaAPI.DTOs;
using TransitoTarijaAPI.Models;

namespace TransitoTarijaAPI.Controllers;

[ApiController]
[Route("api/v1/vehiculos-involucrados")]
[Authorize]
public class VehiculosInvolucradosController : ControllerBase
{
    private readonly AppDbContext _db;

    public VehiculosInvolucradosController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Asociar([FromBody] AsociarVehiculoRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Placa))
            return BadRequest(new { error = "Placa es requerida" });

        if (!await _db.Accidentes.AnyAsync(a => a.Id == req.AccidenteId))
            return NotFound(new { error = "Accidente no encontrado" });

        var vehiculo = new VehiculoInvolucrado
        {
            AccidenteId       = req.AccidenteId,
            Placa             = req.Placa,
            Marca             = req.Marca,
            Modelo            = req.Modelo,
            TipoVehiculo      = req.TipoVehiculo,
            Color             = req.Color,
            NumeroChasis      = req.NumeroChasis,
            SoatVigente       = req.SoatVigente,
            Aseguradora       = req.Aseguradora,
            DescripcionDanios = req.DescripcionDanios,
            RemolcadoA        = req.RemolcadoA,
        };

        _db.VehiculosInvolucrados.Add(vehiculo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(null, null,
            new { vehiculo.Id, vehiculo.AccidenteId, vehiculo.Placa, mensaje = "Vehículo registrado correctamente" });
    }
}
