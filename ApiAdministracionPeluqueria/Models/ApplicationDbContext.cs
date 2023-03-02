using ApiAdministracionPeluqueria.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Creacion de tablas en la base de datos
        public DbSet<Alergia> Alergias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Enfermedad> Enfermedades { get; set; }

        public DbSet<Fecha> Fechas { get; set; }

        public DbSet<Horario> Horarios { get; set; }

        public DbSet<Mascota> Mascotas { get; set; }

        public DbSet<Raza> Razas { get; set; }

        public DbSet<Turno> Turnos { get; set; }

        public DbSet<Calendario> Calendarios { get; set; }
        
    }
}
