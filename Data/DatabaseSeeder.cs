using Bogus;
using Microsoft.EntityFrameworkCore;
using TransitoTarijaAPI.Models;
using BC = BCrypt.Net.BCrypt;

namespace TransitoTarijaAPI.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Oficiales.AnyAsync())
        {
            return;
        }

        Randomizer.Seed = new Random(8888);

        var defaultPasswordHash = BC.HashPassword("123456");

        var grados = new[] { "Sargento Primero", "Sargento Segundo", "Subteniente", "Teniente", "Capitan", "Mayor", "Coronel" };
        var cargos = new[] { "Investigador de Accidentes", "Perito Vial", "Patrullero", "Jefe de Division", "Oficial de Guardia" };
        var unidades = new[] { "Division Especiales", "Division Accidentes", "Patrulla Urbana", "Brigada de Transito" };

        var oficialesFaker = new Faker<Oficial>("es")
            .RuleFor(o => o.Nombres, f => f.Name.FirstName())
            .RuleFor(o => o.Apellidos, f => f.Name.LastName())
            .RuleFor(o => o.Grado, f => f.PickRandom(grados))
            .RuleFor(o => o.Cargo, f => f.PickRandom(cargos))
            .RuleFor(o => o.Unidad, f => f.PickRandom(unidades))
            .RuleFor(o => o.PasswordHash, _ => defaultPasswordHash)
            .RuleFor(o => o.Activo, _ => true)
            .RuleFor(o => o.CreatedAt, f => DateTime.SpecifyKind(f.Date.Past(1), DateTimeKind.Utc));

        var oficiales = new List<Oficial>();
        for (int i = 0; i < 500; i++)
        {
            var oficial = oficialesFaker.Generate();
            oficial.NumeroEscalafon = $"ESC-{(1000 + i):D4}";
            oficiales.Add(oficial);
        }

        await context.Oficiales.AddRangeAsync(oficiales);
        await context.SaveChangesAsync();

        var oficialIds = oficiales.Select(o => o.Id).ToList();

        var barrios = new[] { "San Roque", "El Molino", "La Pampa", "Miraflores", "Senac", "Morros Blancos", "Tabladita", "Lourdes", "San Martin", "Aranjuez" };
        var tiposAccidente = new[] { "Colision frontal", "Choque a objeto fijo", "Atropello", "Vuelco", "Colision por alcance", "Raspaje" };
        var gravedades = new[] { "solo_danos", "con_heridos", "con_fallecidos" };
        var condicionesClima = new[] { "Despejado", "Nublado", "Lluvia moderada", "Lluvia intensa", "Niebla" };
        var estadosCalzada = new[] { "Seca", "Mojada", "Resbaladiza", "En reparacion", "Pavimentada" };
        var estadosCaso = new[] { "en_investigacion", "remitido_fiscalia", "concluido", "desestimado" };
        var instituciones = new[] { "Fiscalia Departamental de Tarija", "Juzgado Cautelar 1", "Juzgado de Transito" };

        var accidentes = new List<Accidente>();
        var accFaker = new Faker("es");

        for (int i = 0; i < 500; i++)
        {
            var fecha = DateTime.SpecifyKind(accFaker.Date.Past(1), DateTimeKind.Utc);
            var estado = accFaker.PickRandom(estadosCaso);

            accidentes.Add(new Accidente
            {
                CodigoCaso = $"CAS-2024-{(10000 + i):D5}",
                FechaHora = fecha,
                TipoAccidente = accFaker.PickRandom(tiposAccidente),
                Gravedad = accFaker.PickRandom(gravedades),
                ZonaBarrio = accFaker.PickRandom(barrios),
                DireccionReferencia = $"Av. {accFaker.Address.StreetName()} y Calle {accFaker.Address.StreetName()}",
                Distrito = $"Distrito {accFaker.Random.Int(1, 13)}",
                Latitud = -21.5355 + (accFaker.Random.Double() - 0.5) * 0.05,
                Longitud = -64.7296 + (accFaker.Random.Double() - 0.5) * 0.05,
                CondicionClimatica = accFaker.PickRandom(condicionesClima),
                EstadoCalzada = accFaker.PickRandom(estadosCalzada),
                OficialAsignadoId = accFaker.PickRandom(oficialIds),
                EstadoCaso = estado,
                JuzgadoOInstitucion = estado == "remitido_fiscalia" ? accFaker.PickRandom(instituciones) : null,
                NumeroCuadernoInvestigacion = estado == "remitido_fiscalia" ? $"CUAD-{accFaker.Random.Int(100, 999)}/24" : null,
                MotivoPase = estado == "remitido_fiscalia" ? "Remision para determinacion de responsabilidad legal" : null,
                Observaciones = accFaker.Lorem.Sentence(),
                CreatedAt = fecha
            });
        }

        await context.Accidentes.AddRangeAsync(accidentes);
        await context.SaveChangesAsync();

        var accidenteIds = accidentes.Select(a => a.Id).ToList();

        var marcas = new[] { "Toyota", "Nissan", "Suzuki", "Hyundai", "Kia", "Ford", "Chevrolet", "Mitsubishi" };
        var modelos = new[] { "Corolla", "Hilux", "Sentra", "Grand Vitara", "Tucson", "Sportage", "Ranger" };
        var tiposVehiculo = new[] { "Automovil", "Camioneta", "Motocicleta", "Minibus", "Camion", "Microbus" };
        var colores = new[] { "Blanco", "Negro", "Plateado", "Rojo", "Azul", "Gris", "Verde" };
        var aseguradoras = new[] { "Illimani", "Credinform", "Alianza", "Bisa Seguros", "Nacional Seguros" };
        var danios = new[] { "Parachoques delantero abollado", "Rotura de faros y parabrisas", "Danos de consideracion en lateral derecho", "Danos leves en chaperia", "Perdida total" };
        var postas = new[] { "Posta Policial Central", "Posta Morros Blancos", "Garaje Municipal" };

        var vehiculos = new List<VehiculoInvolucrado>();
        var vehFaker = new Faker("es");

        for (int i = 0; i < 500; i++)
        {
            var tieneSoat = vehFaker.Random.Bool(0.85f);
            vehiculos.Add(new VehiculoInvolucrado
            {
                AccidenteId = accidenteIds[i % accidenteIds.Count],
                Placa = $"{vehFaker.Random.Int(1000, 9999)}-{vehFaker.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}",
                Marca = vehFaker.PickRandom(marcas),
                Modelo = vehFaker.PickRandom(modelos),
                TipoVehiculo = vehFaker.PickRandom(tiposVehiculo),
                Color = vehFaker.PickRandom(colores),
                NumeroChasis = vehFaker.Random.AlphaNumeric(17).ToUpper(),
                SoatVigente = tieneSoat,
                Aseguradora = tieneSoat ? vehFaker.PickRandom(aseguradoras) : null,
                DescripcionDanios = vehFaker.PickRandom(danios),
                RemolcadoA = vehFaker.PickRandom(postas),
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.VehiculosInvolucrados.AddRangeAsync(vehiculos);
        await context.SaveChangesAsync();

        var vehiculoIds = vehiculos.Select(v => v.Id).ToList();

        var roles = new[] { "Conductor", "Pasajero", "Peaton", "Testigo" };
        var categoriasLic = new[] { "Particular A", "Profesional B", "Profesional C", "Motociclista M" };
        var estadosSalud = new[] { "Ileso", "Heridas leves", "Policontuso", "Grave", "Fallecido" };
        var centrosMedicos = new[] { "Hospital Regional San Juan de Dios", "Clinica Yapur", "Hospital Obrero" };
        var diagnosticos = new[] { "Traumatismo encefalocraneal leve", "Contusion toracica", "Sin lesiones aparentes", "Fractura de miembro inferior" };

        var personas = new List<PersonaInvolucrada>();
        var perFaker = new Faker("es");

        for (int i = 0; i < 500; i++)
        {
            var rol = perFaker.PickRandom(roles);
            var gradoAlc = perFaker.Random.Bool(0.25f) ? Math.Round(perFaker.Random.Double(0.1, 2.5), 2) : 0.0;
            var estadoAlc = gradoAlc > 0.5 ? "ebriedad" : (gradoAlc > 0.0 ? "sancionable" : "sobrio");
            var salud = perFaker.PickRandom(estadosSalud);

            personas.Add(new PersonaInvolucrada
            {
                AccidenteId = accidenteIds[i % accidenteIds.Count],
                VehiculoId = rol == "Peaton" ? null : vehiculoIds[i % vehiculoIds.Count],
                Nombres = perFaker.Name.FirstName(),
                Apellidos = perFaker.Name.LastName(),
                Ci = $"{perFaker.Random.Int(1000000, 9999999)} TJ",
                Telefono = $"{perFaker.Random.Int(60000000, 79999999)}",
                Rol = rol,
                NumeroLicencia = rol == "Conductor" ? $"LIC-{perFaker.Random.Int(1000000, 9999999)}" : null,
                CategoriaLicencia = rol == "Conductor" ? perFaker.PickRandom(categoriasLic) : null,
                GradoAlcoholemiaGral = gradoAlc,
                EstadoAlcoholemia = estadoAlc,
                NumeroActaDosaje = gradoAlc > 0 ? $"ACTA-{perFaker.Random.Int(100, 999)}" : null,
                Laboratorio = gradoAlc > 0 ? "Laboratorio IITCUP Tarija" : null,
                EstadoSalud = salud,
                CentroMedicoTraslado = salud != "Ileso" ? perFaker.PickRandom(centrosMedicos) : null,
                DiagnosticoPreliminar = salud != "Ileso" ? perFaker.PickRandom(diagnosticos) : "Sin lesiones",
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.PersonasInvolucradas.AddRangeAsync(personas);
        await context.SaveChangesAsync();

        var factoresDeterminantes = new[]
        {
            "Exceso de velocidad",
            "Inobservancia de senal de transito",
            "Estado de ebriedad",
            "Falla mecanica en sistema de frenos",
            "Distraccion por dispositivo movil",
            "Imprudencia del peaton"
        };

        var peritajes = new List<Peritaje>();
        var periFaker = new Faker("es");

        for (int i = 0; i < 500; i++)
        {
            peritajes.Add(new Peritaje
            {
                AccidenteId = accidenteIds[i % accidenteIds.Count],
                PeritoId = periFaker.PickRandom(oficialIds),
                FechaPeritaje = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-periFaker.Random.Int(1, 100))),
                VelocidadEstimadaImpactoKmh = Math.Round(periFaker.Random.Double(20.0, 110.0), 1),
                HuellaFrenadoMetros = Math.Round(periFaker.Random.Double(0.0, 35.0), 1),
                FactorDeterminante = periFaker.PickRandom(factoresDeterminantes),
                Conclusiones = periFaker.Lorem.Paragraph(),
                CroquisUrl = $"https://storage.transito-tarija.gob.bo/croquis/croquis_{(1000 + i)}.jpg",
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.Peritajes.AddRangeAsync(peritajes);
        await context.SaveChangesAsync();
    }
}
