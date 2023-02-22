using ApiAdministracionPeluqueria.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

       
        

    }
}
