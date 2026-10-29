using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TransitoTarijaAPI.Data;

#nullable disable

namespace TransitoTarijaAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261029001246_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TransitoTarijaAPI.Models.Accidente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CodigoCaso")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CondicionClimatica")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DireccionReferencia")
                        .HasColumnType("text");

                    b.Property<string>("Distrito")
                        .HasColumnType("text");

                    b.Property<string>("EstadoCalzada")
                        .HasColumnType("text");

                    b.Property<string>("EstadoCaso")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("en_investigacion");

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Gravedad")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("solo_danos");

                    b.Property<string>("JuzgadoOInstitucion")
                        .HasColumnType("text");

                    b.Property<double?>("Latitud")
                        .HasColumnType("double precision");

                    b.Property<double?>("Longitud")
                        .HasColumnType("double precision");

                    b.Property<string>("MotivoPase")
                        .HasColumnType("text");

                    b.Property<string>("NumeroCuadernoInvestigacion")
                        .HasColumnType("text");

                    b.Property<string>("Observaciones")
                        .HasColumnType("text");

                    b.Property<int?>("OficialAsignadoId")
                        .HasColumnType("integer");

                    b.Property<string>("TipoAccidente")
                        .HasColumnType("text");

                    b.Property<string>("ZonaBarrio")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CodigoCaso")
                        .IsUnique();

                    b.HasIndex("OficialAsignadoId");

                    b.ToTable("accidentes");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.Oficial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("boolean");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cargo")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Grado")
                        .HasColumnType("text");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumeroEscalafon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Unidad")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NumeroEscalafon")
                        .IsUnique();

                    b.ToTable("oficiales");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.Peritaje", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccidenteId")
                        .HasColumnType("integer");

                    b.Property<string>("Conclusiones")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CroquisUrl")
                        .HasColumnType("text");

                    b.Property<string>("FactorDeterminante")
                        .HasColumnType("text");

                    b.Property<DateOnly?>("FechaPeritaje")
                        .HasColumnType("date");

                    b.Property<double?>("HuellaFrenadoMetros")
                        .HasColumnType("double precision");

                    b.Property<int?>("PeritoId")
                        .HasColumnType("integer");

                    b.Property<double?>("VelocidadEstimadaImpactoKmh")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AccidenteId");

                    b.HasIndex("PeritoId");

                    b.ToTable("peritajes");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.PersonaInvolucrada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccidenteId")
                        .HasColumnType("integer");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CategoriaLicencia")
                        .HasColumnType("text");

                    b.Property<string>("CentroMedicoTraslado")
                        .HasColumnType("text");

                    b.Property<string>("Ci")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DiagnosticoPreliminar")
                        .HasColumnType("text");

                    b.Property<string>("EstadoAlcoholemia")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EstadoSalud")
                        .HasColumnType("text");

                    b.Property<double>("GradoAlcoholemiaGral")
                        .HasColumnType("double precision");

                    b.Property<string>("Laboratorio")
                        .HasColumnType("text");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumeroActaDosaje")
                        .HasColumnType("text");

                    b.Property<string>("NumeroLicencia")
                        .HasColumnType("text");

                    b.Property<string>("Rol")
                        .HasColumnType("text");

                    b.Property<string>("Telefono")
                        .HasColumnType("text");

                    b.Property<int?>("VehiculoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccidenteId");

                    b.HasIndex("VehiculoId");

                    b.ToTable("personas_involucradas");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.VehiculoInvolucrado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccidenteId")
                        .HasColumnType("integer");

                    b.Property<string>("Aseguradora")
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DescripcionDanios")
                        .HasColumnType("text");

                    b.Property<string>("Marca")
                        .HasColumnType("text");

                    b.Property<string>("Modelo")
                        .HasColumnType("text");

                    b.Property<string>("NumeroChasis")
                        .HasColumnType("text");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RemolcadoA")
                        .HasColumnType("text");

                    b.Property<bool>("SoatVigente")
                        .HasColumnType("boolean");

                    b.Property<string>("TipoVehiculo")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccidenteId");

                    b.ToTable("vehiculos_involucrados");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.Accidente", b =>
                {
                    b.HasOne("TransitoTarijaAPI.Models.Oficial", "OficialAsignado")
                        .WithMany("AccidentesAsignados")
                        .HasForeignKey("OficialAsignadoId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("OficialAsignado");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.Peritaje", b =>
                {
                    b.HasOne("TransitoTarijaAPI.Models.Accidente", "Accidente")
                        .WithMany("Peritajes")
                        .HasForeignKey("AccidenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransitoTarijaAPI.Models.Oficial", "Perito")
                        .WithMany("Peritajes")
                        .HasForeignKey("PeritoId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Accidente");

                    b.Navigation("Perito");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.PersonaInvolucrada", b =>
                {
                    b.HasOne("TransitoTarijaAPI.Models.Accidente", "Accidente")
                        .WithMany("Personas")
                        .HasForeignKey("AccidenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransitoTarijaAPI.Models.VehiculoInvolucrado", "Vehiculo")
                        .WithMany()
                        .HasForeignKey("VehiculoId");

                    b.Navigation("Accidente");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.VehiculoInvolucrado", b =>
                {
                    b.HasOne("TransitoTarijaAPI.Models.Accidente", "Accidente")
                        .WithMany("Vehiculos")
                        .HasForeignKey("AccidenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accidente");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.Accidente", b =>
                {
                    b.Navigation("Peritajes");

                    b.Navigation("Personas");

                    b.Navigation("Vehiculos");
                });

            modelBuilder.Entity("TransitoTarijaAPI.Models.Oficial", b =>
                {
                    b.Navigation("AccidentesAsignados");

                    b.Navigation("Peritajes");
                });
#pragma warning restore 612, 618
        }
    }
}
