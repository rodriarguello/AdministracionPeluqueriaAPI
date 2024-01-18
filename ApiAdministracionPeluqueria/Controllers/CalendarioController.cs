using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/calendario")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CalendarioController : ControllerBase
    {
        private readonly ICalendarioService _calendarioService;

        public CalendarioController(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        [HttpGet]

        public async Task<ActionResult<CalendarioDTO>> GetCalendario()
        {

            try
            {
                var idUsuario = ExtraerClaim("id");

                var calendario = await _calendarioService.GetByIdUserAsync(idUsuario);

                return Ok(calendario);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

       


        [HttpPost]
        public async Task<ActionResult<CalendarioDTO>> CrearCalendario([FromBody] CalendarioCreacionDTO nuevoCalendarioDTO)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var calendario = await _calendarioService.CreateAsync(idUsuario, nuevoCalendarioDTO);

                return Ok(calendario);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500,"Error interno del servidor");
            }


        }

        [HttpPut("modificarnombre")]
        public async Task<ActionResult<CalendarioDTO>> ModificarNombreCalendario(string nuevoNombre)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var calendario = await _calendarioService.UpdateNameAsync(nuevoNombre, idUsuario);

                return Ok(calendario);
            }
            catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }


        }


        [HttpPut("agregarturnos")]
        public async Task<ActionResult<CalendarioDTO>> AgregarTurnos([FromBody]ExtenderCalendarioDTO extenderCalendarioDTO)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var calendario = await _calendarioService.ExtendAsync(extenderCalendarioDTO, idUsuario);

                return Ok(calendario);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }


        }


        [HttpPut("eliminarturnos/{nuevaFechaFin}")]
        public async Task<ActionResult<CalendarioDTO>> EliminarTurnos([FromRoute] DateTime nuevaFechaFin)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var calendario = await _calendarioService.ReduceAsync(nuevaFechaFin, idUsuario);

                return Ok(calendario);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MensajePersonalizadoException ex)
            {
                return StatusCode(499, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                await _calendarioService.DeleteAsync(id, idUsuario);

                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
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
        private string ExtraerClaim(string tipoClaim)
        {
            var claim = HttpContext.User.Claims.Where(c => c.Type == tipoClaim).FirstOrDefault();

            return claim.Value;
        }

    }
}
