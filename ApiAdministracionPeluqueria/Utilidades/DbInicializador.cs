using ApiAdministracionPeluqueria.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Utilidades
{
    public class DbInicializador
    {
        private readonly ApplicationDbContext context;

        public DbInicializador(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void InicializarDb()
        {
            try
            {
                if (context.Database.GetPendingMigrations().Count() > 0)
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
