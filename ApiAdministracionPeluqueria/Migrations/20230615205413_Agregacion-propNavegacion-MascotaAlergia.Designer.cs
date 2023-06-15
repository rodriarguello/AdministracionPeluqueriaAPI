﻿// <auto-generated />
using System;
using ApiAdministracionPeluqueria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230615205413_Agregacion-propNavegacion-MascotaAlergia")]
    partial class AgregacionpropNavegacionMascotaAlergia
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Alergia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Alergias");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Calendario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("HoraFinTurnos")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("HoraInicioTurnos")
                        .HasColumnType("time(6)");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("IntervaloTurnos")
                        .HasColumnType("time(6)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Calendarios");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Enfermedad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Enfermedades");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Fecha", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Dia")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdCalendario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Fechas");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Horario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("time(6)");

                    b.Property<int>("IdCalendario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Horarios");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Mascota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<int>("IdRaza")
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("RazaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("RazaId");

                    b.ToTable("Mascotas");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.MascotaAlergia", b =>
                {
                    b.Property<int>("IdMascota")
                        .HasColumnType("int");

                    b.Property<int>("IdAlergia")
                        .HasColumnType("int");

                    b.Property<int>("AlergiaId")
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MascotaId")
                        .HasColumnType("int");

                    b.HasKey("IdMascota", "IdAlergia");

                    b.HasIndex("AlergiaId");

                    b.HasIndex("MascotaId");

                    b.ToTable("MascotasAlergias");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.MascotaEnfermedad", b =>
                {
                    b.Property<int>("IdMascota")
                        .HasColumnType("int");

                    b.Property<int>("IdEnfermedad")
                        .HasColumnType("int");

                    b.Property<int>("EnfermedadId")
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MascotaId")
                        .HasColumnType("int");

                    b.HasKey("IdMascota", "IdEnfermedad");

                    b.HasIndex("EnfermedadId");

                    b.HasIndex("MascotaId");

                    b.ToTable("MascotasEnfermedades");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Raza", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Razas");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Turno", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool?>("Asistio")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Disponible")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("FechaId")
                        .HasColumnType("int");

                    b.Property<int>("HorarioId")
                        .HasColumnType("int");

                    b.Property<int>("IdCalendario")
                        .HasColumnType("int");

                    b.Property<int>("IdFecha")
                        .HasColumnType("int");

                    b.Property<int>("IdHorario")
                        .HasColumnType("int");

                    b.Property<int?>("IdMascota")
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("MascotaId")
                        .HasColumnType("int");

                    b.Property<int?>("Precio")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FechaId");

                    b.HasIndex("HorarioId");

                    b.HasIndex("MascotaId");

                    b.ToTable("Turnos");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Usuario", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Apellido")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NombrePeluqueria")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nombres")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Calendario", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Mascota", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Cliente", "Cliente")
                        .WithMany("Mascotas")
                        .HasForeignKey("ClienteId");

                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Raza", "Raza")
                        .WithMany()
                        .HasForeignKey("RazaId");

                    b.Navigation("Cliente");

                    b.Navigation("Raza");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.MascotaAlergia", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Alergia", "Alergia")
                        .WithMany()
                        .HasForeignKey("AlergiaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Mascota", "Mascota")
                        .WithMany("Alergias")
                        .HasForeignKey("MascotaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alergia");

                    b.Navigation("Mascota");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.MascotaEnfermedad", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Enfermedad", "Enfermedad")
                        .WithMany()
                        .HasForeignKey("EnfermedadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Mascota", "Mascota")
                        .WithMany("Enfermedades")
                        .HasForeignKey("MascotaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enfermedad");

                    b.Navigation("Mascota");
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
                        .HasForeignKey("MascotaId");

                    b.Navigation("Fecha");

                    b.Navigation("Horario");

                    b.Navigation("Mascota");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ApiAdministracionPeluqueria.Models.Entidades.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Cliente", b =>
                {
                    b.Navigation("Mascotas");
                });

            modelBuilder.Entity("ApiAdministracionPeluqueria.Models.Entidades.Mascota", b =>
                {
                    b.Navigation("Alergias");

                    b.Navigation("Enfermedades");

                    b.Navigation("Turno");
                });
#pragma warning restore 612, 618
        }
    }
}
