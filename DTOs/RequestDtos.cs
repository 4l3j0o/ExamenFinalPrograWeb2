namespace TransitoTarijaAPI.DTOs;

public record LoginRequest(string NumeroEscalafon, string Password);

public record LoginResponse(string Token, OficialDto Oficial);

public record RegistrarOficialRequest(
    string Nombres,
    string Apellidos,
    string? Grado,
    string NumeroEscalafon,
    string? Cargo,
    string? Unidad,
    bool Activo = true,
    string? Password = null
);

public record OficialDto(
    int Id, string Nombres, string Apellidos,
    string? Grado, string? Cargo, string? Unidad
);

public record RegistrarAccidenteRequest(
    string CodigoCaso,
    DateTime FechaHora,
    string? TipoAccidente,
    string Gravedad,
    string? ZonaBarrio,
    string? DireccionReferencia,
    string? Distrito,
    double? Latitud,
    double? Longitud,
    string? CondicionClimatica,
    string? EstadoCalzada,
    int? OficialAsignadoId,
    string? Observaciones
);

public record ActualizarEstadoRequest(
    string? EstadoCaso,
    string? JuzgadoOInstitucion,
    string? NumeroCuadernoInvestigacion,
    string? MotivoPase
);

public record AsociarVehiculoRequest(
    int AccidenteId,
    string Placa,
    string? Marca,
    string? Modelo,
    string? TipoVehiculo,
    string? Color,
    string? NumeroChasis,
    bool SoatVigente,
    string? Aseguradora,
    string? DescripcionDanios,
    string? RemolcadoA
);

public record RegistrarPersonaRequest(
    int AccidenteId,
    int? VehiculoId,
    string Nombres,
    string Apellidos,
    string? Ci,
    string? Telefono,
    string? Rol,
    string? NumeroLicencia,
    string? CategoriaLicencia,
    double GradoAlcoholemiaGral,
    string? EstadoAlcoholemia,
    string? EstadoSalud,
    string? CentroMedicoTraslado,
    string? DiagnosticoPreliminar
);

public record ActualizarAlcoholemiaRequest(
    double? GradoAlcoholemiaGral,
    string? EstadoAlcoholemia,
    string? NumeroActaDosaje,
    string? Laboratorio
);

public record RegistrarPeritajeRequest(
    int AccidenteId,
    int? PeritoId,
    DateOnly? FechaPeritaje,
    double? VelocidadEstimadaImpactoKmh,
    double? HuellaFrenadoMetros,
    string? FactorDeterminante,
    string? Conclusiones,
    string? CroquisUrl
);
