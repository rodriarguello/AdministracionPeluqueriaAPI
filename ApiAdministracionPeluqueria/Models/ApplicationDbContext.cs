using ApiAdministracionPeluqueria.Models.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Models
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Creacion de tablas en la base de datos
        public DbSet<Alergia> Alergias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Enfermedad> Enfermedades { get; set; }

        public DbSet<Mascota> Mascotas { get; set; }

        public DbSet<Raza> Razas { get; set; }

        public DbSet<Turno> Turnos { get; set; }

        public DbSet<Calendario> Calendarios { get; set; }

        public DbSet<MascotaEnfermedad>  MascotasEnfermedades { get; set; }

        public DbSet<MascotaAlergia> MascotasAlergias { get; set; }

        public DbSet<Ingreso> Ingresos { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<MascotaEnfermedad>().HasKey(x=> new {x.IdMascota,x.IdEnfermedad});

            builder.Entity<MascotaAlergia>().HasKey(x => new {x.IdMascota,x.IdAlergia});

            base.OnModelCreating(builder);
        }

    }
}
