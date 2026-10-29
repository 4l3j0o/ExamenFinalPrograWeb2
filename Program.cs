using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TransitoTarijaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "transito_tarija_secret_2026";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer           = false,
            ValidateAudience         = false,
            ClockSkew                = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(opt =>
    opt.AddPolicy("Dev", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors("Dev");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
