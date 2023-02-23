﻿// <auto-generated />
using System;
using ApiAdministracionPeluqueria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230223134132_Creacion_BaseDatos")]
    partial class Creacion_BaseDatos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Alergia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("MascotaId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MascotaId");

                    b.ToTable("Alergias");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdMascota")
                        .HasColumnType("int");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Enfermedad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("MascotaId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MascotaId");

                    b.ToTable("Enfermedades");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Fecha", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Dia")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Fechas");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Horario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("Horarios");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Mascota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdAlergia")
                        .HasColumnType("int");

                    b.Property<int>("IdEnfermedad")
                        .HasColumnType("int");

                    b.Property<int>("IdRaza")
                        .HasColumnType("int");

                    b.Property<int>("IdTurno")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Razaid")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("Razaid");

                    b.ToTable("Mascotas");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Raza", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Razas");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Turno", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Asistio")
                        .HasColumnType("bit");

                    b.Property<bool>("Disponible")
                        .HasColumnType("bit");

                    b.Property<int>("FechaId")
                        .HasColumnType("int");

                    b.Property<int>("HorarioId")
                        .HasColumnType("int");

                    b.Property<int>("IdFecha")
                        .HasColumnType("int");

                    b.Property<int>("IdHorario")
                        .HasColumnType("int");

                    b.Property<int>("IdMascota")
                        .HasColumnType("int");

                    b.Property<int>("MascotaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FechaId");

                    b.HasIndex("HorarioId");

                    b.HasIndex("MascotaId");

                    b.ToTable("Turnos");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Alergia", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Mascota", null)
                        .WithMany("Alergia")
                        .HasForeignKey("MascotaId");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Enfermedad", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Mascota", null)
                        .WithMany("Enfermedad")
                        .HasForeignKey("MascotaId");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Mascota", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Cliente", null)
                        .WithMany("Mascota")
                        .HasForeignKey("ClienteId");

                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Raza", "Raza")
                        .WithMany()
                        .HasForeignKey("Razaid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Raza");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Turno", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Fecha", "Fecha")
                        .WithMany()
                        .HasForeignKey("FechaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Horario", "Horario")
                        .WithMany()
                        .HasForeignKey("HorarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Mascota", "Mascota")
                        .WithMany("Turno")
                        .HasForeignKey("MascotaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fecha");

                    b.Navigation("Horario");

                    b.Navigation("Mascota");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Cliente", b =>
                {
                    b.Navigation("Mascota");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Mascota", b =>
                {
                    b.Navigation("Alergia");

                    b.Navigation("Enfermedad");

                    b.Navigation("Turno");
                });
#pragma warning restore 612, 618
        }
    }
}