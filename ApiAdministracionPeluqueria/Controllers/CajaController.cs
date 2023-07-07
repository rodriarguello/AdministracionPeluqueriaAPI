using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/caja")]
    [ApiController]
    public class CajaController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CajaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("diario/{dia:int}")]
        public async Task<ActionResult> MostrarIngresosPorDia()
        {

            return Ok();
        }

        [HttpGet("mensual/{mes:int}")]
        public async Task<ActionResult> MostrarIngresosPorMes()
        {

            return Ok();
        }

        [HttpGet("anual/{anio:int}")]
        public async Task<ActionResult> MostrarIngresosPorAnio()
        {

            return Ok();
        }

        

    }
}
