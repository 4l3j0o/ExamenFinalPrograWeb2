using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TransitoTarijaAPI.Data;
using TransitoTarijaAPI.DTOs;
using TransitoTarijaAPI.Models;
using BC = BCrypt.Net.BCrypt;

namespace TransitoTarijaAPI.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var oficial = await _db.Oficiales
            .FirstOrDefaultAsync(o => o.NumeroEscalafon == req.NumeroEscalafon && o.Activo);

        if (oficial is null || !BC.Verify(req.Password, oficial.PasswordHash))
            return Unauthorized(new { error = "Credenciales inválidas" });

        var token = GenerarToken(oficial);

        return Ok(new LoginResponse(token, new OficialDto(
            oficial.Id, oficial.Nombres, oficial.Apellidos,
            oficial.Grado, oficial.Cargo, oficial.Unidad)));
    }

    private string GenerarToken(Oficial oficial)
    {
        var secret = _config["Jwt:Secret"] ?? "transito_tarija_secret_2026";
        var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,  oficial.Id.ToString()),
            new Claim("escalafon", oficial.NumeroEscalafon),
            new Claim("cargo",     oficial.Cargo ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expires = int.TryParse(_config["Jwt:ExpiresHours"], out var h) ? h : 8;

        var jwt = new JwtSecurityToken(
            claims:   claims,
            expires:  DateTime.UtcNow.AddHours(expires),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
