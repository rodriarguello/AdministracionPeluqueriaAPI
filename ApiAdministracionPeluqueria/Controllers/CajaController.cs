using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.IngresoDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/caja")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CajaController : ControllerBase
    {
        private readonly ICajaService _cajaService;

        public CajaController(ICajaService cajaService)
        {
            _cajaService = cajaService;
        }

        [HttpGet("{anio:int}")]
        public async Task<ActionResult<ResIngresos>> GetIngresoAnual(int anio)
        {
            try
            {
                var idUsuario = ExtraerIdClaim();

                var ingresos = await _cajaService.GetIngresoAnualAsync(anio, idUsuario);

                return Ok(ingresos);

            }
            catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

        [HttpGet("{anio:int}/{mes:int}")]
        public async Task<ActionResult<ResIngresos>> GetIngresoMensual(int anio, int mes)
        {

            try
            {
                var idUsuario = ExtraerIdClaim();

                var ingresos = await _cajaService.GetIngresoMensualAsync(mes, anio, idUsuario);

                return Ok(ingresos);

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }


        [HttpGet("{anio:int}/{mes:int}/{dia:int}")]
        public async Task<ActionResult<ResIngresos>> GetIngresoDiario(int anio, int mes, int dia)
        {
            try
            {
                var idUsuario = ExtraerIdClaim();

                var ingresos = await _cajaService.GetIngresoDiarioAsync(anio, mes, dia, idUsuario);

                return Ok(ingresos);

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

        private string ExtraerIdClaim()
        {
            var claimId = HttpContext.User.Claims.Where(claim => claim.Type == "id").FirstOrDefault();
            
            return claimId.Value;
        }
        
    }
}
