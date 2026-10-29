using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TransitoTarijaAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "oficiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombres = table.Column<string>(type: "text", nullable: false),
                    Apellidos = table.Column<string>(type: "text", nullable: false),
                    Grado = table.Column<string>(type: "text", nullable: true),
                    NumeroEscalafon = table.Column<string>(type: "text", nullable: false),
                    Cargo = table.Column<string>(type: "text", nullable: true),
                    Unidad = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oficiales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "accidentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoCaso = table.Column<string>(type: "text", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoAccidente = table.Column<string>(type: "text", nullable: true),
                    Gravedad = table.Column<string>(type: "text", nullable: false, defaultValue: "solo_danos"),
                    ZonaBarrio = table.Column<string>(type: "text", nullable: true),
                    DireccionReferencia = table.Column<string>(type: "text", nullable: true),
                    Distrito = table.Column<string>(type: "text", nullable: true),
                    Latitud = table.Column<double>(type: "double precision", nullable: true),
                    Longitud = table.Column<double>(type: "double precision", nullable: true),
                    CondicionClimatica = table.Column<string>(type: "text", nullable: true),
                    EstadoCalzada = table.Column<string>(type: "text", nullable: true),
                    OficialAsignadoId = table.Column<int>(type: "integer", nullable: true),
                    EstadoCaso = table.Column<string>(type: "text", nullable: false, defaultValue: "en_investigacion"),
                    JuzgadoOInstitucion = table.Column<string>(type: "text", nullable: true),
                    NumeroCuadernoInvestigacion = table.Column<string>(type: "text", nullable: true),
                    MotivoPase = table.Column<string>(type: "text", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accidentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accidentes_oficiales_OficialAsignadoId",
                        column: x => x.OficialAsignadoId,
                        principalTable: "oficiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "peritajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccidenteId = table.Column<int>(type: "integer", nullable: false),
                    PeritoId = table.Column<int>(type: "integer", nullable: true),
                    FechaPeritaje = table.Column<DateOnly>(type: "date", nullable: true),
                    VelocidadEstimadaImpactoKmh = table.Column<double>(type: "double precision", nullable: true),
                    HuellaFrenadoMetros = table.Column<double>(type: "double precision", nullable: true),
                    FactorDeterminante = table.Column<string>(type: "text", nullable: true),
                    Conclusiones = table.Column<string>(type: "text", nullable: true),
                    CroquisUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peritajes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_peritajes_accidentes_AccidenteId",
                        column: x => x.AccidenteId,
                        principalTable: "accidentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_peritajes_oficiales_PeritoId",
                        column: x => x.PeritoId,
                        principalTable: "oficiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "vehiculos_involucrados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccidenteId = table.Column<int>(type: "integer", nullable: false),
                    Placa = table.Column<string>(type: "text", nullable: false),
                    Marca = table.Column<string>(type: "text", nullable: true),
                    Modelo = table.Column<string>(type: "text", nullable: true),
                    TipoVehiculo = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    NumeroChasis = table.Column<string>(type: "text", nullable: true),
                    SoatVigente = table.Column<bool>(type: "boolean", nullable: false),
                    Aseguradora = table.Column<string>(type: "text", nullable: true),
                    DescripcionDanios = table.Column<string>(type: "text", nullable: true),
                    RemolcadoA = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculos_involucrados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vehiculos_involucrados_accidentes_AccidenteId",
                        column: x => x.AccidenteId,
                        principalTable: "accidentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personas_involucradas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccidenteId = table.Column<int>(type: "integer", nullable: false),
                    VehiculoId = table.Column<int>(type: "integer", nullable: true),
                    Nombres = table.Column<string>(type: "text", nullable: false),
                    Apellidos = table.Column<string>(type: "text", nullable: false),
                    Ci = table.Column<string>(type: "text", nullable: true),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Rol = table.Column<string>(type: "text", nullable: true),
                    NumeroLicencia = table.Column<string>(type: "text", nullable: true),
                    CategoriaLicencia = table.Column<string>(type: "text", nullable: true),
                    GradoAlcoholemiaGral = table.Column<double>(type: "double precision", nullable: false),
                    EstadoAlcoholemia = table.Column<string>(type: "text", nullable: false),
                    NumeroActaDosaje = table.Column<string>(type: "text", nullable: true),
                    Laboratorio = table.Column<string>(type: "text", nullable: true),
                    EstadoSalud = table.Column<string>(type: "text", nullable: true),
                    CentroMedicoTraslado = table.Column<string>(type: "text", nullable: true),
                    DiagnosticoPreliminar = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personas_involucradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_personas_involucradas_accidentes_AccidenteId",
                        column: x => x.AccidenteId,
                        principalTable: "accidentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_personas_involucradas_vehiculos_involucrados_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculos_involucrados",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_accidentes_CodigoCaso",
                table: "accidentes",
                column: "CodigoCaso",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accidentes_OficialAsignadoId",
                table: "accidentes",
                column: "OficialAsignadoId");

            migrationBuilder.CreateIndex(
                name: "IX_oficiales_NumeroEscalafon",
                table: "oficiales",
                column: "NumeroEscalafon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_peritajes_AccidenteId",
                table: "peritajes",
                column: "AccidenteId");

            migrationBuilder.CreateIndex(
                name: "IX_peritajes_PeritoId",
                table: "peritajes",
                column: "PeritoId");

            migrationBuilder.CreateIndex(
                name: "IX_personas_involucradas_AccidenteId",
                table: "personas_involucradas",
                column: "AccidenteId");

            migrationBuilder.CreateIndex(
                name: "IX_personas_involucradas_VehiculoId",
                table: "personas_involucradas",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculos_involucrados_AccidenteId",
                table: "vehiculos_involucrados",
                column: "AccidenteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "peritajes");

            migrationBuilder.DropTable(
                name: "personas_involucradas");

            migrationBuilder.DropTable(
                name: "vehiculos_involucrados");

            migrationBuilder.DropTable(
                name: "accidentes");

            migrationBuilder.DropTable(
                name: "oficiales");
        }
    }
}
