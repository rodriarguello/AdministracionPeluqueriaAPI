using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Schema;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/caja")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CajaController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Usuario> userManager;
        private readonly ResponseApi response;

        public CajaController(ApplicationDbContext context, UserManager<Usuario> userManager, ResponseApi response)
        {
            this.context = context;
            this.userManager = userManager;
            this.response = response;
        }

        [HttpGet("{anio:int}")]
        public async Task<ActionResult<ModeloRespuesta>> MostrarIngresosPorAnio(int anio)
        {

            if (anio > DateTime.Now.Year) return response.respuestaError("No se puede enviar un año mayor al actual"); 

            var usuario = await ObtenerUsuario();

            
            var ingresos = await context.Caja.Where(ingreso=> ingreso.IdUsuario == usuario.Id)
                                             .Where(ingreso=>ingreso.Fecha.Year== anio)
                                             .ToListAsync();
            var totalIngresos = ingresos.Sum(ingreso=>ingreso.Precio);

            

            return response.respuestaExitosa(new
            {
                cantidadIngresos = ingresos.Count(),
                total = totalIngresos

            });
        }

        [HttpGet("{anio:int}/{mes:int}")]
        public async Task<ActionResult<ModeloRespuesta>> MostrarIngresosPorMes(int anio, int mes)
        {

            if (anio > DateTime.Now.Year) return response.respuestaError("No se puede enviar un año mayor al actual");
            
            var usuario = await ObtenerUsuario();

            var ingresos = await context.Caja.Where(ingreso=>ingreso.IdUsuario == usuario.Id)
                                             .Where(ingreso => ingreso.Fecha.Year == anio && ingreso.Fecha.Month == mes)
                                             .ToListAsync();

            var totalIngresos = ingresos.Sum(ingreso=>ingreso.Precio);

            return response.respuestaExitosa(new {cantidadIngresos = ingresos.Count(), total = totalIngresos});
        }


        [HttpGet("{anio:int}/{mes:int}/{dia:int}")]
        public async Task<ActionResult<ModeloRespuesta>> MostrarIngresosPorDia(int anio, int mes, int dia)
        {


            if (anio > DateTime.Now.Year) return response.respuestaError("No se puede enviar un año mayor al actual");

            var usuario = await ObtenerUsuario();

            var fecha = new DateTime(anio,mes,dia);

            var ingresos = await context.Caja.Where(ingreso=>ingreso.IdUsuario == usuario.Id).Where(ingreso=>ingreso.Fecha.Date == fecha.Date).ToListAsync();

            var totalIngresos = ingresos.Sum(ingreso=> ingreso.Precio);



            return response.respuestaExitosa(new {cantidadIngresos = ingresos.Count(), total = totalIngresos});
        }
        
        private async Task<Usuario> ObtenerUsuario()
        {
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var claimValue = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(claimValue);

            if (usuario == null) return null;

            return usuario;
        }

    }
}
